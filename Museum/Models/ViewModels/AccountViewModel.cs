using System.Collections.Generic;

namespace Museum.Models.NewFolder
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? CurrentRole { get; set; }
        public int? AssignedMuseumId { get; set; }
    }
}
