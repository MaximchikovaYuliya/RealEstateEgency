using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace REA.ViewModels.Navigation
{
    class NavigateViewModel : ViewModelBase
    {
        public void Navigate(string url)
        {
            Messenger.Default.Send(new NavigateArgs(url));
        }
    }
}
