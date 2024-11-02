using Front.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Front.Services
{
    public class CustomAuthStateProvider :
    AuthenticationStateProvider, IDisposable
    {
        private readonly UserService _blazorSchoolUserService;

        public CustomAuthStateProvider(UserService blazorSchoolUserService)
        {
            _blazorSchoolUserService = blazorSchoolUserService;
            AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
        }

        public User? CurrentUser { get; private set; }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _blazorSchoolUserService.SendAuthenticateRequestAsync(email, password);

            var principal = user.ToClaimsPrincipal();
            CurrentUser = user;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();
            var user = await _blazorSchoolUserService.FetchUserFromBrowser();

            if (user is not null)
            {
                principal = user.ToClaimsPrincipal();
                CurrentUser = user;
            }


            return new(principal);
        }


        public async void Logout()
        {
            CurrentUser = null;
            await _blazorSchoolUserService.ClearBrowserUserData();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }

        private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
        {
            var authenticationState = await task;

            if (authenticationState is not null)
            {
                try
                {
                    CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
                }
                catch (ArgumentException)
                {
                    CurrentUser = null;
                }
            }
        }

        public void Dispose()
        {
            AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
            GC.SuppressFinalize(this);
        }
    }
}
