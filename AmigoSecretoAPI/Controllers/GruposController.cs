using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AmigoSecretoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GruposController : ControllerBase
{
    private readonly IAmigoSecretoService _amigoSecretoService;

    public GruposController(IAmigoSecretoService amigoSecretoService)
    {
        _amigoSecretoService = amigoSecretoService;
    }

    [HttpPost]
    public async Task<ActionResult<Grupo>> CriarGrupo(Grupo grupo)
    {
        var grupoCriado = await _amigoSecretoService.CriarGrupo(grupo);
        return CreatedAtAction(nameof(GetGrupo), new { id = grupoCriado.Id }, grupoCriado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Grupo>> GetGrupo(int id)
    {
        var grupo = await _amigoSecretoService.ObterGrupoPorId(id);
        if (grupo == null)
        {
            return NotFound();
        }
        return grupo;
    }
}