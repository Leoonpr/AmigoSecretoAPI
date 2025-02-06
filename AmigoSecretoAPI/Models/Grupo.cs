namespace AmigoSecretoAPI.Models;

public class Grupo
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public List<Participante> Participantes { get; set; } = new List<Participante>();
}
