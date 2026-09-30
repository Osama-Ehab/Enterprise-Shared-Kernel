using AutoMapper;
using Enterprise.Application.Interfaces.Services;
using Enterprise.SharedKernel;
using Enterprise.SharedKernel.Application.Extensions;
using Enterprise.SharedKernel.DTOs.Interfaces;
using Enterprise.SharedKernel.Enums;
using Enterprise.SharedKernel.Interfaces;
using Enterprise.SharedKernel.Interfaces.Models;
using Enterprise.SharedKernel.Interfaces.Repositories;
using Enterprise.SharedKernel.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Enterprise.Application.Services
{
    public class BaseGenericService<TEntity, TKey> : IGenericService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        protected readonly ICurrentUser _currentUser; // متاح للخدمات المُولدة

      
        // 1. حقن المستودعين معاً
        protected readonly IReadRepository<TEntity, TKey> _readRepo;
        protected readonly IWriteRepository<TEntity, TKey> _writeRepo;

        protected readonly IMapper _mapper; // الـ Mapper مكانه الطبيعي هنا

        public BaseGenericService(
            IReadRepository<TEntity, TKey> readRepo,
            IWriteRepository<TEntity, TKey> writeRepo, IMapper mapper, IServiceScopeFactory  serviceScopeFactory, ICurrentUser  currentUser)
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
            _currentUser = currentUser;
        }

        // ==========================================
        // محرك التحقق الديناميكي (Validation Engine)
        // ==========================================
        protected Result ValidateDto<TDto>(TDto dto)
        {
            if (dto == null)
                return Result<bool>.Failure("Provided data cannot be null.", ErrorType.Validation);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var validator = scope.ServiceProvider.GetService<IValidator<TDto>>();

                if (validator == null) return Result.Success();

                return validator.ValidateToResult(dto);

            }
            
        }

        // ==========================================
        // عمليات القراءة (فائقة السرعة عبر مستودع القراءة)
        // ==========================================
        public virtual async Task<Result<TDto>> GetByIdProjectedAsync<TDto>(TKey id, Expression<Func<TEntity, TDto>> selectExpression)
        {
          
            return await ServiceExecutor.RunAsync<TDto>(async () =>
            {
                // جلب الكيان وتحويله في SQL مباشرة باستخدام دالة المستودع التي صممناها
                var dto = await _readRepo.GetByIdProjectedAsync(id, selectExpression);

                return Result<TDto>.Success(dto);

            });
        }

        public virtual async Task<Result<List<TDto>>> GetAllProjectedAsync<TDto>(Expression<Func<TEntity, TDto>> selectExpression)
        {
            return await ServiceExecutor.RunAsync<List<TDto>>(async () =>
            {
                var data = await _readRepo.GetAllProjectedAsync(selectExpression);
                return Result<List<TDto>>.Success(data);

            });
        }

        // ==========================================
        // عمليات الكتابة (آمنة عبر مستودع الكتابة)
        // ==========================================
        public virtual async Task<Result<TKey>> AddAsync<TCreateDTO>(TCreateDTO dto) where TCreateDTO : class
        {
            return await ServiceExecutor.RunAsync<TKey>(async () =>
            {
                // 1. FluentValidation
                var validationResult = ValidateDto(dto);
                if (!validationResult.IsSuccess)
                {
                    return Result<TKey>.Failure(validationResult.ErrorMessage, ErrorType.Validation);
                }

                // 1. تحويل الـ DTO إلى Entity داخل طبقة الخدمة
                TEntity entity = _mapper.Map<TEntity>(dto);

                bool IsExists = await _readRepo.AnyAsync(e => e.Id.Equals(entity.Id));

                if (IsExists)
                {
                    return Result<TKey>.Failure("The Id is already Exists.", ErrorType.ConstraintViolation);
                }

                // 3. Implicit Auditing (تتبع المستخدم الذي قام بالإضافة)
                if (entity is IAuditableEntity<byte, int> auditableEntity)
                {
                    auditableEntity.CreatedBy = _currentUser.UserId;
                    auditableEntity.CreatedDate = DateTime.UtcNow;
                }
                
                // 2. إرسال الكيان الجاهز للمستودع ليتم تتبعه وحفظه
                var newId = await _writeRepo.AddAsync(entity);

                return Result<TKey>.Success(newId);
            });
        }
        public async Task<Result>  UpdateAsync<TUpdateDTO>(TUpdateDTO dto) where TUpdateDTO : class,IIdentifiableDto<TKey>
        {
            return await ServiceExecutor.RunActionAsync(async () =>
            {
                // 1. FluentValidation
                var validationResult = ValidateDto(dto);
                if (!validationResult.IsSuccess)
                {
                    return Result<bool>.Failure(validationResult.ErrorMessage, ErrorType.Validation);
                }

                var entity = await _writeRepo.GetByIdAsync(dto.Id);
                if (entity == null) return Result.Failure("السجل غير موجود");

                _mapper.Map(dto, entity);

                // 3. Implicit Auditing for Update
                if (entity is IAuditableEntity<byte, int> auditableEntity)
                {
                    auditableEntity.ModifiedBy = _currentUser.UserId;
                    auditableEntity.ModifiedDate = DateTime.UtcNow;
                }

                // الحفظ
                var IsSuccess = await _writeRepo.UpdateAsync(entity);

                return IsSuccess ? Result.Success() : Result.Failure("Update Filure!");
            });
          
        }

        public virtual async Task<Result> DeleteAsync(TKey id)
        {
            return await ServiceExecutor.RunActionAsync(async () =>
            {
                var exists = await _readRepo.AnyAsync(e => e.Id.Equals(id));
                if (!exists)
                    return Result.Failure($"السجل برقم {id} غير موجود.");

                await _writeRepo.DeleteAsync(id);
                return Result.Success(); 
            });
        }
    }
}