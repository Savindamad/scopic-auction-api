using Microsoft.EntityFrameworkCore;

#nullable disable

namespace auction_api.Models
{
    public partial class AuctionDbContext : DbContext
    {
        public AuctionDbContext()
        {
        }

        public AuctionDbContext(DbContextOptions<AuctionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<UserConfig> UserConfigs { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<UserItem> UserItems { get; set; }
        public virtual DbSet<UserItemHistory> UserItemHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClosingTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("closing_time");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.Property(e => e.MaxBidUserItemId).HasColumnName("max_bid_user_item_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(19, 2)")
                    .HasColumnName("price");

                entity.HasOne(d => d.MaxBidUserItem)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.MaxBidUserItemId)
                    .HasConstraintName("FK__item__max_bid_us__73BA3083");
            });

            modelBuilder.Entity<UserConfig>(entity =>
            {
                entity.ToTable("user_config");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BalanceBidPrice)
                    .HasColumnType("decimal(19, 0)")
                    .HasColumnName("balance_bid_price");

                entity.Property(e => e.MaxBidPrice)
                    .HasColumnType("decimal(19, 0)")
                    .HasColumnName("max_bid_price");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserConfigs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__user_conf__user___6754599E");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("user_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserItem>(entity =>
            {
                entity.ToTable("user_item");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsAutoBid).HasColumnName("is_auto_bid");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.MaxBidUserItemHistoryId).HasColumnName("max_bid_user_item_history_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserMaxBid)
                    .HasColumnType("decimal(19, 0)")
                    .HasColumnName("user_max_bid");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.UserItems)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__user_item__item___6FE99F9F");

                entity.HasOne(d => d.MaxBidUserItemHistory)
                    .WithMany(p => p.UserItems)
                    .HasForeignKey(d => d.MaxBidUserItemHistoryId)
                    .HasConstraintName("FK__user_item__max_b__71D1E811");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__user_item__user___70DDC3D8");
            });

            modelBuilder.Entity<UserItemHistory>(entity =>
            {
                entity.ToTable("user_item_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(19, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Time)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.UserItemHistories)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__item_bid__item_i__47DBAE45");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserItemHistories)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__item_bid__user_i__48CFD27E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
