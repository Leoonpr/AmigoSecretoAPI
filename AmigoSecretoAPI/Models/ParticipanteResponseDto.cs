namespace AmigoSecretoAPI.Models;

public class ParticipanteResponseDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public int? AmigoSecretoId { get; set; }
    public string? AmigoSecretoNome { get; set; }
}
