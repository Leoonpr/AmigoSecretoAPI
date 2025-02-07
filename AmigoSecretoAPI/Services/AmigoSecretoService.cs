using System.Net.Mail;
using AmigoSecretoAPI.Models;
using AmigoSecretoAPI.Repositories.Interface;
using AmigoSecretoAPI.Services.Interface;
using MailKit.Security;
using MimeKit;

namespace AmigoSecretoAPI.Services;

public class AmigoSecretoService : IAmigoSecretoService
{
  private readonly IGrupoRepository _grupoRepository;
    private readonly IParticipanteRepository _participanteRepository;

    public AmigoSecretoService(IGrupoRepository grupoRepository, IParticipanteRepository participanteRepository)
    {
        _grupoRepository = grupoRepository;
        _participanteRepository = participanteRepository;
    }

    public async Task<Grupo> CriarGrupo(Grupo grupo)
    {
        return await _grupoRepository.CriarGrupo(grupo);
    }

    public async Task<Participante> AdicionarParticipante(Participante participante)
    {
        return await _participanteRepository.AdicionarParticipante(participante);
    }

    public async Task<List<Participante>> ObterParticipantesPorGrupo(int grupoId)
    {
        return await _participanteRepository.ObterParticipantesPorGrupo(grupoId);
    }

    public async Task GerarSorteio(int grupoId)
    {
        var participantes = await _participanteRepository.ObterParticipantesPorGrupo(grupoId);
        var random = new Random();
        var shuffled = participantes.OrderBy(p => random.Next()).ToList();

        for (int i = 0; i < shuffled.Count; i++)
        {
            shuffled[i].AmigoSecretoId = shuffled[(i + 1) % shuffled.Count].Id;
            await _participanteRepository.AtualizarParticipante(shuffled[i]);
        }

        foreach (var participante in participantes)
        {
            var amigoSecreto = await _participanteRepository.ObterParticipantePorId(participante.AmigoSecretoId.Value);
            await EnviarEmail(participante.Email, amigoSecreto.Nome);
        }
    }

    public async Task<string> ObterAmigoSorteado(int participanteId)
    {
        var participante = await _participanteRepository.ObterParticipantePorId(participanteId);
        if (participante == null || participante.AmigoSecretoId == null)
        {
            throw new Exception("Participante ou amigo secreto não encontrado.");
        }

        var amigoSecreto = await _participanteRepository.ObterParticipantePorId(participante.AmigoSecretoId.Value);
        return amigoSecreto.Nome;
    }

    private async Task EnviarEmail(string email, string amigoSecreto)
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Amigo Secreto", "no-reply@amigosecreto.com"));
    message.To.Add(new MailboxAddress("", email));
    message.Subject = "Seu Amigo Secreto!";
    message.Body = new TextPart("plain")
    {
        Text = $"Você tirou: {amigoSecreto}"
    };

    using (var client = new MailKit.Net.Smtp.SmtpClient())
    {
        await client.ConnectAsync("smtp.example.com", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("user", "password");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
}
