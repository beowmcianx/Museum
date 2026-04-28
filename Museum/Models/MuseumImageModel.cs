using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Museum.Models
{
    public class MuseumImageModel
    {
        [Key]
        public int MuseumImageId { get; set; }

        public string ImageUrl { get; set; }

        public int MuseumId { get; set; }

        [ForeignKey(nameof(MuseumId))]
        public MuseumModel Museum { get; set; }
    }

}
