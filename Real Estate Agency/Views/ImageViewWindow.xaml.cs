using System.Windows;
using System.Windows.Input;


namespace REA.Views
{

    public partial class ImageViewWindow : Window
    {
        public ImageViewWindow()
        {
            InitializeComponent();
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e) => SystemCommands.CloseWindow(this);
    }
}
