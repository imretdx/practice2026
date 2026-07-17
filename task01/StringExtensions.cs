using System;
using System.Text;

namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (input == null)
        {
            return false;
        }

        // Очищаем строку в классическом стиле через StringBuilder (понятнее для студента)
        StringBuilder cleanBuilder = new StringBuilder();
        string lowerInput = input.ToLower();

        for (int i = 0; i < lowerInput.Length; i++)
        {
            char c = lowerInput[i];
            if (!char.IsPunctuation(c) && !char.IsWhiteSpace(c))
            {
                cleanBuilder.Append(c);
            }
        }

        string cleanStr = cleanBuilder.ToString();
        if (cleanStr.Length == 0)
        {
            return false;
        }

        // Классическая проверка палиндрома с двух концов (два указателя)
        int left = 0;
        int right = cleanStr.Length - 1;

        while (left < right)
        {
            if (cleanStr[left] != cleanStr[right])
            {
                return false;
            }
            left++;
            right--;
        }

        return true;
    }
}
