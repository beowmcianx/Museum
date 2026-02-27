using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace TestProj.Models
{
    public class MuseumModel
    {
        [Key]
        public int MuseumId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan OpeningTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ClosingTime { get; set; }


        public string Type { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<TicketTypeModel> TicketTypes { get; set; }
        public ICollection<OrderModel> Orders { get; set; }
        public ICollection<MuseumEmployeeModel> Employees { get; set; }
        public ICollection<MuseumImageModel> Images { get; set; }
    }
}
