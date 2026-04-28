using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Museum.Models
{
    public enum UserRole
    {
        Client,
        MuseumEmployee,
        Admin
    }

    public class UserModel : IdentityUser<int>
    {
        public string FullName { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
        public MuseumEmployeeModel? MuseumEmployee { get; set; } 
    }
}
