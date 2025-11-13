using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class TwitSharpContext : DbContext
    {
        public TwitSharpContext(DbContextOptions<TwitSharpContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PostComment → User ilişkisi: Cascade Delete KAPAT
            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.User)
                .WithMany()
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // ❗ ÖNEMLİ

            // PostLike → User ilişkisi: Cascade Delete KAPAT
            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany()
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // ❗ ÖNEMLİ
        }
    }
}
