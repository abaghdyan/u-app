using System.Text.RegularExpressions;

namespace VistaLOS.Application.Services.Extensions;

public static class StringExtensions
{
    public static bool IsValidPassword(this string password)
    {
        if (string.IsNullOrEmpty(password)) {
            return false;
        }

        password = password.Trim();

        if (password.Length < 8 || password.Length > 20) {
            return false;
        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        var isValidated = hasNumber.IsMatch(password) &&
                          hasUpperChar.IsMatch(password) &&
                          hasSymbols.IsMatch(password);
        return isValidated;
    }

    public static bool IsValidPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) {
            return false;
        }
        var isNumber = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");

        var isValidated = isNumber.IsMatch(phoneNumber);
        return isValidated;
    }

    public static bool IsValidUserName(this string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname)) {
            return false;
        }

        if (nickname.Length < 3 && nickname.Length > 13) {
            return false;
        }

        var hasNumber = new Regex(@"[0-9]+");
        var hasSymbols = new Regex(@"[!@#$%^&*() _+=\[{\]};:<>|./?,-]");

        var hasNoNumber = !hasNumber.IsMatch(nickname);
        var hasNoSymbol = !hasSymbols.IsMatch(nickname);

        var isValidated = hasNoNumber && hasNoSymbol;

        return isValidated;
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email)) {
            return false;
        }
        try {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException) {
            return false;
        }
        catch (ArgumentException) {
            return false;
        }
    }
}
