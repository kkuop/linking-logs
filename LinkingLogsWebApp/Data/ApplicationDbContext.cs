using System;
using System.Collections.Generic;
using System.Text;
using LinkingLogsWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkingLogsWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobBid> JobBids { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteManager> SiteManagers { get; set; }
        public DbSet<Trucker> Truckers { get; set; }
        public DbSet<WoodType> WoodTypes { get; set; }
        public DbSet<Mill> Mills { get; set; }
        public DbSet<MillWoodType> MillWoodTypes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole
                    {
                        Id = "63f5608d-ee63-47bb-b28a-e19df3176a1e",
                        ConcurrencyStamp = "019e9155-bef1-4fe2-ac03-ae6d642dc869",
                        Name = "SiteManager",
                        NormalizedName = "SITEMANAGER"
                    },
                    new IdentityRole
                    {
                        Id = "ffa519d6-987c-4cf8-82b6-0c648a9d778a",
                        ConcurrencyStamp = "9d56beee-fa4f-4cfe-9449-0ada54ce389a",
                        Name = "Trucker",
                        NormalizedName = "TRUCKER"
                    }
                );
            builder.Entity<WoodType>()
                .HasData(
                    new WoodType { WoodTypeId = 1, Type="Birch"},
                    new WoodType { WoodTypeId = 2, Type="Cedar"},
                    new WoodType { WoodTypeId = 3, Type="Cypress"},
                    new WoodType { WoodTypeId = 4, Type="Douglas-Fir"},
                    new WoodType { WoodTypeId = 5, Type="Fir"},
                    new WoodType { WoodTypeId = 6, Type="Hemlock"},
                    new WoodType { WoodTypeId = 7, Type="Pine"},
                    new WoodType { WoodTypeId = 8, Type="Redwood"},
                    new WoodType { WoodTypeId = 9, Type="Spruce"},
                    new WoodType { WoodTypeId = 10, Type="Cherry"},
                    new WoodType { WoodTypeId = 11, Type="Chestnut"},
                    new WoodType { WoodTypeId = 12, Type="Ironwood"}
                );
            builder.Entity<Mill>()
                .HasData(
                    new Mill { MillId = 1, Name="Superior", Address = "N-1064, US-41, Trenary, MI" },
                    new Mill { MillId = 2, Name="Gwinn", Address = "650 Avenue A, Gwinn, MI"},
                    new Mill { MillId = 3, Name="Biewer Prentice", Address = "400 Red Pine Ct, Prentice, WI"},
                    new Mill { MillId = 4, Name="Biewer Lake City", Address="1560 W Houghton Lake Rd, Lake City, MI"},
                    new Mill { MillId = 5, Name="Biewer McBain", Address = "6251 W Gerwoude Drive, McBain, MI"},
                    new Mill { MillId = 6, Name="Atwood",Address="1177 17 Mile Rd NE, Cedar Springs, MI"}
                );
            builder.Entity<MillWoodType>()
                .HasData(
                    new MillWoodType { MillWoodTypeId = 1, MillId = 1, WoodTypeId = 4},
                    new MillWoodType { MillWoodTypeId = 2, MillId = 1, WoodTypeId = 5},
                    new MillWoodType { MillWoodTypeId = 3, MillId = 1, WoodTypeId = 6 },
                    new MillWoodType { MillWoodTypeId = 4, MillId = 1, WoodTypeId = 7 },
                    new MillWoodType { MillWoodTypeId = 5, MillId = 2, WoodTypeId = 1 },
                    new MillWoodType { MillWoodTypeId = 6, MillId = 2, WoodTypeId = 2 },
                    new MillWoodType { MillWoodTypeId = 7, MillId = 2, WoodTypeId = 3 },
                    new MillWoodType { MillWoodTypeId = 8, MillId = 2, WoodTypeId = 4 },
                    new MillWoodType { MillWoodTypeId = 9, MillId = 3, WoodTypeId = 8 },
                    new MillWoodType { MillWoodTypeId = 10, MillId = 3, WoodTypeId = 9 },
                    new MillWoodType { MillWoodTypeId = 11, MillId = 3, WoodTypeId = 10 },
                    new MillWoodType { MillWoodTypeId = 12, MillId = 3, WoodTypeId = 11 },
                    new MillWoodType { MillWoodTypeId = 13, MillId = 4, WoodTypeId = 12 },
                    new MillWoodType { MillWoodTypeId = 14, MillId = 4, WoodTypeId = 1 },
                    new MillWoodType { MillWoodTypeId = 15, MillId = 4, WoodTypeId = 3 },
                    new MillWoodType { MillWoodTypeId = 16, MillId = 4, WoodTypeId = 5 },
                    new MillWoodType { MillWoodTypeId = 17, MillId = 5, WoodTypeId = 7 },
                    new MillWoodType { MillWoodTypeId = 18, MillId = 5, WoodTypeId = 9 },
                    new MillWoodType { MillWoodTypeId = 19, MillId = 5, WoodTypeId = 11 },
                    new MillWoodType { MillWoodTypeId = 20, MillId = 5, WoodTypeId = 2 },
                    new MillWoodType { MillWoodTypeId = 21, MillId = 6, WoodTypeId = 4 },
                    new MillWoodType { MillWoodTypeId = 22, MillId = 6, WoodTypeId = 6 },
                    new MillWoodType { MillWoodTypeId = 23, MillId = 6, WoodTypeId = 8 },
                    new MillWoodType { MillWoodTypeId = 24, MillId = 6, WoodTypeId = 10 }
                );
        }
    }
}
