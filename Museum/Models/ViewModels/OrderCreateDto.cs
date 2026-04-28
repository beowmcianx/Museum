using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Museum.Models.ViewModels
{
    public class OrderItemInput
    {
        public int TicketTypeId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        public int MuseumId { get; set; }

        public bool AcceptRules { get; set; }

        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; } = DateTime.Today;

        public List<OrderItemInput> Items { get; set; } = new List<OrderItemInput>();
    }
}
