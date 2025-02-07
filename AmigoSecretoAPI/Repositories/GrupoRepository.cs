using AmigoSecretoAPI.Context;
using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace AmigoSecretoAPI.Repositories;

public class GrupoRepository : IGrupoRepository
{
    private readonly AmigoSecretoContext _context;

    public GrupoRepository(AmigoSecretoContext context)
    {
        _context = context;
    }

    public async Task<Grupo> CriarGrupo(Grupo grupo)
    {
        _context.Grupos.Add(grupo);
        await _context.SaveChangesAsync();
        return grupo;
    }

    public async Task<Grupo> ObterGrupoPorId(int id)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Grupos.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.

    }

    public async Task<List<Grupo>> ObterTodosGrupos()
    {
        return await _context.Grupos.ToListAsync();
    }
}
