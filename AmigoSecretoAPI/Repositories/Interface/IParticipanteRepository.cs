using AmigoSecretoAPI.Models;

namespace AmigoSecretoAPI.Repositories.Interface;

public interface IParticipanteRepository
{
    Task<Participante> AdicionarParticipante(Participante participante);
    Task<Participante> ObterParticipantePorId(int id);
    Task<List<Participante>> ObterParticipantesPorGrupo(int grupoId);
    Task AtualizarParticipante(Participante participante);
}
