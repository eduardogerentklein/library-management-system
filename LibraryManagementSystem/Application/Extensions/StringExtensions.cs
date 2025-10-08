namespace Application.Extensions;

public static class StringExtensions
{
    public static bool IsValidIsbn13(this string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        // Remove any hyphens
        isbn = isbn.Replace("-", "").Trim();

        if (isbn.Length != 13 || !isbn.All(char.IsDigit))
            return false;

        // Compute ISBN-13 checksum
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = isbn[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int checksum = (10 - (sum % 10)) % 10;
        int lastDigit = isbn[12] - '0';

        return checksum == lastDigit;
    }
}
