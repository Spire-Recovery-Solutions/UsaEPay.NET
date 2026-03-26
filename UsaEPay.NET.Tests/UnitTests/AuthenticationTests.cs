using System.Security.Cryptography;
using System.Text;
using UsaEPay.NET.Models.Authentication;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class AuthenticationTests
{
    [Test]
    public async Task Constructor_KnownVector_ProducesCorrectAuthKey()
    {
        // Arrange
        var seed = "abcdefghijklmnop";
        var apiKey = "testkey";
        var apiPin = "1234";

        var prehash = apiKey + seed + apiPin; // "testkeyabcdefghijklmnop1234"
        var sha256Hex = ComputeSha256Hex(prehash);
        var expectedApiHash = "s2/" + seed + "/" + sha256Hex;
        var expectedBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(apiKey + ":" + expectedApiHash));
        var expectedAuthKey = "Basic " + expectedBase64;

        // Act
        var auth = new Authentication(seed, apiKey, apiPin);

        // Assert
        await Assert.That(auth.AuthKey).IsEqualTo(expectedAuthKey);
    }

    [Test]
    public async Task Constructor_KnownVector_AuthKeyStartsWithBasic()
    {
        // Arrange & Act
        var auth = new Authentication("abcdefghijklmnop", "testkey", "1234");

        // Assert
        await Assert.That(auth.AuthKey).StartsWith("Basic ");
    }

    [Test]
    public async Task Constructor_KnownVector_AuthKeyContainsBase64()
    {
        // Arrange & Act
        var auth = new Authentication("abcdefghijklmnop", "testkey", "1234");

        // Assert — the base64 portion should decode without throwing
        var base64Part = auth.AuthKey["Basic ".Length..];
        var decoded = Encoding.ASCII.GetString(Convert.FromBase64String(base64Part));
        await Assert.That(decoded).StartsWith("testkey:");
    }

    [Test]
    public async Task Constructor_KnownVector_HashFormatContainsSeedAndSha256()
    {
        // Arrange
        var seed = "abcdefghijklmnop";
        var apiKey = "testkey";
        var apiPin = "1234";

        // Act
        var auth = new Authentication(seed, apiKey, apiPin);

        // Extract the apihash from the decoded base64
        var base64Part = auth.AuthKey["Basic ".Length..];
        var decoded = Encoding.ASCII.GetString(Convert.FromBase64String(base64Part));
        var colonIndex = decoded.IndexOf(':');
        var apihash = decoded[(colonIndex + 1)..];

        // Assert — hash format is "s2/<seed>/<sha256hex>"
        await Assert.That(apihash).StartsWith("s2/" + seed + "/");

        // The SHA256 hex portion should be 64 characters
        var parts = apihash.Split('/');
        await Assert.That(parts).HasCount().EqualTo(3);
        await Assert.That(parts[2]).HasLength().EqualTo(64);
    }

    [Test]
    public async Task Constructor_KnownVector_PrehashIsConcatenationOfKeyAndSeedAndPin()
    {
        // Arrange
        var seed = "myseed";
        var apiKey = "mykey";
        var apiPin = "mypin";
        var expectedPrehash = "mykeymyseedmypin";
        var expectedSha256 = ComputeSha256Hex(expectedPrehash);

        // Act
        var auth = new Authentication(seed, apiKey, apiPin);

        // Extract hash
        var base64Part = auth.AuthKey["Basic ".Length..];
        var decoded = Encoding.ASCII.GetString(Convert.FromBase64String(base64Part));
        var apihash = decoded[(decoded.IndexOf(':') + 1)..];
        var sha256Part = apihash.Split('/')[2];

        // Assert
        await Assert.That(sha256Part).IsEqualTo(expectedSha256);
    }

    [Test]
    public async Task Constructor_SetsProperties()
    {
        // Arrange & Act
        var auth = new Authentication("seed1", "key1", "pin1");

        // Assert
        await Assert.That(auth.Seed).IsEqualTo("seed1");
        await Assert.That(auth.ApiKey).IsEqualTo("key1");
        await Assert.That(auth.ApiPin).IsEqualTo("pin1");
    }

    private static string ComputeSha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }
}
