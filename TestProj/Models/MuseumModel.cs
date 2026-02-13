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

        [Required]
        public string City { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string WorkingHours { get; set; }

        public string Type { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<TicketTypeModel> TicketTypes { get; set; }
        public ICollection<OrderModel> Orders { get; set; }
        public ICollection<MuseumEmployeeModel> Employees { get; set; }
        public ICollection<MuseumImageModel> Images { get; set; }
    }
}
