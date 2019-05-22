using REA.Models;
using System;
using System.Collections.Generic;

namespace REA.ViewModels
{
    class DealTypesVM : BasePageVM<TypeOfDeal>
    {
        #region Properties

        public IList<double> PercentList { get; }

        #endregion

        public DealTypesVM()
        {
            PercentList = new List<double>();

            for (double i = 1.0; i < 3.1; i = i + 0.1) 
            {
                PercentList.Add(Math.Round(i, 1));
            }
        }

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
