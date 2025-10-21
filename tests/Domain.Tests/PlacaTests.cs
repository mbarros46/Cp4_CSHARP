using System;
using Xunit;
using Domain.ValueObjects;

namespace Domain.Tests;

public class PlacaTests
{
    [Theory]
    [InlineData("ABC1234")]
    [InlineData("ABC1D23")]
    [InlineData("abc1234")] // will normalize to uppercase
    public void Placa_ValidFormats_ShouldNotThrow(string value)
    {
        var placa = new Placa(value);
        Assert.Equal(value.ToUpperInvariant(), placa.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("ABCDE12")]
    [InlineData("AA11111")]
    public void Placa_InvalidFormats_ShouldThrow(string? value)
    {
        Assert.Throws<ArgumentException>(() => new Placa(value!));
    }
}
