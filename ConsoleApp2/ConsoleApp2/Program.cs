using System;

public interface IAuthStrategy
{
    bool Authenticate(string username, string credentials);
}

public class OAuthAuth : IAuthStrategy
{
    public bool Authenticate(string username, string token)
    {
        Console.WriteLine($"Проверка OAuth токена для пользователя {username}");
        return !string.IsNullOrEmpty(token) && token.StartsWith("oauth_");
    }
}

public class JWTAuth : IAuthStrategy
{
    public bool Authenticate(string username, string token)
    {
        Console.WriteLine($"Проверка JWT токена для пользователя {username}");
        return !string.IsNullOrEmpty(token) && token.Length > 30;
    }
}

public class BasicAuth : IAuthStrategy
{
    public bool Authenticate(string username, string password)
    {
        Console.WriteLine($"Проверка Basic аутентификации для пользователя {username}");
        return !string.IsNullOrEmpty(password) && password.Length >= 8;
    }
}

public class AuthenticationService
{
    private IAuthStrategy _authStrategy;

    public AuthenticationService(IAuthStrategy authStrategy)
    {
        _authStrategy = authStrategy;
    }

    public void SetAuthStrategy(IAuthStrategy authStrategy)
    {
        _authStrategy = authStrategy;
    }

    public bool AuthenticateUser(string username, string credentials)
    {
        return _authStrategy.Authenticate(username, credentials);
    }
}

class Program
{
    static void Main(string[] args)
    {
        var authService = new AuthenticationService(new BasicAuth());

        Console.WriteLine("Тестирование BasicAuth:");
        bool result = authService.AuthenticateUser("admin", "password123");
        Console.WriteLine($"Результат аутентификации: {result}\n");

        authService.SetAuthStrategy(new OAuthAuth());
        Console.WriteLine("Тестирование OAuthAuth:");
        result = authService.AuthenticateUser("user1", "oauth_token123");
        Console.WriteLine($"Результат аутентификации: {result}\n");

        authService.SetAuthStrategy(new JWTAuth());
        Console.WriteLine("Тестирование JWTAuth:");
        result = authService.AuthenticateUser("user2", "jwt_token_very_long_string_1234567890");
        Console.WriteLine($"Результат аутентификации: {result}\n");
    }
}