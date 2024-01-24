using Microsoft.EntityFrameworkCore;
using Redis.Example.Apı.Model;

namespace Redis.Example.Apı.Context
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<About> Abouts { get; set; }
		public DbSet<Contact>  Contacts { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		   modelBuilder.Entity<Product>().HasData(
			   
			   new Product() { Id=1,Name="Kalem"},
               new Product() { Id = 2, Name = "Silgi",Price=20 ,Description="lorem ıpsum"},
			   new Product() { Id = 3, Name = "Defter" , Price = 20 ,Description = "lorem ıpsum"},
			   new Product() { Id = 4, Name = "Kitap" , Price = 20 ,Description="lorem ıpsum"}
         );

			modelBuilder.Entity<Category>().HasData(

			   new Category() { Id = 1, Name = "Kırtasiye",Description="lorem ıpsum"}
			   
		 );

			modelBuilder.Entity<About>().HasData(

			   new About() { Id = 1, Title = "Lorem ıpsum" ,Description = "lorem ıpsum" }

		 );

			modelBuilder.Entity<Contact>().HasData(

			   new Contact() { Id = 1, Name= "Lorem ıpsum" ,Email = "lorem ıpsum" }

		 );


		}
	}
}
