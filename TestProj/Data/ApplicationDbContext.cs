using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TestProj.Models;

namespace TestProj.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
    {
        public DbSet<MuseumModel> Museums { get; set; } = null!;
        public DbSet<TicketTypeModel> TicketTypes { get; set; } = null!;
        public DbSet<OrderModel> Orders { get; set; } = null!;
        public DbSet<OrderItemModel> OrderItems { get; set; } = null!;
        public DbSet<MuseumImageModel> MuseumImages { get; set; } = null!;
        public DbSet<MuseumEmployeeModel> MuseumEmployees { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Orders → Museum
            builder.Entity<OrderModel>()
                .HasOne(o => o.Museum)
                .WithMany(m => m.Orders)
                .HasForeignKey(o => o.MuseumId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade from Museum → Orders

            // OrderItems → Orders
            builder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Only cascade from Order → OrderItem

            // OrderItems → TicketTypes
            builder.Entity<OrderItemModel>()
                .HasOne(oi => oi.TicketType)
                .WithMany()
                .HasForeignKey(oi => oi.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict); // NO cascade here

            // TicketTypes → Museum
            builder.Entity<TicketTypeModel>()
                .HasOne(tt => tt.Museum)
                .WithMany(m => m.TicketTypes)
                .HasForeignKey(tt => tt.MuseumId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade from Museum → TicketTypes

            // Fix decimal precision warnings
            builder.Entity<OrderItemModel>()
                .Property(oi => oi.PriceAtPurchase)
                .HasPrecision(18, 2);

            builder.Entity<TicketTypeModel>()
                .Property(tt => tt.Price)
                .HasPrecision(18, 2);

            builder.Entity<MuseumModel>().HasData(
               new MuseumModel { MuseumId = 1, Name = "Louvre Museum", City = "Paris", Address = "Rue de Rivoli", Description = "World's largest art museum.", WorkingHours = "09:00-18:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 2, Name = "British Museum", City = "London", Address = "Great Russell St", Description = "Museum dedicated to human history and culture.", WorkingHours = "10:00-17:30", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 3, Name = "Metropolitan Museum of Art", City = "New York", Address = "1000 5th Ave", Description = "Largest art museum in the US.", WorkingHours = "10:00-17:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 4, Name = "Vatican Museums", City = "Vatican City", Address = "Viale Vaticano", Description = "Christian and art museums.", WorkingHours = "09:00-16:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 5, Name = "Prado Museum", City = "Madrid", Address = "Calle de Ruiz de Alarcón", Description = "Spanish national art museum.", WorkingHours = "10:00-20:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 6, Name = "State Hermitage Museum", City = "Saint Petersburg", Address = "Palace Square", Description = "One of the largest museums in the world.", WorkingHours = "10:30-18:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 7, Name = "National Gallery", City = "London", Address = "Trafalgar Square", Description = "Collection of European paintings.", WorkingHours = "10:00-18:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 8, Name = "Rijksmuseum", City = "Amsterdam", Address = "Museumstraat 1", Description = "Dutch national museum.", WorkingHours = "09:00-17:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 9, Name = "Uffizi Gallery", City = "Florence", Address = "Piazzale degli Uffizi", Description = "Famous Italian art museum.", WorkingHours = "08:15-18:30", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 10, Name = "Acropolis Museum", City = "Athens", Address = "Dionysiou Areopagitou 15", Description = "Archaeological museum focused on the Acropolis.", WorkingHours = "08:00-20:00", Type = "Archaeology", IsActive = true },

               new MuseumModel { MuseumId = 11, Name = "Egyptian Museum", City = "Cairo", Address = "Tahrir Square", Description = "Ancient Egyptian antiquities.", WorkingHours = "09:00-17:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 12, Name = "Smithsonian National Air and Space Museum", City = "Washington", Address = "600 Independence Ave SW", Description = "Aviation and space artifacts.", WorkingHours = "10:00-17:30", Type = "Science", IsActive = true },
               new MuseumModel { MuseumId = 13, Name = "National Museum of China", City = "Beijing", Address = "East Chang'an Avenue", Description = "Chinese art and history.", WorkingHours = "09:00-17:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 14, Name = "Tokyo National Museum", City = "Tokyo", Address = "13-9 Uenokoen", Description = "Japanese art and antiquities.", WorkingHours = "09:30-17:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 15, Name = "National Museum of Korea", City = "Seoul", Address = "137 Seobinggo-ro", Description = "Korean history and art.", WorkingHours = "10:00-18:00", Type = "History", IsActive = true },

               new MuseumModel { MuseumId = 16, Name = "Art Institute of Chicago", City = "Chicago", Address = "111 S Michigan Ave", Description = "Famous art museum in Chicago.", WorkingHours = "11:00-17:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 17, Name = "Museum of Modern Art", City = "New York", Address = "11 W 53rd St", Description = "Modern and contemporary art.", WorkingHours = "10:30-17:30", Type = "Modern Art", IsActive = true },
               new MuseumModel { MuseumId = 18, Name = "Van Gogh Museum", City = "Amsterdam", Address = "Museumplein 6", Description = "Works of Vincent van Gogh.", WorkingHours = "09:00-18:00", Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 19, Name = "Guggenheim Museum", City = "Bilbao", Address = "Abandoibarra Etorbidea 2", Description = "Contemporary art museum.", WorkingHours = "10:00-19:00", Type = "Modern Art", IsActive = true },
               new MuseumModel { MuseumId = 20, Name = "National WWII Museum", City = "New Orleans", Address = "945 Magazine St", Description = "World War II history.", WorkingHours = "09:00-17:00", Type = "War", IsActive = true },

               new MuseumModel { MuseumId = 21, Name = "Canadian Museum of History", City = "Gatineau", Address = "100 Laurier St", Description = "Canadian history and culture.", WorkingHours = "09:00-17:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 22, Name = "Museo Nacional de Antropología", City = "Mexico City", Address = "Av. Paseo de la Reforma", Description = "Mexican archaeology and anthropology.", WorkingHours = "09:00-19:00", Type = "Anthropology", IsActive = true },
               new MuseumModel { MuseumId = 23, Name = "Pergamon Museum", City = "Berlin", Address = "Bodestraße 1-3", Description = "Classical antiquities museum.", WorkingHours = "10:00-18:00", Type = "Archaeology", IsActive = true },
               new MuseumModel { MuseumId = 24, Name = "National Palace Museum", City = "Taipei", Address = "221 Zhishan Rd", Description = "Chinese imperial artifacts.", WorkingHours = "09:00-17:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 25, Name = "Museum of Tomorrow", City = "Rio de Janeiro", Address = "Praça Mauá 1", Description = "Science museum focused on sustainability.", WorkingHours = "10:00-18:00", Type = "Science", IsActive = true },

               new MuseumModel { MuseumId = 26, Name = "Te Papa Tongarewa", City = "Wellington", Address = "55 Cable St", Description = "Museum of New Zealand.", WorkingHours = "10:00-18:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 27, Name = "National Museum of Anthropology", City = "Madrid", Address = "Calle Alfonso XII 68", Description = "Anthropology museum.", WorkingHours = "09:30-15:00", Type = "Anthropology", IsActive = true },
               new MuseumModel { MuseumId = 28, Name = "Powerhouse Museum", City = "Sydney", Address = "500 Harris St", Description = "Science and design museum.", WorkingHours = "10:00-17:00", Type = "Science", IsActive = true },
               new MuseumModel { MuseumId = 29, Name = "Apartheid Museum", City = "Johannesburg", Address = "Northern Parkway Rd", Description = "History of apartheid in South Africa.", WorkingHours = "09:00-17:00", Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 30, Name = "Museum of Islamic Art", City = "Doha", Address = "Corniche", Description = "Islamic art collection.", WorkingHours = "09:00-19:00", Type = "Art", IsActive = true }
           );
        }

    }
}
