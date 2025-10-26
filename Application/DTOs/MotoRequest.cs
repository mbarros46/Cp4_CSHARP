namespace Application.DTOs;

public class MotoRequest
{
    /// <summary>
    /// Modelo da motocicleta (ex.: "Honda CG 160").
    /// </summary>
    public required string Modelo { get; set; }

    /// <summary>
    /// Placa do veículo (AAA9999 ou formato Mercosul).
    /// </summary>
    public required string Placa { get; set; }

    /// <summary>
    /// Status da moto (ex.: "Active", "Inactive").
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Ano de fabricação.
    /// </summary>
    public int Ano { get; set; }

    /// <summary>
    /// Identificador do pátio associado (opcional).
    /// </summary>
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
