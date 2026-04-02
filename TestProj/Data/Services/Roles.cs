using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestProj.Data.Services
{
    public static class Roles
    {
        public const string Client = "Client";
        public const string Worker = "Worker";
        public const string Admin = "Admin";
    }
}
