using REA.Models;

namespace REA.ViewModels
{
    class ClientsPageVM : BasePageVM<Client>
    {
        public override bool CanAdd()
        {
            if (NewItem[nameof(NewItem.Surname)] == "" && NewItem[nameof(NewItem.Name)] == "" &&
                NewItem[nameof(NewItem.Patronymic)] == ""  && NewItem[nameof(NewItem.Birthday)] == "" && 
                NewItem[nameof(NewItem.Passport)] == "" && NewItem[nameof(NewItem.Phone)] == "")
                return true;
            return false;
        }

        public override bool CanSave()
        {
            if (SelectedItem == null || (SelectedItem[nameof(SelectedItem.Surname)] == "" && SelectedItem[nameof(SelectedItem.Name)] == "" &&
                SelectedItem[nameof(SelectedItem.Patronymic)] == "" && SelectedItem[nameof(SelectedItem.Birthday)] == "" &&
                SelectedItem[nameof(SelectedItem.Passport)] == "" && SelectedItem[nameof(SelectedItem.Phone)] == ""))
                return true;
            return false;
        }
    }
}
