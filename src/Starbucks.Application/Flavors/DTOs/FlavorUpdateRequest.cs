namespace Starbucks.Application.Flavors.DTOs;
public class FlavorUpdateRequest
{
    public required string Name { get; set; }
    public required Stream Stream { get; set; }
}
