using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AmigoSecretoAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantesController : ControllerBase
{
    private readonly IAmigoSecretoService _amigoSecretoService;

    public ParticipantesController(IAmigoSecretoService amigoSecretoService)
    {
        _amigoSecretoService = amigoSecretoService;
    }

    [HttpPost]
    public async Task<ActionResult<Participante>> AdicionarParticipante(Participante participante)
    {
        var participanteAdicionado = await _amigoSecretoService.AdicionarParticipante(participante);
        return CreatedAtAction(nameof(GetParticipante), new { id = participanteAdicionado.Id }, participanteAdicionado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Participante>> GetParticipante(int id)
    {
        var participante = await _amigoSecretoService.ObterParticipantePorId(id);
        if (participante == null)
        {
            return NotFound();
        }
        return participante;
    }

    [HttpGet("grupo/{grupoId}")]
    public async Task<ActionResult<List<Participante>>> GetParticipantesPorGrupo(int grupoId)
    {
        var participantes = await _amigoSecretoService.ObterParticipantesPorGrupo(grupoId);
        return participantes;
    }

    [HttpPost("sorteio/{grupoId}")]
    public async Task<ActionResult> GerarSorteio(int grupoId)
    {
        await _amigoSecretoService.GerarSorteio(grupoId);
        return Ok();
    }

    [HttpGet("sorteio/{participanteId}")]
    public async Task<ActionResult<string>> GetAmigoSorteado(int participanteId)
    {
        try
        {
            var amigoSecreto = await _amigoSecretoService.ObterAmigoSorteado(participanteId);
            return amigoSecreto;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
