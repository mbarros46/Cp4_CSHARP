namespace Application.DTOs;

public class MotoRequest
{
    public required string Modelo { get; set; }
    public required string Placa { get; set; }
    public required string Status { get; set; }
    public int Ano { get; set; }
    public Guid? PatioId { get; set; }

    public MotoRequest() { }

    public MotoRequest(string modelo, string placa, string status, int ano, Guid? patioId = null)
    {
        Modelo = modelo;
        Placa = placa;
        Status = status;
        Ano = ano;
        PatioId = patioId;
    }
}
