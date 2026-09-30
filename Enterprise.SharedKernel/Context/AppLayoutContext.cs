using FluentValidation;
using System;
using System.Globalization;
using System.Threading;

namespace Enterprise.SharedKernel.Context
{
    public static class AppLayoutContext
    {
        // ==========================================
        // 1. الخصائص الأساسية (Properties)
        // ==========================================
        public static bool IsArabicLayout { get; private set; } = true; // الافتراضي
        public static bool IsDarkMode { get; private set; } = false;

        // حدث (Event) ينطلق عندما تتغير اللغة، لتحديث الشاشات المفتوحة
        public static event EventHandler LayoutDirectionChanged;
        public static event EventHandler ThemeChanged;

    // ... (الخصائص السابقة IsArabicLayout و IsDarkMode) ...

    // الدالة الجديدة للتهيئة وقت الإقلاع
    public static void Initialize(string cultureCode, bool isDarkMode)
    {
        SetLanguage(cultureCode);

        // تعيين الثيم مباشرة دون استدعاء الحدث (لأن الشاشات لم تُبنى بعد)
        IsDarkMode = isDarkMode;
    }

    public static void SetLanguage(string cultureCode)
    {
            bool isArabic = cultureCode.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
            IsArabicLayout = isArabic;

            // 1. تحديد ثقافة واجهة المستخدم (للكلام والرسائل)
            var uiCulture = new CultureInfo(isArabic ? "ar-EG" : "en-US");

            // 2. 💎 بناء "ثقافة بيانات هجينة" مخصصة لنظام الـ ERP
            // نقوم باستنساخ الثقافة العربية المحلية للحصول على العملة والإعدادات الإقليمية
            var hybridDataCulture = (CultureInfo)new CultureInfo(isArabic ? "ar-EG" : "en-US").Clone();

            // أ) استبدال تنسيق التواريخ والتقويم بالكامل ليطابق التنسيق الإنجليزي البريطاني (dd/MM/yyyy)
            hybridDataCulture.DateTimeFormat = new CultureInfo("en-GB").DateTimeFormat;

            // ب) إجبار النظام على استخدام الأرقام الغربية (0123456789) بدلاً من الأرقام العربية/الهندية (٠١٢٣٤٥٦٧٨٩)
            hybridDataCulture.NumberFormat.DigitSubstitution = DigitShapes.None;

            // 3. تطبيق الثقافة الهجينة على الـ Thread
            Thread.CurrentThread.CurrentUICulture = uiCulture;         // كلام عربي
            Thread.CurrentThread.CurrentCulture = hybridDataCulture;   // تواريخ إنجليزية + عملة مصرية/عربية

            // 4. تفعيل لغة محرك التحقق
            ValidatorOptions.Global.LanguageManager.Culture = uiCulture;
        
        LayoutDirectionChanged?.Invoke(null, EventArgs.Empty);
    }

    public static void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}