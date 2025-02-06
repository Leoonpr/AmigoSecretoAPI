namespace AmigoSecretoAPI.Models;

public class Participante
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public Grupo? Grupo { get; set; }
    public int? AmigoSecretoId { get; set; }
    public Participante? AmigoSecreto { get; set; }
}
