using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NetCore3MVC.Models
{
    public partial class VlogContext : DbContext
    {
        public VlogContext()
        {
        }

        public VlogContext(DbContextOptions<VlogContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<ArticleСategory> ArticleCategories { get; set; }
        public virtual DbSet<FileType> FileTypes { get; set; }
        public virtual DbSet<MediaFile> MediaFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Vlog;Username=postgres;Password=qwerty");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(20)
                    .HasColumnName("country");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("firstname");

                entity.Property(e => e.Secondname)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("secondname");

                entity.Property(e => e.Userlogin)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("userlogin");

                entity.Property(e => e.Userpassword)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("userpassword");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(8);

                entity.Property(e => e.RoleId)
                    .HasColumnName("roleid");

                //entity.HasData(new User() { Id = 1, Firstname = "Admin", Secondname = "admin", Userlogin = "admin", Userpassword = "admin", RoleId = 1 });
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(16);

               // entity.HasData(new Role() { Id = 1, Name = "Admin" });

            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("articles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnName("title");

                entity.Property(e => e.Content)
                    .HasColumnName("content");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnName("createtimestamp");

                entity.Property(e => e.AuthorId)
                    .HasColumnName("authorid");
            });

            modelBuilder.Entity<New>(entity =>
            {
                entity.ToTable("news");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnName("title");

                entity.Property(e => e.Content)
                    .HasColumnName("content");

                entity.Property(e => e.CreateTimestamp)
                    .HasColumnName("createtimestamp");
            });

            modelBuilder.Entity<ArticleСategory>(entity =>
            {
                entity.ToTable("ArticleCategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e=>e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.ToTable("MediaFile");

                entity.Property(e=>e.Id).HasColumnName("id");

                entity.Property(e=>e.Path).HasColumnName("path");
            });

            modelBuilder.Entity<FileType>(entity =>
            {
                entity.ToTable("FileType");

                entity.Property(e=>e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
