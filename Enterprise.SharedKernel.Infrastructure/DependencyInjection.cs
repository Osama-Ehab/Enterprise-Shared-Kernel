// Infrastructure/DependencyInjection.cs
using Enterprise.SharedKernel.Infrastructure.Utilities;
using Enterprise.SharedKernel.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace Enterprise.SharedKernel.Infrastructure
{
    public  static partial class DependencyInjection
    {
        // هذه الدالة ستستدعيها الـ UI، ونمرر لها الـ Configuration
        public static  IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // استخراج الإعدادات في متغير لاستخدامها فوراً في نفس الدالة
            var errorConfigs = new Dictionary<int, ErrorConfig>();
            configuration.GetSection("SqlErrors").Bind(errorConfigs);

            // إنشاء الـ Mapper للخطاف
            var sqlMapper = new SqlErrorMapper(Options.Create(errorConfigs));

            // 3. ربط الخطاف (Hook) - وهو المكان الوحيد الذي سيستخدم هذا الـ Mapper
            ServiceExecutor.ExceptionMapperHook = (Exception ex) =>
            {
                if (ex is DbUpdateException dbEx && dbEx.InnerException is SqlException innerSql)
                    return (sqlMapper.GetMessage(innerSql), sqlMapper.GetErrorType(innerSql.Number));

                if (ex is SqlException sqlEx)
                    return (sqlMapper.GetMessage(sqlEx), sqlMapper.GetErrorType(sqlEx.Number));

                return null;
            };

            // 4. تسجيل باقي التبعيات العادية (DbContext, Repositories, etc.)
            // ...

            return services;
        }
    }
}