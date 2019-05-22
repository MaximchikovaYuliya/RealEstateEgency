using GalaSoft.MvvmLight;
using System.Text;

namespace REA.ViewModels
{
    class ErrorWindowVM : ViewModelBase
    {
        private StringBuilder errorString;

        public string ErrorString
        {
            get => errorString.ToString();
            set
            {
                errorString = new StringBuilder(value);
                RaisePropertyChanged();
            }
        }

        public ErrorWindowVM()
        {
            errorString = new StringBuilder();
            foreach (var item in CurrentState.ErrorList)
            {
                errorString.Insert(errorString.Length, item);
            }
        }
    }
}
