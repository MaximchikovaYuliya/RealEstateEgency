using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace REA.Views
{
    public partial class RealtyPage : Page
    {
        public RealtyPage()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, (msg) =>
            {
                if (msg.Notification == "OpenImageWindow")
                {
                    var imageWindow = new ImageViewWindow();
                    imageWindow.ShowDialog();
                }
            });
        }
    }
}
