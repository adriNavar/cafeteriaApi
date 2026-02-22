using Cafeteria.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Persistence;

public class CafeteriaDbContext : DbContext

{
 public required DbSet<Category> Categories { get; set; } 
 public required DbSet<Coffe> Coffes { get; set; }
 public required DbSet<Ingredient> Ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         base.OnModelCreating(modelBuilder);// Es partede del dbcontext, es un creador de modelos 
         //Categoria
         modelBuilder.Entity<Category>()
        .HasMany(c => c.Coffes) // dice que una categoria tiene muchos cafes
        .WithOne(co => co.Category) // y un cafe una categoria
        .HasForeignKey(co => co.CategoryId) // la clave foranea es el CategoryId que esta en la clase Coffe
        .IsRequired() // indica que la categoria es requerida en el cafe
        .OnDelete(DeleteBehavior.Cascade); // cuando se elimina una categoria, se eliminan los cafes asociados

        //Cafe
        modelBuilder.Entity<Coffe>()
        .Property(co=> co.Price)
        .HasPrecision(10,2); // esto es para que el precio tenga 10 digitos en total y 2 decimales
    
    modelBuilder.Entity<Coffe>()
        .HasMany(co => co.Ingredients) // un cafe tiene muchos ingredientes
        .WithOne(c => c.Coffe) // y un ingrediente pertenece a un cafe
        .HasForeignKey(i => i.CoffeId) // la clave foranea es el CoffeId que esta en la clase Ingredient
        .IsRequired() // indica que el cafe es requerido en el ingrediente
        .OnDelete(DeleteBehavior.Cascade); // cuando se elimina un cafe, se eliminan los ingredientes asociados
    } 

}
