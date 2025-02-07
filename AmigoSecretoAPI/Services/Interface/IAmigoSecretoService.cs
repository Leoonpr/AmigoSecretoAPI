using AmigoSecretoAPI.Models;

namespace AmigoSecretoAPI.Services.Interface;

public interface IAmigoSecretoService
{
    Task<Grupo> CriarGrupo(Grupo grupo);
    Task<Participante> AdicionarParticipante(Participante participante);
    Task<List<Participante>> ObterParticipantesPorGrupo(int grupoId);
    Task<Grupo> ObterGrupoPorId(int id);
    Task<Participante> ObterParticipantePorId(int id);
    Task GerarSorteio(int grupoId);
    Task<string> ObterAmigoSorteado(int participanteId);
}
