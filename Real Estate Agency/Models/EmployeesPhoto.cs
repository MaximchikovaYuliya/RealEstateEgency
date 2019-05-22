using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REA.Models
{
    public class EmployeesPhoto
    {
        #region Properties

        [Key, ForeignKey("Employee")]
        public string Email { get; set; }

        public byte[] File { get; set; }

        [Required]
        public virtual Employee Employee { get; set; }

        #endregion

        public EmployeesPhoto() { }

        public EmployeesPhoto(string email, byte[] file)
        {
            Email = email;
            File = file;
        }
    }
}
