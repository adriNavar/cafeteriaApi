namespace Starbucks.Application.Flavors.DTOs;
public class FlavorResponse
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string> Images { get; set; } = [];
}
