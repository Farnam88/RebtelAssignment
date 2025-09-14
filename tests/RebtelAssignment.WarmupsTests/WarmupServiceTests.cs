namespace RebtelAssignment.WarmupsTests;

public class WarmupServiceTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(10, false)]
    [InlineData(4, true)]
    [InlineData(14, false)]
    [InlineData(16, true)]
    public void PowerOfTwo_ReturnsTrueIfItIs_AndFalseIfItIsNot(int number, bool expected)
    {
        //Act
        var actualResult = WarmupService.IsPowerOfTwo(number);

        //Assert
        Assert.Equal(expected, actualResult);
    }

    [Theory]
    [InlineData("This is a test", "tset a si sihT")]
    [InlineData("congratulations", "snoitalutargnoc")]
    [InlineData("   test", "tset   ")]
    [InlineData("Moby Dick", "kciD yboM")]
    public void ReverseString_ShouldReturn_ReversedString(string title, string reversedTitle)
    {
        //Act
        var actualResult = WarmupService.ReversedString(title);

        //Assert
        Assert.Equal(reversedTitle, actualResult);
    }

    [Theory]
    [InlineData("Test", "TestTest", 2)]
    [InlineData("One Repeat", "One Repeat", 1)]
    [InlineData(" Stockholm ", "", 0)]
    [InlineData("Read", "ReadReadRead", 3)]
    [InlineData(" Sweden", " Sweden Sweden Sweden Sweden Sweden", 5)]
    public void StringRepeat_ShouldReturn_RepeatedString(string str, string repeatedStr, int repetitions)
    {
        //Act
        var actualResult = WarmupService.RepeatString(str, repetitions);

        //Assert
        Assert.Equal(repeatedStr, actualResult);
    }

    [Theory]
    [MemberData(nameof(OddNumberTestInput))]
    public void GetOddNumbersList_ShouldReturn_AListOfOddNumbersFromTheInputArray(int[] input,
        List<int> expectedOddNumbersList)
    {
        //Act
        var actualResult = WarmupService.GetOddNumbersList(input);

        //Assert
        Assert.Equal(expectedOddNumbersList, actualResult);
    }

    public static IEnumerable<object[]> OddNumberTestInput => new List<object[]>
    {
        new object[]
        {
            new int[] { 0, 1, 5, 2, 6, 10, 11, 29, 39 },
            new List<int>() { 1, 5, 11, 29, 39 }
        },
        new object[]
        {
            new int[] { 1234, 233, 334567, 1233465 },
            new List<int>() { 233, 334567, 1233465 }
        },
        new object[]
        {
            new int[] { 2, 4, 6, 8 },
            new List<int>()
        },
        new object[]
        {
            new int[] { },
            new List<int>()
        },
        new object[]
        {
            null,
            new List<int>()
        }
    };
}