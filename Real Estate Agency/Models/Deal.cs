using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REA.Models
{
    public class Deal : IDataErrorInfo
    {
        #region Fields

        [NotMapped]
        private string _additionalInfo;

        #endregion

        #region Properties

        public int Id { get; set; }

        public int TypeId { get; set; }

        [Required]
        public virtual TypeOfDeal Type { get; set; }

        [Required]
        public string EmployeeEmail { get; set; }

        [ForeignKey("EmployeeEmail")]
        public virtual Employee Employee { get; set; }

        public int ClientId { get; set; }

        [Required, ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public int RealtyId { get; set; }

        [Required, ForeignKey("RealtyId")]
        public virtual Realty Realty { get; set; }

        public DateTime Date { get; set; }

        public string AdditionalInfo
        {
            get => _additionalInfo;
            set
            {
                if (value == "")
                    _additionalInfo = null;
                else
                    _additionalInfo = value;
            }
        }

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
                if (propertyName == "Type" && Type == null)
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Realty" && Realty == null)
                    result = App.Current.Resources["_NotNull"].ToString();
                if (propertyName == "Client" && Client == null)
                    result = App.Current.Resources["_NotNull"].ToString();
                return result;
            }
        }

        #endregion

        public double GetCommission() => (double)Realty.Price * (Type.PerCent / 100.0);
    }
}
