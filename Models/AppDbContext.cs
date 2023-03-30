using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//Tích hợp Entity Framework
// dotnet add package Microsoft.EntityFrameworkCore.Design
// dotnet add package Microsoft.EntityFrameworkCore.SqlServer
//Tích hợp Identity
// dotnet add package Microsoft.AspNetCore.Identity
// dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

namespace WebApiBlog.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            // Bỏ tiền tố AspNet của các bảng: mặc định
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName()!;
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category"); //Tùy chọn tên của bảng là Category (mặc định là category)
                entity.Property(c => c.UserId).IsRequired();
                entity.HasKey(c => c.Id); //Trường Id làm khóa chính
                entity.HasOne(c => c.User)
                    .WithMany(u => u.Categories)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                //entity.HasIndex(p => p.UserName)     // Đánh chỉ mục UserName (user_name)
                //        .IsUnique(true);               // Unique
            });
            builder.Entity<Post>(entity =>
            {

                entity.ToTable("Post"); //Tùy chọn tên của bảng là Post (mặc định là post)
                entity.HasKey(p => p.Id); //Trường Id làm khóa chính
                entity.Property(p => p.CategoryId).IsRequired();
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Posts)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
                //entity.HasIndex(p => p.UserName)     // Đánh chỉ mục UserName (user_name)
                //        .IsUnique(true);               // Unique
            });
        }

        public DbSet<Category> Categories { set; get; }
        public DbSet<Post> Posts { set; get; }

    }
}