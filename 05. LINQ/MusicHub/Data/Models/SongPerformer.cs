using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class SongPerformer
    {
        public int SongId { get; set; }

        [Required]
        public Song Song { get; set; } = null!;

        public int PerformerId { get; set; }

        [Required]
        public Performer Performer { get; set; } = null!;
    }
}
