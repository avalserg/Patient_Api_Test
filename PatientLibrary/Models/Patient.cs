using PatientLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace PatientLibrary.Models
{
    public class Patient
    {
        public Guid PatientId { get; set; }
        public Name Name { get; set; }
        public Gender? Gender { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public bool? Active { get; set; }
    }
}
