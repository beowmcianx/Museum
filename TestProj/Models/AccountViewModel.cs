using System.Collections.Generic;

namespace TestProj.Models
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? CurrentRole { get; set; }
    }
}