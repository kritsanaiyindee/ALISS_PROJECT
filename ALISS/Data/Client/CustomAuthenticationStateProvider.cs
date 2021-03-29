using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ALISS.Data.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        //private ISessionStorageService _sessionStorageService;
        //public CustomAuthenticationStateProvider()
        //{
            //_sessionStorageService = sessionStorageService;
        //}

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "admin"),
            }, "Fake authentication type");

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));

            //throw new NotImplementedException();
        }

        //public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    //var userName = await _sessionStorageService.GetItemAsync<string>("userName");
        //    ClaimsIdentity identity;

        //    //if (userName != null)
        //    //{
        //    //    identity = new ClaimsIdentity(new List<Claim>
        //    //    {
        //    //        new Claim(ClaimTypes.Name, userName)
        //    //    }, "apiauth_type");
        //    //}
        //    //else
        //    //{
        //    //    identity = new ClaimsIdentity();
        //    //}

        //    identity = new ClaimsIdentity(new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, "Admin")
        //    }, "apiauth_type");

        //    var user = new ClaimsPrincipal(identity);

        //    //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));


        //    //var user = new ClaimsPrincipal(identity);

        //    return await Task.FromResult(new AuthenticationState(user));
        //}

        public async Task<AuthenticationState> GetAuthenticationStateAsync123()
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, "admin"),
        }, "Fake authentication type");

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string userName)
        {
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            //_sessionStorageService.RemoveItemAsync("userName");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
