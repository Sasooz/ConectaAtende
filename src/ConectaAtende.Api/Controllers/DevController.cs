using Microsoft.AspNetCore.Mvc;
using ConectaAtende.Application.Services;

namespace ConectaAtende.Api.Controllers;

[ApiController]
[Route("dev")]
public class DevController : ControllerBase
{
    private readonly ContactService _service;

    public DevController(ContactService service)
    {
        _service = service;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed(int count = 1000)
    {
        for (int i = 0; i < count; i++)
        {
            await _service.CreateAsync(
                $"Contact {i}",
                $"3199999{i:0000}"
            );
        }

        return Ok(count);
    }
}