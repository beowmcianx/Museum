using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using TestProj.Models;


namespace TestProj.Models
{
    public enum OrderStatus
    {
        New,
        Confirmed,
        Cancelled,
        Used
    }

    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string OrderCode { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; } = OrderStatus.New;

        public int UserId { get; set; }
        public int MuseumId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; }

        [ForeignKey(nameof(MuseumId))]
        public MuseumModel Museum { get; set; }

        public ICollection<OrderItemModel> OrderItems { get; set; }
    }

}
