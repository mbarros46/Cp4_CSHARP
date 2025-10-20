namespace Application.DTOs;

public class PatioResponse
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Endereco { get; set; }
    public int Capacidade { get; set; }
    public int OcupacaoAtual { get; set; }
}
