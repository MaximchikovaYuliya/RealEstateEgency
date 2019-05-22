using System.ComponentModel.DataAnnotations;

namespace REA.Models
{
    public class RealtyPhoto
    {
        #region Properties

        public int Id { get; set; }

        public byte[] File { get; set; }

        public int RealtyId { get; set; }

        [Required]
        public virtual Realty Realty { get; set; }

        #endregion
    }
}
