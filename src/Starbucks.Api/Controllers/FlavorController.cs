using Core.mediatOR.Contracts;
using Microsoft.AspNetCore.Mvc;
using Starbucks.Application.Flavors.DTOs;
using static Starbucks.Application.Flavors.Commands.FlavorImageUpdate;
using static Starbucks.Application.Flavors.Queries.FlavorGet;

namespace Starbucks.Api.Controllers;

[Route("api/flavors")]
[ApiController]
public class FlavorController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new Query();
        var results = await _mediator.Send(query, cancellationToken);
        return Ok(results);
    }

    [HttpPut("{id}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Put(
        [FromForm] FlavorImageUpdateRequest request,
        int id,
        CancellationToken cancellationToken
    )
    {
        await using var stream = request.ImageFile?.OpenReadStream();

        var flavorRequest = new FlavorUpdateRequest
        {
            Name = request.Name!,
            Stream = stream!
        };

        var command = new Command(id, flavorRequest);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest();

    }




}

public class FlavorImageUpdateRequest
{
    public string? Name { get; set; }
    public IFormFile? ImageFile { get; set; }
}
