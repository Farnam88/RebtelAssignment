using System.Text;

namespace RebtelAssignment.WarmupsTests;

internal class WarmupService
{
    /// <summary>
    /// Check if the number is a power of two
    /// assuming we are covering only int32
    /// </summary>
    /// <returns></returns>
    public static bool IsPowerOfTwo(int num)
    {
        if (num <= 0)
        {
            return false;
        }

        return (num & (num - 1)) == 0;
    }

    public static string ReversedString(string title)
    {
        char[] charArray = title.ToCharArray();
        int left = 0;
        int right = charArray.Length - 1;

        while (left < right)
        {
            // Swap characters
            (charArray[left], charArray[right]) = (charArray[right], charArray[left]);

            // Move pointers
            left++;
            right--;
        }

        return new string(charArray);
    }

    public static string RepeatString(string str, int repetitions)
    {
        if (repetitions < 1)
            return string.Empty;
        StringBuilder builder = new();
        for (int i = 0; i < repetitions; i++)
        {
            builder.Append(str);
        }

        return builder.ToString();
    }

    public static List<int> GetOddNumbersList(int[]? input)
    {
        List<int> oddNumbers = new();
        if (input == null || input.Length == 0)
            return oddNumbers;
        
        for (int i = 0; i < input.Length; i++)
            if (input[i] % 2 != 0) // odd when divided by 2 is not zero
                oddNumbers.Add(input[i]);
        return oddNumbers;
    }
}