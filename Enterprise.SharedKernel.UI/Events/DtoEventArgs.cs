

using Enterprise.SharedKernel.DTOs.Interfaces;

namespace Enterprise.SharedKernel.UI
{
    public class DtoEventArgs : EventArgs
    {
        public IIdentifiableDto SelectedDto { get; }

        public DtoEventArgs(IIdentifiableDto selectedDto)
        {
            SelectedDto = selectedDto;
        }
    }
}