using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class ClientTests
{
    [Test]
    [Arguments(null, "key", "pin", "seed")]
    [Arguments("", "key", "pin", "seed")]
    [Arguments(" ", "key", "pin", "seed")]
    [Arguments("url", null, "pin", "seed")]
    [Arguments("url", "", "pin", "seed")]
    [Arguments("url", " ", "pin", "seed")]
    [Arguments("url", "key", null, "seed")]
    [Arguments("url", "key", "", "seed")]
    [Arguments("url", "key", " ", "seed")]
    [Arguments("url", "key", "pin", null)]
    [Arguments("url", "key", "pin", "")]
    [Arguments("url", "key", "pin", " ")]
    public async Task Constructor_NullOrEmptyArgs_ThrowsArgumentException(
        string? apiUrl, string? apiKey, string? apiPin, string? randomSeed)
    {
        await Assert.That(() => new UsaEPayClient(apiUrl!, apiKey!, apiPin!, randomSeed!))
            .ThrowsException();
    }

    [Test]
    public async Task Constructor_ValidArgs_DoesNotThrow()
    {
        using var client = new UsaEPayClient("v2", "testkey", "1234", "randomseed", useSandbox: true);
        await Assert.That(client).IsNotNull();
    }

    [Test]
    public async Task Disposed_Client_ThrowsObjectDisposedException()
    {
        var client = new UsaEPayClient("v2", "testkey", "1234", "randomseed", useSandbox: true);
        client.Dispose();

        var request = new UsaEPayRequest
        {
            Endpoint = "transactions",
            Command = "cc:sale"
        };

        await Assert.That(async () => await client.SendRequest<UsaEPayResponse>(request))
            .ThrowsExactly<ObjectDisposedException>();
    }
}
