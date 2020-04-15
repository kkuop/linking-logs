using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkingLogsWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
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
        }
    }
}
