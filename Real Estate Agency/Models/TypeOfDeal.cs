﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REA.Models
{
    public class TypeOfDeal : IDataErrorInfo
    {
        #region Fields

        [NotMapped]
        private string _name;

        [NotMapped]
        private string _description;

        #endregion

        #region Properties

        public int Id { get; set; }

        [Required]
        public string Name
        {
            get => _name;
            set
            {
                if (value == "")
                    _name = null;
                else
                    _name = value;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (value == "")
                    _description = null;
                else
                    _description = value;
            }
        }

        public double PerCent { get; set; } = 1;

        public virtual ICollection<Deal> Deals { get; set; }

        #endregion

        #region IDataErrorInfo

        [NotMapped]
        public string Error { get => string.Empty; }

        [NotMapped]
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                propertyName = propertyName ?? string.Empty;
                if (propertyName == "Name" && (Name == null || Name == ""))
                    result = App.Current.Resources["_NotNull"].ToString();
                return result;
            }
        }

        #endregion

        public TypeOfDeal()
        {
            Deals = new List<Deal>();
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }
    }
}
