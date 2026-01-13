using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;

namespace WiseUltimaTests.Pages.Login
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        public LoginPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
        }

        public async Task NavigateToLoginPageAsync()
        {
            await _page.GotoAsync(
                WiseUltimaTests.Utils.ConfigReader.Get("LoginPageUrl"),
                new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

            await _page.GetByPlaceholder("Enter your email").WaitForAsync();
        }

        public async Task ValidateEmptyUserName()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("empty_username");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateEmptyPassword()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("empty_password");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateInvalidLogin()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("invalid");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateValidLogin()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateSuperAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();

            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("superadmin");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }

        public async Task ValidateUltimaAdminAccount(IPage page)
        {
            await NavigateToLoginPageAsync();

            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("ultimaadmin");

            await _basicSetup.LoginAsync(user.Username, user.Password);
        }
    }
}
