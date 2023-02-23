using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{ 
    public class Doctor
    {
        public Doctor()
        {
            var Visitations = new HashSet<Visitation>();
        }

        public int DoctorId { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(250)]
        public string? Specialty { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; }
    }
}
