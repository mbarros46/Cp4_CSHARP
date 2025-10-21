using System;
using Domain.ValueObjects;

Console.WriteLine("VO Validator - Placa samples");
var samples = new[] { "ABC1234", "abc1234", "ABC1D23", "123", "" };
foreach (var s in samples)
{
    try
    {
        var p = new Placa(s);
        Console.WriteLine($"OK: {s} -> {p}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERR: {s} -> {ex.Message}");
    }
}

return 0;
