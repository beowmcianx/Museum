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

            builder.Entity<MuseumImageModel>()
            .HasOne(mi => mi.Museum)
            .WithMany(m => m.Images)
            .HasForeignKey(mi => mi.MuseumId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MuseumEmployeeModel>()
            .HasOne(me => me.User)
            .WithOne(u => u.MuseumEmployee)
            .HasForeignKey<MuseumEmployeeModel>(me => me.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MuseumEmployeeModel>()
            .HasOne(me => me.Museum)
            .WithMany(m => m.Employees)
            .HasForeignKey(me => me.MuseumId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MuseumModel>().HasData(
               new MuseumModel { MuseumId = 1, Name = "Louvre Museum", Country = "France", City = "Paris", Address = "Rue de Rivoli", Description = "World's largest art museum.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 2, Name = "British Museum", Country = "United Kingdom", City = "London", Address = "Great Russell St", Description = "Museum dedicated to human history and culture.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 30, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 3, Name = "Metropolitan Museum of Art", Country = "USA", City = "New York", Address = "1000 5th Ave", Description = "Largest art museum in the US.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 4, Name = "Vatican Museums", Country = "Vatican City", City = "Vatican City", Address = "Viale Vaticano", Description = "Christian and art museums.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(16, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 5, Name = "Prado Museum", Country = "Spain", City = "Madrid", Address = "Calle de Ruiz de Alarcón", Description = "Spanish national art museum.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(20, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 6, Name = "State Hermitage Museum", Country = "Russia", City = "Saint Petersburg", Address = "Palace Square", Description = "One of the largest museums in the world.", OpeningTime = new TimeSpan(10, 30, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 7, Name = "National Gallery", Country = "United Kingdom", City = "London", Address = "Trafalgar Square", Description = "Collection of European paintings.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 8, Name = "Rijksmuseum", Country = "Netherlands", City = "Amsterdam", Address = "Museumstraat 1", Description = "Dutch national museum.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 9, Name = "Uffizi Gallery", Country = "Italy", City = "Florence", Address = "Piazzale degli Uffizi", Description = "Famous Italian art museum.", OpeningTime = new TimeSpan(8, 15, 0), ClosingTime = new TimeSpan(18, 30, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 10, Name = "Acropolis Museum", Country = "Greece", City = "Athens", Address = "Dionysiou Areopagitou 15", Description = "Archaeological museum focused on the Acropolis.", OpeningTime = new TimeSpan(8, 0, 0), ClosingTime = new TimeSpan(20, 0, 0), Type = "Archaeology", IsActive = true },

               new MuseumModel { MuseumId = 11, Name = "Egyptian Museum", Country = "Egypt", City = "Cairo", Address = "Tahrir Square", Description = "Ancient Egyptian antiquities.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 12, Name = "Smithsonian National Air and Space Museum", Country = "USA", City = "Washington", Address = "600 Independence Ave SW", Description = "Aviation and space artifacts.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 30, 0), Type = "Science", IsActive = true },
               new MuseumModel { MuseumId = 13, Name = "National Museum of China", Country = "China", City = "Beijing", Address = "East Chang'an Avenue", Description = "Chinese art and history.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 14, Name = "Tokyo National Museum", Country = "Japan", City = "Tokyo", Address = "13-9 Uenokoen", Description = "Japanese art and antiquities.", OpeningTime = new TimeSpan(9, 30, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 15, Name = "National Museum of Korea", Country = "South Korea", City = "Seoul", Address = "137 Seobinggo-ro", Description = "Korean history and art.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "History", IsActive = true },

               new MuseumModel { MuseumId = 16, Name = "Art Institute of Chicago", Country = "USA", City = "Chicago", Address = "111 S Michigan Ave", Description = "Famous art museum in Chicago.", OpeningTime = new TimeSpan(11, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 17, Name = "Museum of Modern Art", Country = "USA", City = "New York", Address = "11 W 53rd St", Description = "Modern and contemporary art.", OpeningTime = new TimeSpan(10, 30, 0), ClosingTime = new TimeSpan(17, 30, 0), Type = "Modern Art", IsActive = true },
               new MuseumModel { MuseumId = 18, Name = "Van Gogh Museum", Country = "Netherlands", City = "Amsterdam", Address = "Museumplein 6", Description = "Works of Vincent van Gogh.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Art", IsActive = true },
               new MuseumModel { MuseumId = 19, Name = "Guggenheim Museum", Country = "Spain", City = "Bilbao", Address = "Abandoibarra Etorbidea 2", Description = "Contemporary art museum.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(19, 0, 0), Type = "Modern Art", IsActive = true },
               new MuseumModel { MuseumId = 20, Name = "National WWII Museum", Country = "USA", City = "New Orleans", Address = "945 Magazine St", Description = "World War II history.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "War", IsActive = true },

               new MuseumModel { MuseumId = 21, Name = "Canadian Museum of History", Country = "Canada", City = "Gatineau", Address = "100 Laurier St", Description = "Canadian history and culture.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 22, Name = "Museo Nacional de Antropología", Country = "Mexico", City = "Mexico City", Address = "Av. Paseo de la Reforma", Description = "Mexican archaeology and anthropology.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(19, 0, 0), Type = "Anthropology", IsActive = true },
               new MuseumModel { MuseumId = 23, Name = "Pergamon Museum", Country = "Germany", City = "Berlin", Address = "Bodestraße 1-3", Description = "Classical antiquities museum.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Archaeology", IsActive = true },
               new MuseumModel { MuseumId = 24, Name = "National Palace Museum", Country = "Taiwan", City = "Taipei", Address = "221 Zhishan Rd", Description = "Chinese imperial artifacts.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 25, Name = "Museum of Tomorrow", Country = "Brazil", City = "Rio de Janeiro", Address = "Praça Mauá 1", Description = "Science museum focused on sustainability.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "Science", IsActive = true },

               new MuseumModel { MuseumId = 26, Name = "Te Papa Tongarewa", Country = "New Zealand", City = "Wellington", Address = "55 Cable St", Description = "Museum of New Zealand.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(18, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 27, Name = "National Museum of Anthropology", Country = "Spain", City = "Madrid", Address = "Calle Alfonso XII 68", Description = "Anthropology museum.", OpeningTime = new TimeSpan(9, 30, 0), ClosingTime = new TimeSpan(15, 0, 0), Type = "Anthropology", IsActive = true },
               new MuseumModel { MuseumId = 28, Name = "Powerhouse Museum", Country = "Australia", City = "Sydney", Address = "500 Harris St", Description = "Science and design museum.", OpeningTime = new TimeSpan(10, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "Science", IsActive = true },
               new MuseumModel { MuseumId = 29, Name = "Apartheid Museum", Country = "South Africa", City = "Johannesburg", Address = "Northern Parkway Rd", Description = "History of apartheid in South Africa.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(17, 0, 0), Type = "History", IsActive = true },
               new MuseumModel { MuseumId = 30, Name = "Museum of Islamic Art", Country = "Qatar", City = "Doha", Address = "Corniche", Description = "Islamic art collection.", OpeningTime = new TimeSpan(9, 0, 0), ClosingTime = new TimeSpan(19, 0, 0), Type = "Art", IsActive = true }
            );

            builder.Entity<TicketTypeModel>().HasData(
                new TicketTypeModel { TicketTypeId = 1, MuseumId = 1, Name = "Adult", Price = 25, IsActive = true },
                new TicketTypeModel { TicketTypeId = 2, MuseumId = 1, Name = "Student", Price = 15, IsActive = true },
                new TicketTypeModel { TicketTypeId = 3, MuseumId = 1, Name = "Child", Price = 10, IsActive = true },

                new TicketTypeModel { TicketTypeId = 4, MuseumId = 2, Name = "Adult", Price = 20, IsActive = true },
                new TicketTypeModel { TicketTypeId = 5, MuseumId = 2, Name = "Student", Price = 12, IsActive = true },
                new TicketTypeModel { TicketTypeId = 6, MuseumId = 2, Name = "Child", Price = 8, IsActive = true },

                new TicketTypeModel { TicketTypeId = 7, MuseumId = 3, Name = "Adult", Price = 28, IsActive = true },
                new TicketTypeModel { TicketTypeId = 8, MuseumId = 3, Name = "Student", Price = 18, IsActive = true },
                new TicketTypeModel { TicketTypeId = 9, MuseumId = 3, Name = "Child", Price = 12, IsActive = true },

                new TicketTypeModel { TicketTypeId = 10, MuseumId = 4, Name = "Adult", Price = 30, IsActive = true },
                new TicketTypeModel { TicketTypeId = 11, MuseumId = 4, Name = "Student", Price = 20, IsActive = true },
                new TicketTypeModel { TicketTypeId = 12, MuseumId = 4, Name = "Child", Price = 15, IsActive = true },

                new TicketTypeModel { TicketTypeId = 13, MuseumId = 5, Name = "Adult", Price = 22, IsActive = true },
                new TicketTypeModel { TicketTypeId = 14, MuseumId = 5, Name = "Student", Price = 14, IsActive = true },
                new TicketTypeModel { TicketTypeId = 15, MuseumId = 5, Name = "Child", Price = 9, IsActive = true }
            );

            builder.Entity<MuseumImageModel>().HasData(

                new MuseumImageModel
                {
                    MuseumImageId = 1,
                    MuseumId = 1,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/aa/Louvre_Museum_Wikimedia_Commons.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 2,
                    MuseumId = 2,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a3/British_Museum_from_NE_2.JPG"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 3,
                    MuseumId = 3,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/0c/Metropolitan_Museum_of_Art_entrance_NYC.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 4,
                    MuseumId = 4,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/6/6f/Vatican_Museums_Entrance.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 5,
                    MuseumId = 5,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Museo_del_Prado_2016_%28cropped%29.jpg"
                }
            );

            var ticketId = 16; 
            var ticketTypes = new List<TicketTypeModel>();

            for (int museumId = 6; museumId <= 30; museumId++)
            {
                ticketTypes.Add(new TicketTypeModel
                {
                    TicketTypeId = ticketId++,
                    MuseumId = museumId,
                    Name = "Adult",
                    Price = 20,
                    IsActive = true
                });

                ticketTypes.Add(new TicketTypeModel
                {
                    TicketTypeId = ticketId++,
                    MuseumId = museumId,
                    Name = "Student",
                    Price = 12,
                    IsActive = true
                });

                ticketTypes.Add(new TicketTypeModel
                {
                    TicketTypeId = ticketId++,
                    MuseumId = museumId,
                    Name = "Child",
                    Price = 8,
                    IsActive = true
                });
            }

            builder.Entity<TicketTypeModel>().HasData(ticketTypes);
        }

    }
}
