namespace Application.DTOs;

public class MotoResponse
{
    public Guid Id { get; set; }
    /// <summary>
    /// Modelo da motocicleta.
    /// </summary>
    public required string Modelo { get; set; }

    /// <summary>
    /// Placa do veículo (normalizada pelo ValueObject).
    /// </summary>
    public required string Placa { get; set; }

    /// <summary>
    /// Status da moto.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Ano de fabricação.
    /// </summary>
    public int Ano { get; set; }

    /// <summary>
    /// Identificador do pátio associado (se houver).
    /// </summary>
    public Guid? PatioId { get; set; }

    public MotoResponse() { }

    public MotoResponse(Guid id, string modelo, string placa, string status, int ano, Guid? patioId)
    {
        Id = id;
        Modelo = modelo;
        Placa = placa;
        Status = status;
        Ano = ano;
        PatioId = patioId;
    }
}
