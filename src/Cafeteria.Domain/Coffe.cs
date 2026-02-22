namespace Cafeteria.Domain;

public class Coffe : BaseEntity // Heredo de la clase BaseEntity para tener el Id
{
    public required string Name { get; set; } 
    public string? Description { get; set; } 
    public decimal Price { get; set; }

    public int CategoryId { get; set; }
    
    public string? Imagen { get; set; }

    public Category? Category { get; set; } // esto es para la relacion con la categoria, un cafe tiene una categoria, es decir que un cafe pertenece a una categoria
    public ICollection<Ingredient> Ingredients { get; set; } = []; // esto es para la relacion con los ingredientes, un cafe tiene muchos ingredientes, es decir que un cafe tiene una coleccion de ingredientes
    
}

