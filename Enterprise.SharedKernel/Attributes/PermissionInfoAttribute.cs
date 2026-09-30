using System;


namespace Enterprise.SharedKernel.Attributes
{
  
    [AttributeUsage(AttributeTargets.Field)]
    public partial class PermissionInfoAttribute : Attribute
    {
        public string Category { get; }
        public string DisplayName { get; }

        public PermissionInfoAttribute(string category, string displayName)
        {
            Category = category;
            DisplayName = displayName;
        }
    }
}
