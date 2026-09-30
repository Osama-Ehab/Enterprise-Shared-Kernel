using Enterprise.SharedKernel.Enums;
using Enterprise.SharedKernel.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Enterprise.SharedKernel
{
    public static class ServiceExecutor
    {
        // =========================================================================
        // 💎 1. خطاف ترجمة الاستثناءات (Hook): يُحَقن من مشروع البنية التحتية عند إقلاع البرنامج
        // يستقبل الاستثناء (Exception) ويرجع (الرسالة، ونوع الخطأ ErrorType) في حال كان خطأ داتابيز
        // =========================================================================
        public static Func<Exception, (string ErrorMessage, ErrorType Type)?> ExceptionMapperHook { get; set; }

        // =========================================================================
        // 2. معالج العمليات التي ترجع بيانات (Result<T>)
        // =========================================================================
        public static async Task<Result<T>> RunAsync<T>(Func<Task<Result<T>>> logic)
        {

            try
            {
                return await logic();
            }
            catch (Exception ex)
            {
                return HandleException<T>(ex);
            }
        }

        // =========================================================================
        // 3. معالج العمليات التي لا ترجع بيانات (Action / Void)
        // =========================================================================
        public static async Task<Result> RunActionAsync(Func<Task<Result>> logic)
        {
            try
            {
                return await logic();
            }
            catch (Exception ex)
            {
                // 💎 الآن يستخدم المنطق المركزي الحقيقي بشكل متطابق 100%
                return HandleException<bool>(ex);
            }
        }
        public static async Task<Result<TResult>> GetResultAsync<TService, TResult>(
           this IServiceScopeFactory serviceScopeFactory,
           Func<TService, Task<Result<TResult>>> logic)
           where TService : notnull
        {
            // 1. فتح نطاق (Scope) جديد
            using (var scope = serviceScopeFactory.CreateScope())
            {
                // 2. استدعاء الخدمة المطلوبة من هذا النطاق بنسخة DbContext نظيفة
                var service = scope.ServiceProvider.GetRequiredService<TService>();

                // 3. تمرير الخدمة للدالة (Logic) وتنفيذها
                return await logic(service).ConfigureAwait(false);
            }
        }

        public static async Task<Result> GetResultAsync<TService>(
    this IServiceScopeFactory serviceScopeFactory,
    Func<TService, Task<Result>> logic)
    where TService : notnull
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return await logic(service).ConfigureAwait(false);
            }
        }
        public static Result RunScopedAction<TService>(
    this IServiceScopeFactory serviceScopeFactory,
    Func<TService, Result> logic)
    where TService : notnull
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                return  logic(service);
            }
        }

        // =========================================================================
        // 4. دالة معالجة الاستثناءات المركزية (DRY Principle)
        // =========================================================================
        private static Result<T> HandleException<T>(Exception ex)
        {
            // أ) التحقق أولاً: هل تم تسجيل محول استثناءات خارجي (مثل SqlErrorMapper) وهل تعرف على الخطأ؟
            if (ExceptionMapperHook != null)
            {
                var mappedError = ExceptionMapperHook(ex);
                if (mappedError.HasValue)
                {
                    return Result<T>.Failure(mappedError.Value.ErrorMessage, mappedError.Value.Type);
                }
            }

            // ب) إذا لم يكن استثناء قاعدة بيانات (أو لم يُعَرَّف)، يُرجع كخطأ غير متوقع
            return Result<T>.Failure("An unexpected error occurred: " + ex.Message, ErrorType.Unexpected);
        }
    }
}