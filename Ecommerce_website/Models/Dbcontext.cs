using Microsoft.EntityFrameworkCore;

namespace Ecommerce_website.Models
{
    public class Dbcontext : DbContext
    {
        public Dbcontext(DbContextOptions<Dbcontext> options ) : base( options ) 
        {
                
        }
        public DbSet<Admin> admin_tbl {  get; set; }
        public DbSet<Customer> customer_tbl { get; set; }
        public DbSet<Category> category_tbl { get; set; }
        public DbSet<Product> product_tbl { get; set; }

        public DbSet<Cart> cart_tbl { get; set; }
        public DbSet<Feedback> feedback_tbl { get; set; }
        public DbSet<Faqs> faq_tbl { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(p=> p.category).WithMany(c=> c.products).HasForeignKey(p=>p.cat_id);
        }
    }
}
