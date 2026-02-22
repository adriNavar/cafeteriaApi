

namespace Cafeteria.Domain;

public class BaseEntity // Clase base o padre para las entidades
{
 public Guid Id { get; set; } = Guid.CreateVersion7();

 
}
