using System;
namespace Domain.Entities;

public class Moto
{
    public Guid Id { get; private set; }
    public string Modelo { get; private set; }
    public string Placa { get; private set; }
    public string Status { get; private set; }
    public int Ano { get; private set; }

    // relacionamento com pátio
    public Guid? PatioId { get; private set; }
    public Patio Patio { get; private set; }

    private Moto(string modelo, string placa, string status, int ano, Guid? patioId = null)
    {
        ValidateModelo(modelo);
        ValidatePlaca(placa);
        ValidateStatus(status);
        ValidateAno(ano);

        Id = Guid.NewGuid();
        Modelo = modelo;
        Placa = placa;
        Status = status;
        Ano = ano;
        PatioId = patioId;
    }

    public static Moto Create(string modelo, string placa, string status, int ano, Guid? patioId = null)
    {
        return new Moto(modelo, placa, status, ano, patioId);
    }

    public void AtualizarDados(string modelo, string placa, string status, int ano, Guid? patioId = null)
    {
        ValidateModelo(modelo);
        ValidatePlaca(placa);
        ValidateStatus(status);
        ValidateAno(ano);

        Modelo = modelo;
        Placa = placa;
        Status = status;
        Ano = ano;
        PatioId = patioId;
    }

    private void ValidateModelo(string modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo não pode ser nulo ou vazio.");
    }

    private void ValidatePlaca(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa não pode ser nula ou vazia.");
    }

    private void ValidateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status não pode ser nulo ou vazio.");
    }

    private void ValidateAno(int ano)
    {
        if (ano < 1900 || ano > DateTime.Now.Year)
            throw new ArgumentException("Ano inválido.");
    }
}