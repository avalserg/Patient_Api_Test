using System.ComponentModel.DataAnnotations;

namespace PatientLibrary.Models
{
    public class Name
    {
        public Guid Id { get; set; }
        public string? Use { get; set; }
        [Required]
        public string Family { get; set; } = string.Empty;
        public List<string>? Given { get; set; }

    }
}
