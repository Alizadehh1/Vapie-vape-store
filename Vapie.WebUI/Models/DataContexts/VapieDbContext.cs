using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.Entities.Membership;
using Vapie.WebUI.Models.FormModels;

namespace Vapie.WebUI.Models.DataContexts
{
    public class VapieDbContext : IdentityDbContext<VapieUser, VapieRole, int, VapieUserClaim, VapieUserRole, VapieUserLogin, VapieRoleClaim, VapieUserToken>
    {
        public VapieDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChangePasswordFormModel>(e =>
            {
                e.HasNoKey();
            });

            builder.Entity<VapieUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });
            builder.Entity<VapieRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });
            builder.Entity<VapieUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });
            builder.Entity<VapieUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });
            builder.Entity<VapieRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });
            builder.Entity<VapieUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });
            builder.Entity<VapieUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }

        public DbSet<Vapie.WebUI.Models.FormModels.ChangePasswordFormModel> ChangePasswordFormModel { get; set; }

    }
}
