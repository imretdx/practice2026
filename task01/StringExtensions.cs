using System;
using System.Linq;

namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (input == null) return false;
        var cleanChars = input.ToLower().Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).ToArray();
        if (cleanChars.Length == 0) return false;
        return cleanChars.SequenceEqual(cleanChars.Reverse());
    }
}
