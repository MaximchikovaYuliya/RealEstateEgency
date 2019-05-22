using GalaSoft.MvvmLight.Command;
using REA.ViewModels.Navigation;
using System.Collections.Generic;
using System.Windows.Input;

namespace REA.ViewModels
{
    class MainWindowVM : NavigateViewModel
    {
        #region Commands

        private ICommand _welcomePageCommand;
        private ICommand _clientsPageCommand;
        private ICommand _realtyPageCommand;
        private ICommand _editProfileCommand;
        private ICommand _dealsPageCommand;
        private ICommand _dealTypesPageCommand;
        private ICommand _realtyTypesPageCommand;
        public RelayCommand ChangeLanguageCommand { get; set; }

        #endregion

        #region Properties

        public List<string> Languages { get; set; } = new List<string>() { "РУС", "ENG" };

        public string Language
        {
            get => CurrentState.CurrentLanguage;
            set => CurrentState.CurrentLanguage = value;
        }

        public ICommand WelcomePageCommand
        {
            get
            {
                if (_welcomePageCommand == null)
                {
                    _welcomePageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/WelcomePage.xaml");
                    });
                }
                return _welcomePageCommand;
            }
            set => _welcomePageCommand = value;
        }

        public ICommand ClientsPageCommand
        {
            get
            {
                if (_clientsPageCommand == null)
                {
                    _clientsPageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/ClientsPage.xaml");
                    });
                }
                return _clientsPageCommand;
            }
            set => _clientsPageCommand = value;
        }

        public ICommand RealtyPageCommand
        {
            get
            {
                if (_realtyPageCommand == null)
                {
                    _realtyPageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/RealtyPage.xaml");
                    });
                }
                return _realtyPageCommand;
            }
            set => _realtyPageCommand = value;
        }

        public ICommand DealsPageCommand
        {
            get
            {
                if (_dealsPageCommand == null)
                {
                    _dealsPageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/DealsPage.xaml");
                    });
                }
                return _dealsPageCommand;
            }
            set => _dealsPageCommand = value;
        }

        public ICommand DealTypesPageCommand
        {
            get
            {
                if (_dealTypesPageCommand == null)
                {
                    _dealTypesPageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/DealTypesPage.xaml");
                    });
                }
                return _dealTypesPageCommand;
            }
            set => _dealTypesPageCommand = value;
        }

        public ICommand RealtyTypesPageCommand
        {
            get
            {
                if (_realtyTypesPageCommand == null)
                {
                    _realtyTypesPageCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/RealtyTypesPage.xaml");
                    });
                }
                return _realtyTypesPageCommand;
            }
            set => _realtyTypesPageCommand = value;
        }

        public ICommand EditProfileCommand
        {
            get
            {
                if (_editProfileCommand == null)
                {
                    _editProfileCommand = new RelayCommand(() =>
                    {
                        Navigate("Views/EditProfilePage.xaml");
                    });
                }
                return _editProfileCommand;
            }
            set => _editProfileCommand = value;
        }

        #endregion

        public MainWindowVM()
        {
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
        }

        public void ChangeLanguage()
        {
            if (Language == "РУС")
                App.SelectCulture("ru-RU");
            else
                App.SelectCulture("en-US");
        }
    }
}
