using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using Museum.Models;

namespace Museum.Models
{
    public class TicketTypeModel
    {
        [Key]
        public int TicketTypeId { get; set; }

        [Required]
        public string Name { get; set; } // Adult, Student, Family

        [Required]
        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public int MuseumId { get; set; }

        [ForeignKey(nameof(MuseumId))]
        public MuseumModel Museum { get; set; }
    }
}
