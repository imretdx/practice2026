using Xunit;
using task01;

namespace task01tests;

public class StringExtensionsTests
{
    [Fact] public void IsPalindrome_ValidPalindrome_ReturnsTrue() => Assert.True("А роза упала на лапу Азора".IsPalindrome());
    [Fact] public void IsPalindrome_NotPalindrome_ReturnsFalse() => Assert.False("Hello, world!".IsPalindrome());
    [Fact] public void IsPalindrome_EmptyString_ReturnsFalse() => Assert.False("".IsPalindrome());
    [Fact] public void IsPalindrome_WithPunctuation_IgnoresPunctuation() => Assert.True("Was it a car or a cat I saw?".IsPalindrome());
}
