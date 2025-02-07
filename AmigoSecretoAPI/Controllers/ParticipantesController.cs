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

        // Adicionar um participante ao grupo
        [HttpPost]
        public async Task<ActionResult<Participante>> AdicionarParticipante([FromBody] ParticipanteDto participanteDto)
        {
            // Obter o grupo com base no grupoId
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

            // Adicionar o participante
            var participanteAdicionado = await _amigoSecretoService.AdicionarParticipante(participante);

            // Retornar a resposta com o participante adicionado, com a URL do recurso criado
            return CreatedAtAction(nameof(GetParticipante), new { id = participanteAdicionado.Id }, participanteAdicionado);
        }

        // Obter um participante por ID
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

        // Obter todos os participantes de um grupo
        
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


        // Gerar o sorteio de amigo secreto para um grupo
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

        // Obter o amigo secreto sorteado para um participante
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
