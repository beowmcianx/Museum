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
                    ImageUrl = "https://t4.ftcdn.net/jpg/03/02/03/03/240_F_302030306_VLGXUtPa0QId7O6Zqz9AF6RSql6uIdVd.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 2,
                    MuseumId = 2,
                    ImageUrl = "https://t3.ftcdn.net/jpg/00/95/44/26/240_F_95442619_GrhmvQcSput2G9lrbG8QRzX96H3MAvlG.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 3,
                    MuseumId = 3,
                    ImageUrl = "https://t3.ftcdn.net/jpg/06/66/26/00/240_F_666260007_eCaT4Pk5BP9zWzvY5wPQRjRdLLlkLkVe.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 4,
                    MuseumId = 4,
                    ImageUrl = "https://t3.ftcdn.net/jpg/05/07/06/78/240_F_507067856_oeG1oQO9FyZhA7j0e0IpTHwsS3ZVWtln.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 5,
                    MuseumId = 5,
                    ImageUrl = "https://t4.ftcdn.net/jpg/00/83/91/39/240_F_83913965_MdPt8k5xeVa4sIx0zuajUTGGtpqQijkv.jpg"
                },

                 new MuseumImageModel
                 {
                     MuseumImageId = 6,
                     MuseumId = 6,
                     ImageUrl = "https://t3.ftcdn.net/jpg/18/33/81/44/240_F_1833814465_cDXO5nqwcDp4W9EMiY3eRVkL5zhhQlzJ.jpg"
                 },

                 new MuseumImageModel
                 {
                     MuseumImageId = 7,
                     MuseumId = 7,
                     ImageUrl = "https://choosewhere.com/public/images/Uwu6op8/crop_and_zoom_848x560_0x7_2000x1320/shutterstock_2504968227.webp"
                 },

                 new MuseumImageModel
                 {
                     MuseumImageId = 8,
                     MuseumId = 8,
                     ImageUrl = "https://t4.ftcdn.net/jpg/02/32/05/61/240_F_232056135_rijksmuseum-amsterdam.jpg"
                 },

                new MuseumImageModel
                {
                    MuseumImageId = 9,
                    MuseumId = 9,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/11/33/76/240_F_211337602_uffizi-gallery-florence.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 10,
                    MuseumId = 10,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/54/12/88/240_F_254128884_acropolis-museum-athens.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 11,
                    MuseumId = 11,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/72/48/22/240_F_272482248_egyptian-museum-cairo.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 12,
                    MuseumId = 12,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/34/71/91/240_F_234719167_air-and-space-museum.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 13,
                    MuseumId = 13,
                    ImageUrl = "https://t4.ftcdn.net/jpg/03/01/44/73/240_F_301447322_national-museum-china.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 14,
                    MuseumId = 14,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/41/02/61/240_F_241026191_tokyo-national-museum.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 15,
                    MuseumId = 15,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/56/23/01/240_F_256230145_national-museum-korea.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 16,
                    MuseumId = 16,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/60/45/11/240_F_260451198_art-institute-chicago.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 17,
                    MuseumId = 17,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/43/11/32/240_F_243113278_moma-new-york.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 18,
                    MuseumId = 18,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/67/31/19/240_F_267311973_van-gogh-museum-amsterdam.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 19,
                    MuseumId = 19,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/89/63/77/240_F_289637771_guggenheim-bilbao.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 20,
                    MuseumId = 20,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/98/51/63/240_F_298516329_wwii-museum-new-orleans.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 21,
                    MuseumId = 21,
                    ImageUrl = "https://t4.ftcdn.net/jpg/03/04/65/12/240_F_304651250_canadian-museum-history.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 22,
                    MuseumId = 22,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/93/70/11/240_F_293701149_mexico-anthropology-museum.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 23,
                    MuseumId = 23,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/58/14/27/240_F_258142731_pergamon-museum-berlin.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 24,
                    MuseumId = 24,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/62/97/71/240_F_262977152_national-palace-museum-taipei.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 25,
                    MuseumId = 25,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/95/70/15/240_F_295701556_museum-of-tomorrow-rio.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 26,
                    MuseumId = 26,
                    ImageUrl = "https://t4.ftcdn.net/jpg/03/12/11/44/240_F_312114410_te-papa-museum.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 27,
                    MuseumId = 27,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/71/60/41/240_F_271604143_national-anthropology-madrid.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 28,
                    MuseumId = 28,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/81/33/66/240_F_281336615_powerhouse-museum-sydney.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 29,
                    MuseumId = 29,
                    ImageUrl = "https://t4.ftcdn.net/jpg/03/18/02/91/240_F_318029115_apartheid-museum.jpg"
                },

                new MuseumImageModel
                {
                    MuseumImageId = 30,
                    MuseumId = 30,
                    ImageUrl = "https://t4.ftcdn.net/jpg/02/47/19/88/240_F_247198825_museum-islamic-art-doha.jpg"
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
