
namespace Cafeteria.Domain;

public class Ingredient : BaseEntity
{
    public required string Name { get; set; }
    public ICollection <Coffe> Coffes { get; set; } = []; // esto es para la relacion con los cafes, un ingrediente puede estar en muchos cafes, es decir que un ingrediente tiene una coleccion de cafes
    public ICollection<CoffeIngredient> CoffeIngredient { get; set; } = []; // esto es para la relacion con los cafes, un ingrediente puede estar en muchos cafes, es decir que un ingrediente tiene una coleccion de cafes ingredientes

}
