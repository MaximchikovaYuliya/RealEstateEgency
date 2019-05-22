using REA.Models;

namespace REA.ViewModels
{
    class RealtyTypesVM : BasePageVM<TypeOfRealty>
    {
        public override bool CanAdd()
        {
            if (NewItem[nameof(NewItem.Name)] == "")
                return true;
            return false;
        }

        public override bool CanSave()
        {
            if (SelectedItem != null && SelectedItem[nameof(SelectedItem.Name)] != "")
                return false;
            return true;
        }
    }
}
