using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TestProj.Models
{
    public class MuseumEmployeeModel
    {
        [Key]
        public int MuseumEmployeeId { get; set; }

        public int UserId { get; set; }
        public int MuseumId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; }

        [ForeignKey(nameof(MuseumId))]
        public MuseumModel Museum { get; set; }
    }
}
