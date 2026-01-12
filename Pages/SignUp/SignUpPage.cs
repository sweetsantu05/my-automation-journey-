using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using AppConfig = WiseUltimaTests.Utils.ConfigReader;

namespace WiseUltimaTests.Pages.SignUp
{
    public class SignUpPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        public SignUpPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
        }

        public async Task NavigateToSignUpPageAsync()
        {
            await _page.GotoAsync(
                _basicSetup.LoginPageUrl.Replace("Login", "Register"),
                new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }
            );

            await _page.GetByPlaceholder("Enter your email").WaitForAsync();
        }

        public async Task ValidateEmptyUserName()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("empty_username");

            await _basicSetup.SignUpAsync(
                user.Role,
                user.Username,
                user.Password,
                user.Password
            );
        }

        public async Task ValidateEmptyPassword()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("empty_password");
            var email = Guid.NewGuid().ToString("N").Substring(0, 10) + "@ultima.com";

            await _basicSetup.SignUpAsync(
                user.Role,
                email,
                user.Password,
                user.Password
            );
        }

        public async Task ValidateDuplicateEmail()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("standard_user");
            var email = "duplicateuser@ultima.com";

            for (int i = 0; i < 2; i++)
            {
                await _basicSetup.SignUpAsync(
                    user.Role,
                    email,
                    user.Password,
                    user.Password
                );
            }
        }

        public async Task ValidateMismatchingPassword()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("invalid");

            await _basicSetup.SignUpAsync(
                user.Role,
                user.Username,
                user.Password,
                Guid.NewGuid().ToString("N").Substring(0, 8)
            );
        }

        public async Task ValidateEmptyOrganization()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("invalid");

            await _basicSetup.SignUpAsync(
                user.Role,
                user.Username,
                user.Password,
                user.Password,
                isOrganizationEmpty: true
            );
        }

        public async Task ValidateValidRegistration()
        {
            await NavigateToSignUpPageAsync();

            var user = AppConfig.GetCredential("standard_user");
            var email = Guid.NewGuid().ToString("N").Substring(0, 10) + "@ultima.com";

            await _basicSetup.SignUpAsync(
                user.Role,
                email,
                user.Password,
                user.Password
            );
        }
    }
}
