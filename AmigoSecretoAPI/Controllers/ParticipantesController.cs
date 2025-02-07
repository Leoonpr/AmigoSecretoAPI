using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmigoSecretoAPI.Controllers
{
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
        public async Task<ActionResult<Participante>> AdicionarParticipante([FromBody] ParticipanteDto participanteDto)
        {
            var grupo = await _amigoSecretoService.ObterGrupoPorId(participanteDto.GrupoId);
            if (grupo == null)
            {
                return BadRequest("Grupo não encontrado.");
            }

            // Criar o novo participante com os dados recebidos
            var participante = new Participante
            {
                Nome = participanteDto.Nome,
                Email = participanteDto.Email,
                GrupoId = participanteDto.GrupoId,
                Grupo = grupo // Associa o participante ao grupo
            };

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
        public async Task<ActionResult<List<ParticipanteResponseDto>>> GetParticipantesPorGrupo(int grupoId)
        {
            var participantes = await _amigoSecretoService.ObterParticipantesPorGrupo(grupoId);
            
            // Mapeie para o DTO
            var participantesResponse = participantes.Select(p => new ParticipanteResponseDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Email = p.Email,
                AmigoSecretoId = p.AmigoSecretoId,
                AmigoSecretoNome = p.AmigoSecreto?.Nome
            }).ToList();

            return participantesResponse;
        }


        [HttpPost("sorteio/{grupoId}")]
        public async Task<ActionResult> GerarSorteio(int grupoId)
        {
            try
            {
                await _amigoSecretoService.GerarSorteio(grupoId);
                return Ok("Sorteio gerado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao gerar sorteio: {ex.Message}");
            }
        }

        [HttpGet("sorteio/{participanteId}")]
        public async Task<ActionResult<string>> GetAmigoSorteado(int participanteId)
        {
            try
            {
                var amigoSecreto = await _amigoSecretoService.ObterAmigoSorteado(participanteId);
                if (string.IsNullOrEmpty(amigoSecreto))
                {
                    return NotFound("Amigo secreto não encontrado.");
                }
                return Ok(amigoSecreto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter amigo secreto: {ex.Message}");
            }
        }
    }
}
