using System.Text.Json;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Classes;
using UsaEPay.NET.Models.Enumerations;

namespace UsaEPay.NET.Tests.UnitTests;

/// <summary>
/// The <c>bin</c> object comes back only from a transaction lookup with <c>return_bin=true</c>.
/// These tests pin the shapes seen on the wire: <c>bin.bin</c> was JSON null on the sandbox (the
/// documented string form and numbers must also work), issuer and data_source can be null,
/// category can be empty, and <c>type</c> is the funding type with inconsistent casing.
/// </summary>
public sealed class BinDeserializationTests
{
    private static UsaEPayResponse? Deserialize(string json) =>
        JsonSerializer.Deserialize<UsaEPayResponse>(json, UsaEPaySerializerContext.Default.Options);

    private static Bin? DeserializeBin(string binJson) =>
        Deserialize($$"""{"type":"transaction","key":"k1","bin":{{binJson}}}""")?.Bin;

    #region Real sandbox fixtures

    [Test]
    public async Task Fixture_ApprovedWithBin_DeserializesBinAndFundingType()
    {
        var response = Deserialize(TestDataFiles.Read(TestDataFiles.ApprovedWithBin));

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Bin).IsNotNull();
        await Assert.That(response.Bin!.CardBin).IsNull();
        await Assert.That(response.Bin.Number).IsEqualTo("4000xxxxxxxx2224");
        await Assert.That(response.Bin.Type).IsEqualTo("credit");
        await Assert.That(response.Bin.FundingType).IsEqualTo(CardFundingType.Credit);
        await Assert.That(response.Bin.Issuer).IsNull();
        await Assert.That(response.Bin.Bank).IsNull();
        await Assert.That(response.Bin.Country).IsEqualTo("US");
        await Assert.That(response.Bin.CountryName).IsEqualTo("UNITED STATES");
        await Assert.That(response.Bin.CountryIso).IsEqualTo("840");
        await Assert.That(response.Bin.Category).IsEqualTo(string.Empty);
        await Assert.That(response.Bin.DataSource).IsEqualTo("merch");
    }

    [Test]
    public async Task Fixture_DeclinedWithBin_DeserializesBinAndFundingType()
    {
        var response = Deserialize(TestDataFiles.Read(TestDataFiles.DeclinedWithBin));

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("D");
        await Assert.That(response.Bin).IsNotNull();
        await Assert.That(response.Bin!.CardBin).IsNull();
        await Assert.That(response.Bin.Type).IsEqualTo("CREDIT");
        await Assert.That(response.Bin.FundingType).IsEqualTo(CardFundingType.Credit);
        await Assert.That(response.Bin.Issuer).IsNull();
        await Assert.That(response.Bin.DataSource).IsNull();
    }

    [Test]
    public async Task Fixture_WithoutReturnBinFlag_HasNoBin()
    {
        var response = Deserialize(TestDataFiles.Read(TestDataFiles.ApprovedWithoutBin));

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Bin).IsNull();
    }

    #endregion

    #region bin.bin shapes

    [Test]
    [Arguments("\"444455\"", "444455")]
    [Arguments("444455", "444455")]
    [Arguments("\"012345\"", "012345")]
    [Arguments("\" 444455 \"", "444455")]
    [Arguments("null", null)]
    [Arguments("\"\"", null)]
    [Arguments("\"   \"", null)]
    [Arguments("true", null)]
    [Arguments("false", null)]
    [Arguments("[444455]", null)]
    [Arguments("{\"value\":444455}", null)]
    public async Task CardBin_AnyJsonShape_DoesNotThrow(string binValueJson, string? expected)
    {
        var bin = DeserializeBin($$"""{"bin":{{binValueJson}},"type":"debit"}""");

        await Assert.That(bin).IsNotNull();
        await Assert.That(bin!.CardBin).IsEqualTo(expected);
        // The members after bin.bin must still be read, including after a skipped object or array.
        await Assert.That(bin.Type).IsEqualTo("debit");
    }

    [Test]
    public async Task CardBin_Absent_DoesNotThrow()
    {
        var bin = DeserializeBin("""{"type":"debit"}""");

        await Assert.That(bin).IsNotNull();
        await Assert.That(bin!.CardBin).IsNull();
        await Assert.That(bin.FundingType).IsEqualTo(CardFundingType.Debit);
    }

    [Test]
    public async Task CardBin_Serializes_AsString()
    {
        var json = JsonSerializer.Serialize(new Bin { CardBin = "012345" }, UsaEPaySerializerContext.Default.Options);

        await Assert.That(json).Contains("\"bin\":\"012345\"");
    }

    #endregion

    #region Other members

    [Test]
    public async Task OtherStringMembers_NonStringValues_DoNotThrow()
    {
        var bin = DeserializeBin("""
            {"bin":"444455","number":4444,"type":"Prepaid","issuer":["VISA"],"bank":{"name":"x"},
             "country":"US","country_name":true,"country_iso":840,"location":null,"phone":8005551212,
             "category":"","data_source":"binbase"}
            """);

        await Assert.That(bin).IsNotNull();
        await Assert.That(bin!.CardBin).IsEqualTo("444455");
        await Assert.That(bin.Number).IsEqualTo("4444");
        await Assert.That(bin.Type).IsEqualTo("Prepaid");
        await Assert.That(bin.FundingType).IsEqualTo(CardFundingType.Prepaid);
        await Assert.That(bin.Issuer).IsNull();
        await Assert.That(bin.Bank).IsNull();
        await Assert.That(bin.Country).IsEqualTo("US");
        await Assert.That(bin.CountryName).IsNull();
        await Assert.That(bin.CountryIso).IsEqualTo("840");
        await Assert.That(bin.Location).IsNull();
        await Assert.That(bin.Phone).IsEqualTo("8005551212");
        await Assert.That(bin.Category).IsEqualTo(string.Empty);
        await Assert.That(bin.DataSource).IsEqualTo("binbase");
    }

    [Test]
    public async Task EmptyBinObject_DoesNotThrow_FundingTypeUnknown()
    {
        var bin = DeserializeBin("{}");

        await Assert.That(bin).IsNotNull();
        await Assert.That(bin!.CardBin).IsNull();
        await Assert.That(bin.FundingType).IsEqualTo(CardFundingType.Unknown);
    }

    [Test]
    public async Task FundingType_IsNotSerialized()
    {
        var json = JsonSerializer.Serialize(new Bin { Type = "debit" }, UsaEPaySerializerContext.Default.Options);

        await Assert.That(json).Contains("\"type\":\"debit\"");
        await Assert.That(json).DoesNotContain("unding");
    }

    #endregion

    #region FundingType

    [Test]
    [Arguments("DEBIT", CardFundingType.Debit)]
    [Arguments("Debit", CardFundingType.Debit)]
    [Arguments("debit", CardFundingType.Debit)]
    [Arguments("CREDIT", CardFundingType.Credit)]
    [Arguments("credit", CardFundingType.Credit)]
    [Arguments("Prepaid", CardFundingType.Prepaid)]
    [Arguments("PREPAID", CardFundingType.Prepaid)]
    [Arguments(" debit ", CardFundingType.Debit)]
    [Arguments("Visa", CardFundingType.Unknown)]
    [Arguments("credit card", CardFundingType.Unknown)]
    [Arguments("", CardFundingType.Unknown)]
    [Arguments("   ", CardFundingType.Unknown)]
    [Arguments(null, CardFundingType.Unknown)]
    public async Task FundingType_MapsTypeCaseInsensitively(string? type, CardFundingType expected)
    {
        var bin = new Bin { Type = type };

        await Assert.That(bin.FundingType).IsEqualTo(expected);
    }

    [Test]
    [Arguments("\"DEBIT\"", CardFundingType.Debit)]
    [Arguments("\"Debit\"", CardFundingType.Debit)]
    [Arguments("\"CREDIT\"", CardFundingType.Credit)]
    [Arguments("\"Prepaid\"", CardFundingType.Prepaid)]
    [Arguments("\"Visa\"", CardFundingType.Unknown)]
    [Arguments("\"\"", CardFundingType.Unknown)]
    [Arguments("null", CardFundingType.Unknown)]
    [Arguments("12", CardFundingType.Unknown)]
    public async Task FundingType_FromJson_MapsType(string typeJson, CardFundingType expected)
    {
        var bin = DeserializeBin($$"""{"bin":null,"type":{{typeJson}}}""");

        await Assert.That(bin).IsNotNull();
        await Assert.That(bin!.FundingType).IsEqualTo(expected);
    }

    #endregion
}
