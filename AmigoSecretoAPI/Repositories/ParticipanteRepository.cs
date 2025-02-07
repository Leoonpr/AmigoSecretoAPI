using AmigoSecretoAPI.Context;
using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmigoSecretoAPI.Repositories;

public class ParticipanteRepository : IParticipanteRepository
{
  private readonly AmigoSecretoContext _context;

    public ParticipanteRepository(AmigoSecretoContext context)
    {
        _context = context;
    }

    public async Task<Participante> AdicionarParticipante(Participante participante)
    {
        _context.Participantes.Add(participante);
        await _context.SaveChangesAsync();
        return participante;
    }

    public async Task<Participante> ObterParticipantePorId(int id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Participantes.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.

    }

    public async Task<List<Participante>> ObterParticipantesPorGrupo(int grupoId)
    {
        return await _context.Participantes
        .Where(p => p.Grupo != null && p.Grupo.Id == grupoId)
        .ToListAsync();
    }


    public async Task AtualizarParticipante(Participante participante)
    {
        _context.Participantes.Update(participante);
        await _context.SaveChangesAsync();
    }

}
