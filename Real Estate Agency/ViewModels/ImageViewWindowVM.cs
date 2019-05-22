using GalaSoft.MvvmLight;
using REA.Models;

namespace REA.ViewModels
{
    class ImageViewWindowVM : ViewModelBase
    {
        public RealtyPhoto Photo { get => CurrentState.Photo; }
    }
}
