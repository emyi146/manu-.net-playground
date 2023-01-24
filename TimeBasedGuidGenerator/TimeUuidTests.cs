using GuidGenerator.Cassandra;

namespace TimeBasedGuidGenerator;

public class TimeUuidTests
{
    [Fact]
    public void GenerateCode_NoDatePassed_ReturnsCode()
    {
        // Act
        var result = GenerateCode();

        // Arrange
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateCode_DatePassed_ReturnsSameHalfCode()
    {
        // Arrange
        var dateTime = new DateTimeOffset(2000, 01, 01, 22, 59, 59, TimeSpan.Zero);

        // Act
        var result1 = GenerateCode(dateTime);
        var result2 = GenerateCode(dateTime);

        // Arrange
        var expectedFirstHalf = "2bbcc180-c09f-11d3";
        Assert.NotEqual(result1, result2);
        Assert.StartsWith(expectedFirstHalf, result1);
        Assert.StartsWith(expectedFirstHalf, result2);
    }

    [Fact]
    public void GenerateCode_MinDatePassed_ReturnsCode()
    {
        // Act
        var result = GenerateCode(DateTimeOffset.MinValue);

        // Arrange
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateCode_MaxDatePassed_ReturnsCode()
    {
        // Act
        var result = GenerateCode(DateTimeOffset.MaxValue);

        // Arrange
        Assert.NotNull(result);
    }

    [Fact]
    public void GenerateCode_MultipleGenerations_SameHalfWithNoCollisions()
    {
        // Arrange
        const int oneMillion = 1_000_000;

        // Act
        var codes = ParallelEnumerable
            .Range(1, oneMillion)
            .Select(_ => GenerateCode(new DateTimeOffset(2000, 01, 01, 22, 59, 59, TimeSpan.Zero)))
            .Distinct();

        // Arrange
        string expectedFirstHalf = "2bbcc180-c09f-11d3";
        Assert.Equal(oneMillion, codes.Count());
        Assert.True(codes.All(code => code.StartsWith(expectedFirstHalf)));
    }

    [Fact]
    public void TryExtractDateTime_ValidGuid_ReturnsDateTime()
    {
        // Arrange
        var expectedDate = new DateTimeOffset(2000, 01, 01, 22, 59, 59, TimeSpan.Zero);

        // Act
        var success = TryExtractDateTime("2bbcc180-c09f-11d3-abcd-abcdabcdabcd", out var result);

        // Arrange
        Assert.True(success);
        Assert.Equal(expectedDate, result.GetValueOrDefault());
    }

    [Fact]
    public void TryExtractDateTime_EmptyGuid_ReturnsMinGregorianDate()
    {
        // Arrange
        var minGregorianDate = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);

        // Act
        var success = TryExtractDateTime(Guid.Empty.ToString(), out var result);

        // Arrange
        Assert.True(success);
        Assert.Equal(minGregorianDate, result);
    }

    [Fact]
    public void TryExtractDateTime_GenericGuid_ReturnsDateTime()
    {
        // Arrange

        // Act
        var success = TryExtractDateTime("abacabac-abac-abac-abac-abacabacabac", out var result);

        // Arrange
        Assert.True(success);
        Assert.NotNull(result);
    }

    [Fact]
    public void TryExtractDateTime_AfterGenerateCodeFromDateTime_ReturnsSameDateTime()
    {
        // Arrange
        var expectedDate = new DateTimeOffset(2000, 01, 01, 22, 59, 59, TimeSpan.Zero);
        var code = GenerateCode(expectedDate);

        // Act
        var success = TryExtractDateTime(code, out var result);

        // Arrange
        Assert.True(success);
        Assert.Equal(expectedDate, result.GetValueOrDefault());
    }

    [Fact]
    public void TryExtractDateTime_AfterGenerateCode_ReturnsAlmostUtcNow()
    {
        // Arrange
        var code = GenerateCode();
        var expectedDateTime = DateTimeOffset.UtcNow;

        // Act
        var success = TryExtractDateTime(code, out var result);

        // Arrange
        Assert.True(success);
        var diff = result.GetValueOrDefault() - expectedDateTime;
        Assert.True(diff.Minutes < 1);
    }

    [Fact]
    public void TryExtractDateTime_InvalidGuid_ReturnsNoSuccess()
    {
        // Arrange

        // Act
        var success = TryExtractDateTime("2bbcc180-c09f-11d3-xxxx-xxxxxxxxxxxx", out var result);

        // Arrange
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryExtractDateTime_InvalidString_ReturnsNoSuccess()
    {
        // Arrange

        // Act
        var success = TryExtractDateTime("invalidString", out var result);

        // Arrange
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryExtractDateTime_NullString_ReturnsNoSuccess()
    {
        // Act
        var success = TryExtractDateTime(null, out var result);

        // Arrange
        Assert.False(success);
        Assert.Null(result);
    }

    public static string GenerateCode() => GenerateCode(DateTimeOffset.UtcNow);

    public static string GenerateCode(DateTimeOffset datetime)
    {
        return TimeUuid.NewId(datetime.ToUniversalTime()).ToString();
    }

    /// <inheritdoc/>
    public static bool TryExtractDateTime(string? code, out DateTimeOffset? dateTime)
    {
        dateTime = null;
        if (Guid.TryParse(code, out _))
        {
            dateTime = TimeUuid.Parse(code).GetDate().UtcDateTime;
        }
        return dateTime != null;
    }
}