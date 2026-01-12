using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;

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
            var signUpUrl =
                WiseUltimaTests.Utils.ConfigReader
                    .Get("LoginPageUrl")
                    .Replace("Login", "Register");

            await _page.GotoAsync(
                signUpUrl,
                new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });

            await _page.GetByPlaceholder("Enter your email").WaitForAsync();
        }

        public async Task ValidateRegisterationEmptyUserName()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("empty_username");

            await _basicSetup.SignUpAsync(
                name: "",
                email: user.Username,
                password: user.Password,
                confirmPassword: user.Password
            );
        }

        public async Task ValidateRegisterationEmptyPassword()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("empty_password");

            await _basicSetup.SignUpAsync(
                name: "Test User",
                email: "test" + Guid.NewGuid() + "@ultima.com",
                password: "",
                confirmPassword: ""
            );
        }

        public async Task ValidateRegisterationValidCredentials()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.SignUpAsync(
                name: "Ultima User",
                email: Guid.NewGuid() + "@ultima.com",
                password: user.Password,
                confirmPassword: user.Password
            );
        }
    }
}
