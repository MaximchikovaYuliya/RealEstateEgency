using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using REA.Models;
using REA.Models.Repository;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REA.ViewModels
{
    class AuthorizationWindowVM : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private readonly GenericRepository<Employee> repository;
        private Employee employee;
        private string repeatPassword;

        #endregion

        #region Properties

        public List<string> Languages { get; set; } = new List<string>() { "РУС", "ENG" };

        public string Language
        {
            get => CurrentState.CurrentLanguage;
            set => CurrentState.CurrentLanguage = value??CurrentState.CurrentLanguage;
        }

        public Employee Employee
        {
            get => employee;
            set
            {
                employee = value;
                RaisePropertyChanged();
            }
        }

        public string RepeatPassword
        {
            get => repeatPassword;
            set
            {
                repeatPassword = value;
                RaisePropertyChanged();
                RegisterEmployeeCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand RegisterEmployeeCommand { get; set; }
        public RelayCommand LoginEmployeeCommand { get; set; }
        public RelayCommand ClearDataCommand { get; }
        public RelayCommand ChangeLanguageCommand { get; set; }

        public ICommand OnDataChangedCommand
        {
            get
            {
                return new RelayCommand(() => 
                {
                    LoginEmployeeCommand.RaiseCanExecuteChanged();
                    RegisterEmployeeCommand.RaiseCanExecuteChanged();
                });
            }
        }

        #endregion

        public AuthorizationWindowVM()
        {
            repository = App.UnitOfWork.GenericRepository<Employee>();
            employee = new Employee();

            RegisterEmployeeCommand = new RelayCommand(RegisterAsync, CanRegister);
            LoginEmployeeCommand = new RelayCommand(LoginAsync, CanLogin);
            ClearDataCommand = new RelayCommand(ClearData);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage);
        }

        public bool CanRegister()
        {
            if (Employee[nameof(Employee.Surname)] == "" && Employee[nameof(Employee.Name)] == "" &&
                Employee[nameof(Employee.Patronymic)] == "" && Employee[nameof(Employee.Email)] == "" &&
                Employee[nameof(Employee.HashPassword)] == "" && this[nameof(RepeatPassword)] == "")
                return true;
            return false;
        }

        public async void RegisterAsync()
        {
            Task.Run(() => Employee.HashPassword = Hasher.GetHash(Employee.HashPassword));
            if (await Task.Run(() => repository.GetAsync(Employee.Email)) != null)
            {
                CurrentState.ErrorList.Add(App.Current.Resources["_ErrorUserAlreadyExist"].ToString());
                Rollback();
            }
            else
            {
                await Task.Run(() => repository.AddAsyn(Employee));
                CurrentState.UserEmail = Employee.Email;
                OpenMainWindow();
            }
        }

        public bool CanLogin()
        {
            if (Employee[nameof(Employee.Email)] == "" && Employee[nameof(Employee.HashPassword)] == "")
                return true;
            return false;
        }

        public async void LoginAsync()
        {
            Employee user = await Task.Run(() => repository.GetAsync(Employee.Email));
            if (user == null)
            {
                CurrentState.ErrorList.Add(App.Current.Resources["_ErrorUserNotExist"].ToString());
                Rollback();
            }
            else if (Hasher.GetHash(Employee.HashPassword) != user.HashPassword)
            {
                CurrentState.ErrorList.Add(App.Current.Resources["_ErrorWrongPassword"].ToString());
                Rollback();
            }
            else
            {
                CurrentState.UserEmail = user.Email;
                OpenMainWindow();
            }
        }

        public void ClearData()
        {
            Employee = new Employee();
            RepeatPassword = null;
        }

        public void Rollback()
        {
            Messenger.Default.Send(new NotificationMessage("OpenErrorWindow"));
            CurrentState.ErrorList.Clear();
            ClearData();
        }

        public void OpenMainWindow()
        {
            Messenger.Default.Send(new NotificationMessage("OpenMainWindow"));
        }

        public void ChangeLanguage()
        {
            if (Language == "РУС")
                App.SelectCulture("ru-RU");
            else
                App.SelectCulture("en-US");
        }

        #region IDataErrorInfo

        public string Error { get => null; }
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                propertyName = propertyName ?? string.Empty;
                if (propertyName == "RepeatPassword" && RepeatPassword != Employee.HashPassword)
                    result = App.Current.Resources["_ErrorMatchPasswords"].ToString();
                return result;
            }
        }

        #endregion
    }
}
