using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REA.Models
{
    public class Realty : IDataErrorInfo
    {
        #region Fields

        [NotMapped]
        private string _address;


        #endregion

        #region Properties

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address
        {
            get => _address;
            set
            {
                if (value == "")
                    _address = null;
                else
                    _address = value;
            }
        }

        public int Square { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Currency { get; set; } = "USD";

        public virtual ICollection<RealtyPhoto> Photos { get; set; }

        public int TypeId { get; set; }

        [Required]
        public virtual TypeOfRealty Type { get; set; }

        public int OwnerId { get; set; }

        [Required]
        public virtual Owner Owner { get; set; }

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
                if (propertyName == "Address" && (Address == null || Address == ""))
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Name" && (Name == null || Name == ""))
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Type" && Type == null)
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Owner" && Owner == null)
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Square" && Square <= 0)
                    result = App.Current.Resources["_IncorrectValue"].ToString();
                if (propertyName == "Price" && Price <= 0)
                    result = App.Current.Resources["_IncorrectValue"].ToString();
                return result;
            }
        }

        #endregion

        public Realty()
        {
            Photos = new List<RealtyPhoto>();
            Deals = new List<Deal>();
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }
    }
}