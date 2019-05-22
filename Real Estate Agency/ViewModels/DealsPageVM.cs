using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using REA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace REA.ViewModels
{
    class DealsPageVM : BasePageVM<Deal>
    {
        #region Commands

        public RelayCommand<object> MakeContractCommand { get; set; }

        #endregion

        #region Properties

        public List<Client> Clients { get; }
        public List<Realty> Realty { get; }
        public List<TypeOfDeal> Types { get; }

        #endregion

        public DealsPageVM()
        {
            Types = App.UnitOfWork.GenericRepository<TypeOfDeal>().GetAll().ToList();
            Clients = App.UnitOfWork.GenericRepository<Client>().GetAll().ToList();
            Realty = App.UnitOfWork.GenericRepository<Realty>().GetAll().ToList();

            AddItemCommand = new RelayCommand(AddItem, CanAdd);
            MakeContractCommand = new RelayCommand<object>(MakeContract);
        }

        public override bool CanAdd()
        {
            if (NewItem[nameof(NewItem.Client)] == "" && NewItem[nameof(NewItem.Realty)] == "" &&
                NewItem[nameof(NewItem.Type)] == "")
                return true;
            return false;
        }

        public override bool CanSave() => true;

        public new async void AddItem()
        {
            NewItem.Date = DateTime.Today.Date;
            NewItem.Employee = App.UnitOfWork.GenericRepository<Employee>().Get(CurrentState.UserEmail);

            await itemsRepository.AddAsyn(NewItem);
            new Task(() => itemsRepository.MakeChanged(Items));
            Items.Add(NewItem);
            NewItem = new Deal();
        }

        public void MakeContract(object item)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                InitialDirectory = "",
                Filter = App.Current.Resources["_SaveWord"].ToString()
            };
            if (dlg.ShowDialog() == true)
            {
                DocumentCreator.CreateWordDoc(dlg.FileName, item as Deal);
            }
        }
    }
}
