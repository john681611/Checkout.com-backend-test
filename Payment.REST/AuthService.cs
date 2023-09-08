using System.Net;

namespace Payment.REST.Auth;

// This is a dumb mock service for auth ovo it would do a API call to a propper auth service to validate
public static class AuthService {
    public static bool Authenticate(IHeaderDictionary headers)
    {
       if(!headers.TryGetValue("Authorization", out var headerValue)) return false;
       return headerValue == "Bearer 1234";
    }
}