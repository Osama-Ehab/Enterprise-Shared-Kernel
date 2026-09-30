using System;


namespace Enterprise.SharedKernel.Attributes
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedDisplayAttribute : Attribute
    {
        public string NameAr { get; }
        public string NameEn { get; }

        public LocalizedDisplayAttribute(string nameAr, string nameEn)
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }

        // 💎 دالة ذكية ترجع الاسم فوراً حسب ثقافة الويندوز الحالية!
        public string GetLocalizedName()
        {
            bool isArabic = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ar";
            return isArabic ? NameAr : NameEn;
        }
    }
}
