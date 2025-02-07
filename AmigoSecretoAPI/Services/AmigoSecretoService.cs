using System.Net;
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
    public async Task<Grupo> ObterGrupoPorId(int id)
    {
      return await _grupoRepository.ObterGrupoPorId(id);
    }

    public async Task<Participante> ObterParticipantePorId(int id)
    {
      return await _participanteRepository.ObterParticipantePorId(id);
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
            if (participante.AmigoSecretoId.HasValue)
            {
                var amigoSecreto = await _participanteRepository.ObterParticipantePorId(participante.AmigoSecretoId.Value);
                if (amigoSecreto != null)
                {
                    string corpoEmail = $"Olá {participante.Nome},\n\nSeu amigo secreto é: {amigoSecreto.Nome}.";
                    await EnviarEmail(participante.Email, amigoSecreto.Nome, corpoEmail);
                }
                else
                {
                    throw new InvalidOperationException($"Amigo secreto não encontrado para o participante {participante.Nome}.");
                }
            }
            else
            {
                throw new InvalidOperationException($"Amigo secreto não atribuído para o participante {participante.Nome}.");
            }
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

     public async Task EnviarEmail(string destinatario, string assunto, string corpo)
    {
        var smtpClient = new SmtpClient("localhost")
        {
            Port = 25,
            Credentials = new NetworkCredential("", ""),  
            EnableSsl = false
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("seu-email@dominio.com"),
            Subject = assunto,
            Body = corpo,
            IsBodyHtml = true
        };

        mailMessage.To.Add(destinatario);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            Console.WriteLine("Email enviado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar o email: {ex.Message}");
        }
    }
}
