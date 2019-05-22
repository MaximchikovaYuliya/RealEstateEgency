using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using REA.Models;
using REA.Models.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REA.ViewModels
{
    class RealtyPageVM : BasePageVM<Realty>
    {
        public class SearchFields
        {
            public string Address { get; set; }
            public string Currency { get; set; }
            public string Square { get; set; }
            public string Price { get; set; }
        }

        #region Fields

        private readonly GenericRepository<RealtyPhoto> realtyPhotosRepository;
        private readonly GenericRepository<Owner> ownersRepository;
        private ObservableCollection<RealtyPhoto> photos;
        private ObservableCollection<Owner> owners;
        private Owner newOwner;
        private SearchFields searchRealty;

        #endregion

        #region Commands

        public RelayCommand AddPhotoCommand { get; set; }
        public RelayCommand ShowImageCommand { get; set; }
        public RelayCommand<object> DeletePhotoCommand { get; set; }
        public RelayCommand AddOwnerCommand { get; set; }
        public RelayCommand CancelOwnerCommand { get; set; }
        public RelayCommand<object> MouseDoubleClickCommand { get; private set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public ICommand OnOwnerDataChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AddOwnerCommand.RaiseCanExecuteChanged();
                });
            }
        }

        #endregion

        #region Properties

        public List<TypeOfRealty> Types { get; set; }
        public List<string> Currency { get; set; } = new List<string>() { "USD", "EUR", "BYN" };

        public ObservableCollection<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                RaisePropertyChanged();
            }
        }

        public Owner NewOwner
        {
            get => newOwner;
            set
            {
                newOwner = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<RealtyPhoto> Photos
        {
            get => photos;
            set
            {
                photos = value;
                RaisePropertyChanged();
            }
        }

        public SearchFields SearchRealty
        {
            get => searchRealty;
            set
            {
                searchRealty = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public RealtyPageVM()
        {
            realtyPhotosRepository = App.UnitOfWork.GenericRepository<RealtyPhoto>();
            ownersRepository = App.UnitOfWork.GenericRepository<Owner>();

            NewOwner = new Owner();
            SearchRealty = new SearchFields();

            Photos = new ObservableCollection<RealtyPhoto>();
            Owners = new ObservableCollection<Owner>(ownersRepository.GetAll());
            Types = App.UnitOfWork.GenericRepository<TypeOfRealty>().GetAll().ToList();

            AddItemCommand = new RelayCommand(AddItem, CanAdd);

            AddPhotoCommand = new RelayCommand(AddPhoto);
            DeletePhotoCommand = new RelayCommand<object>(DeletePhoto);

            AddOwnerCommand = new RelayCommand(AddOwner, CanAddOwner);
            CancelOwnerCommand = new RelayCommand(() => NewOwner = new Owner());

            SearchCommand = new RelayCommand(Search);

            ClearCommand = new RelayCommand(() =>
            {
                SearchRealty = new SearchFields();
                Items = new ObservableCollection<Realty>(itemsRepository.GetAll());
            });

            MouseDoubleClickCommand = new RelayCommand<object>((obj) =>
            {
                CurrentState.Photo = obj as RealtyPhoto;
                Messenger.Default.Send(new NotificationMessage("OpenImageWindow"));
            });
        }

        public override bool CanSave()
        {
            if (SelectedItem == null || (SelectedItem[nameof(SelectedItem.Name)] == "" && SelectedItem[nameof(SelectedItem.Owner)] == "" &&
                SelectedItem[nameof(SelectedItem.Square)] == "" && SelectedItem[nameof(SelectedItem.Type)] == "" &&
                SelectedItem[nameof(SelectedItem.Address)] == ""))
                return true;
            return false;
        }

        public override bool CanAdd()
        {
            if (NewItem[nameof(NewItem.Name)] == "" && NewItem[nameof(NewItem.Owner)] == "" &&
                NewItem[nameof(NewItem.Square)] == "" && NewItem[nameof(NewItem.Type)] == "" &&
                NewItem[nameof(NewItem.Address)] == "")
                return true;
            return false;
        }

        public new async void AddItem()
        {
            itemsRepository.MakeUnchanged(Items);
            await itemsRepository.AddAsyn(NewItem);
            foreach (var item in Photos)
            {
                await realtyPhotosRepository.AddAsyn(item);
            }
            new Task(() => itemsRepository.MakeChanged(Items));
            Items.Add(NewItem);
            NewItem = new Realty();
            Photos = new ObservableCollection<RealtyPhoto>();
        }

        public bool CanAddOwner()
        {
            if (NewOwner[nameof(NewOwner.Surname)] == "" && NewOwner[nameof(NewOwner.Name)] == "" &&
                NewOwner[nameof(NewOwner.Patronymic)] == "" && NewOwner[nameof(NewOwner.Birthday)] == "" &&
                NewOwner[nameof(NewOwner.Passport)] == "" && NewOwner[nameof(NewOwner.Phone)] == "")
                return true;
            return false;
        }

        public async void AddOwner()
        {
            itemsRepository.MakeUnchanged(Items); 
            await ownersRepository.AddAsyn(NewOwner);
            new Task(() => itemsRepository.MakeChanged(Items));
            Owners.Add(NewOwner);
            NewOwner = new Owner();
        }

        public void AddPhoto()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                InitialDirectory = "",
                Filter = App.Current.Resources["_SaveImage"].ToString()
            };
            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                Photos.Add(new RealtyPhoto() { File = ImageConverter.ConvertImageToByteArray(selectedFileName), Realty = NewItem });
            }
        }

        public void DeletePhoto(object items)
        {
            System.Collections.IList list = (System.Collections.IList)items;
            var collection = new List<RealtyPhoto>(list.Cast<RealtyPhoto>());
            foreach (var item in collection)
            {
                Photos.Remove(item);
            }
        }

        public void Search()
        {
            decimal resultPrice;
            int resultSquare;
            Items = new ObservableCollection<Realty>(itemsRepository.GetAll());
            if (!string.IsNullOrEmpty(SearchRealty.Address))
                Items = new ObservableCollection<Realty>(Items.Where(x => x.Address == SearchRealty.Address));
            if (!string.IsNullOrEmpty(SearchRealty.Currency))
                Items = new ObservableCollection<Realty>(Items.Where(x => x.Currency == SearchRealty.Currency));
            if (decimal.TryParse(SearchRealty.Price, out resultPrice))
                Items = new ObservableCollection<Realty>(Items.Where(x => x.Price == resultPrice));
            if (int.TryParse(SearchRealty.Square, out resultSquare))
                Items = new ObservableCollection<Realty>(Items.Where(x => x.Square == resultSquare));
        }
    }
}
