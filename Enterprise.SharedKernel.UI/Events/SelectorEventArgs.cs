using Enterprise.SharedKernel.DTOs.Interfaces;

namespace SchoolERP.UI.Events
{
    public class SelectorEventArgs : EventArgs
    {
        public Type SelectorControlType { get; }

        public SelectorEventArgs(Type selectorControlType)
        {
            SelectorControlType = selectorControlType;
        }
    }
}
