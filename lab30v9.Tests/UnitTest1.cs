using Xunit;
using lab30v9.Services;

namespace lab30v9.Tests;

public class TemperatureConverterTests
{
    private readonly TemperatureConverter _converter = new();

    [Fact]
    public void Convert_Zero()
    {
        Assert.Equal(32, _converter.CelsiusToFahrenheit(0));
    }

    [Fact]
    public void Convert_Negative()
    {
        Assert.Equal(14, _converter.CelsiusToFahrenheit(-10));
    }

    [Fact]
    public void Convert_Boiling()
    {
        Assert.Equal(212, _converter.CelsiusToFahrenheit(100));
    }

    [Fact]
    public void Convert_Decimal()
    {
        Assert.Equal(97.88, _converter.CelsiusToFahrenheit(36.6), 2);
    }

    [Fact]
    public void Convert_Large()
    {
        Assert.Equal(1832, _converter.CelsiusToFahrenheit(1000));
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(10, 50)]
    [InlineData(20, 68)]
    [InlineData(30, 86)]
    [InlineData(-40, -40)]
    public void Convert_Theory(double input, double expected)
    {
        Assert.Equal(expected, _converter.CelsiusToFahrenheit(input));
    }

    [Theory]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public void Convert_Extreme(double input)
    {
        var result = _converter.CelsiusToFahrenheit(input);
        Assert.False(double.IsNaN(result));
    }
}