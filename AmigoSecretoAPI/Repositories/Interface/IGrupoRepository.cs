using AmigoSecretoAPI.Models;

namespace AmigoSecretoAPI.Repositories.Interface;

public interface IGrupoRepository
{
    Task<Grupo> CriarGrupo(Grupo grupo);
    Task<Grupo> ObterGrupoPorId(int id);
    Task<List<Grupo>> ObterTodosGrupos();
}
