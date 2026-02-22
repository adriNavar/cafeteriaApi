

namespace Cafeteria.Domain;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public string?  Description { get; set; }
    public List<Coffe> Coffes { get; set; } = []; // quiere decir que hay una coleccion de cafes, es decir que una categoria tiene muchos cafes
    

}
