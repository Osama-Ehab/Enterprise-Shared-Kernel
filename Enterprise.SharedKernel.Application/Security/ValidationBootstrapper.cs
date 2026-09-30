
using Enterprise.SharedKernel.Attributes;
using FluentValidation;
using System.Reflection;

namespace Enterprise.SharedKernel.Application
{
    public static class ValidationBootstrapper
    {
        public static void ConfigureValidationEngine()
        {
            // 1. ضبط محول الأسماء العالمي ليعتمد على LocalizedDisplayAttribute الموجود في Core
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                if (memberInfo == null) return null;

                var localizedAttr = memberInfo.GetCustomAttribute<LocalizedDisplayAttribute>();
                return localizedAttr?.GetLocalizedName();
            };

            // 2. أي إعدادات عامة أخرى للـ FluentValidation
        }
    }
}