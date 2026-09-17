using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class MovieTicketBookingDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoginDetails> LoginDetails { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=MovieTicketBookingDB;user=root;password=Arzaul123@;",
                ServerVersion.AutoDetect(
                    "server=localhost;database=MovieTicketBookingDB;user=root;password=Arzaul123@;"
                )
            );
        }
        protected override void OnModelCreating(
    ModelBuilder modelBuilder)
        {
            // LoginDetails primary key
            modelBuilder.Entity<LoginDetails>()
                .HasKey(l => l.LoginID);

            // Show → Movie
            modelBuilder.Entity<Show>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Shows)
                .HasForeignKey(s => s.MovieID)
                .OnDelete(DeleteBehavior.Restrict);

            // Show → Theatre
            modelBuilder.Entity<Show>()
                .HasOne(s => s.Theatre)
                .WithMany(t => t.Shows)
                .HasForeignKey(s => s.TheatreID)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking → Customer
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking → Show
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ShowID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}