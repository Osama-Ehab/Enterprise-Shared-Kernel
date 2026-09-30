namespace Enterprise.SharedKernel.Interfaces
{
    public interface ICurrentUser
    {
        int UserId { get; }
        string FullName { get; }
        string Username { get; }
        long Permissions { get; } // الـ Bitwise رقم
        string LanguagePreference { get; }
        bool IsAuthenticated { get; }

        // دالة مساعدة لطبقة الـ Application لفحص الصلاحيات بسهولة
        bool HasPermission(long requiredPermission);
    }
}