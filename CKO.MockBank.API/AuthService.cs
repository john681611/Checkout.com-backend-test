using System.Net;

namespace CKO.MockBank.API.Auth;

// This is a dumb mock service for auth ovo it would do a API call to a propper auth service to validate
public static class AuthService {
    public static bool Authenticate(IHeaderDictionary headers)
    {
       if(!headers.TryGetValue("Authorization", out var headerValue)) return false;
       Console.WriteLine(headerValue);
       return headerValue == "Bearer 4321";
    }
}