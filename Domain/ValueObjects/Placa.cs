using System;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed class Placa
{
    public string Value { get; }

    // Accepts old Brazilian format (AAA9999) and Mercosul (AAA9A99)
    private static readonly Regex ValidPattern = new Regex(@"^([A-Z]{3}\d{4}|[A-Z]{3}\d[A-Z]\d{2})$", RegexOptions.Compiled);

    public Placa(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa não pode ser nula ou vazia.");

        var normalized = placa.Trim().ToUpperInvariant();

        if (!ValidPattern.IsMatch(normalized))
            throw new ArgumentException("Placa em formato inválido.");

        Value = normalized;
    }

    public override string ToString() => Value;

    public override bool Equals(object? obj)
        => obj is Placa other && other.Value == Value;

    public override int GetHashCode() => Value.GetHashCode(StringComparison.InvariantCulture);

    public static implicit operator string(Placa p) => p.Value;
    public static implicit operator Placa(string s) => new Placa(s);
}
