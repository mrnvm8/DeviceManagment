using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace DeviceManagment.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _http;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _http = http;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //this method will grab or get the token from the local storage
            //and pass the claims and create a new identity
            //and it will notifier the components if the user is authicated or not

            //get auth token/ this a Bearer token
            string authToken = await _localStorageService.GetItemAsStringAsync("authToken");
            //now get the claims identiity 
            var identity = new ClaimsIdentity();
            //create a default header 
            _http.DefaultRequestHeaders.Authorization = null;

            //now going check the authToken and pass the claims
            //and set the Authorization header to the new Token that is store in the local storage

            //=> check if we have the token from the local storage
            if (!string.IsNullOrEmpty(authToken))
            {
                //if not null or empty
                try
                {
                    //this will be our claims
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
                    _http.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", "")); //remove the quotation marks

                }
                catch
                {
                    await _localStorageService.RemoveItemAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }
            //set the user now
            var user = new ClaimsPrincipal(identity);
            //set the state now 
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }
    }
}
