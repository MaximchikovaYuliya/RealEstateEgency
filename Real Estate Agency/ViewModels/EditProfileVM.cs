using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using REA.Models;
using REA.Models.Repository;
using System.Windows.Input;

namespace REA.ViewModels
{
    class EditProfileVM : ViewModelBase
    {
        #region Fields

        private readonly GenericRepository<Employee> employeeRepository;
        private readonly GenericRepository<EmployeesPhoto> photoRepository;
        private Employee user;
        private EmployeesPhoto photo;

        #endregion

        #region Properties

        public Employee User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged();
            }
        }

        public EmployeesPhoto Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand DeletePhotoCommand { get; set; }
        public RelayCommand LoadPhotoCommand { get; set; }

        public ICommand OnDataChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveChangesCommand.RaiseCanExecuteChanged();
                });
            }
        }

        #endregion

        public EditProfileVM()
        {
            employeeRepository = App.UnitOfWork.GenericRepository<Employee>();
            photoRepository = App.UnitOfWork.GenericRepository<EmployeesPhoto>();
            User = employeeRepository.Get(CurrentState.UserEmail);
            Photo = photoRepository.Get(User.Email);

            SaveChangesCommand = new RelayCommand(SaveChangesAsync, CanSave);
            DeletePhotoCommand = new RelayCommand(() => Photo = null);
            LoadPhotoCommand = new RelayCommand(LoadPhoto);
        }

        public bool CanSave()
        {
            if (User[nameof(User.Surname)] == "" && User[nameof(User.Name)] == "" &&
                User[nameof(User.Patronymic)] == "" && User[nameof(User.Birthday)] == "" && 
                User[nameof(User.Passport)] == "" && User[nameof(User.Phone)] == "")
                return true;
            return false;
        }

        public async void SaveChangesAsync()
        {
            EmployeesPhoto oldPhoto = await photoRepository.GetAsync(User.Email);

            if (oldPhoto == null && Photo != null)
                await photoRepository.AddAsyn(Photo);
            else if (Photo == null && oldPhoto != null)
                await photoRepository.DeleteAsyn(oldPhoto);
            else if (Photo != null && oldPhoto != null)
                await photoRepository.UpdateAsyn(Photo, User.Email);

            await employeeRepository.UpdateAsyn(User, User.Email);
        }

        public void LoadPhoto()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = "",
                Filter = App.Current.Resources["_SaveImage"].ToString()
            };
            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                Photo = new EmployeesPhoto(User.Email, ImageConverter.ConvertImageToByteArray(selectedFileName));
            }
        }
    }
}
