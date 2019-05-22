using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using REA.Models.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REA.ViewModels
{
    abstract class BasePageVM<Model> : ViewModelBase where Model : class, new()
    {
        #region Fields

        protected readonly GenericRepository<Model> itemsRepository;
        private ObservableCollection<Model> items;
        private Model newItem;

        #endregion

        #region Commands

        public RelayCommand AddItemCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand<object> DeleteCommand { get; set; }
        public RelayCommand CreateExcelCommand { get; set; }

        public ICommand OnDataChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AddItemCommand.RaiseCanExecuteChanged();
                });
            }
        }

        public ICommand OnDataTableChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    UpdateCommand.RaiseCanExecuteChanged();
                });
            }
        }

        #endregion

        #region Properties

        public Model SelectedItem { get; set; }

        public ObservableCollection<Model> Items
        {
            get => items;
            set
            {
                items = value;
                RaisePropertyChanged();
            }
        }

        public Model NewItem
        {
            get => newItem;
            set
            {
                newItem = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public BasePageVM()
        {
            App.UnitOfWork = new Models.UnitOfWork.UnitOfWork();
            itemsRepository = App.UnitOfWork.GenericRepository<Model>();

            NewItem = new Model();
            Items = new ObservableCollection<Model>(itemsRepository.GetAll());

            AddItemCommand = new RelayCommand(AddItem, CanAdd);
            CancelCommand = new RelayCommand(() => NewItem = new Model());
            DeleteCommand = new RelayCommand<object>(Delete);
            UpdateCommand = new RelayCommand(() => App.UnitOfWork.Save(), CanSave);
            CreateExcelCommand = new RelayCommand(CreateExcel);
        }

        public abstract bool CanAdd();
        public abstract bool CanSave();

        public async void AddItem()
        {
            itemsRepository.MakeUnchanged(Items);
            await itemsRepository.AddAsyn(NewItem);
            new Task(() => itemsRepository.MakeChanged(Items));
            Items.Add(NewItem);
            NewItem = new Model();
        }

        public async void Delete(object items)
        {
            System.Collections.IList list = (System.Collections.IList)items;
            var collection = new List<Model>(list.Cast<Model>());
            foreach (var item in collection)
            {
                await itemsRepository.DeleteAsyn(item);
                Items.Remove(item);
            }
        }

        public void CreateExcel()
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                InitialDirectory = "",
                Filter = App.Current.Resources["_SaveExcel"].ToString()
            };
            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;
                DocumentCreator.CreateExcelDoc(fileName, itemsRepository.GetAll().ToList());
            }
        }
    }
}
