using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TestProj.Models
{
    public class OrderItemModel
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public int TicketTypeId { get; set; }

        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public OrderModel Order { get; set; }
        public TicketTypeModel TicketType { get; set; }
    }


}
