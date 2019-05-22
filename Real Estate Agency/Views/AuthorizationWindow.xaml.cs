using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Input;

namespace REA.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => DragMove();
            Messenger.Default.Register<NotificationMessage>(this, (msg)=>
            {
                if (msg.Notification == "OpenMainWindow")
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    SystemCommands.CloseWindow(this);
                }
                if (msg.Notification == "OpenErrorWindow")
                {
                    var errorWindow = new ErrorWindow();
                    errorWindow.ShowDialog();
                }
            });
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(this);
    }
}
