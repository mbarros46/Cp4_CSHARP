namespace Application.DTOs;

public class PatioRequest
{
    public required string Nome { get; set; }
    public required string Endereco { get; set; }
    public int Capacidade { get; set; }
}
