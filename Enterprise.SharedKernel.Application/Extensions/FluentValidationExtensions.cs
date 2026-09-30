using FluentValidation;
using System.Threading;
namespace Enterprise.SharedKernel.Extensions
{

    public static class FluentValidationExtensions
    {
        // 💎 دالة سحرية تستقبل الرسالتين مباشرة وتدير اللغة داخلياً وقت حدوث الخطأ
        public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessage<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule,
            string messageAr,
            string messageEn)
        {
            return rule.WithMessage(_ =>
            {
                // فحص ثقافة Thread الويندوز في اللحظة التي يقع فيها الخطأ
                bool isArabic = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar";
                return isArabic ? messageAr : messageEn;
            });
        }
    }
}
