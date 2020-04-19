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
        }
    }
}
