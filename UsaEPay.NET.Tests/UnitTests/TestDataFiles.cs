namespace UsaEPay.NET.Tests.UnitTests;

/// <summary>
/// Loads the JSON fixtures under <c>TestData</c>, which the test csproj copies to the output directory.
/// </summary>
internal static class TestDataFiles
{
    public const string ApprovedWithBin = "sandbox-get-transaction-return-bin.json";
    public const string DeclinedWithBin = "sandbox-get-declined-return-bin.json";
    public const string ApprovedWithoutBin = "sandbox-get-transaction-no-flag.json";

    public static string Read(string fileName)
    {
        return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "TestData", fileName));
    }
}
