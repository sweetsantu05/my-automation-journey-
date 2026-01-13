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

            await _page.GotoAsync(signUpUrl);
            await _page.WaitForSelectorAsync("input[placeholder='Enter your email']");
        }

        public async Task VerifySignUpButtonIsClickableAsync()
        {
            await _page.GetByRole(
                AriaRole.Button,
                new() { Name = "Sign Up" }
            ).IsEnabledAsync();
        }

        public async Task ValidateRegisterationEmptyUserName()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

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
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.SignUpAsync(
                name: "Test Account",
                email: user.Username,
                password: "",
                confirmPassword: ""
            );
        }

        public async Task ValidateRegisterationValidCredentials()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            var uniqueEmail =
                $"autouser_{DateTime.UtcNow.Ticks}@wisework.in";

            await _basicSetup.SignUpAsync(
                name: "Test Account",
                email: uniqueEmail,
                password: user.Password,
                confirmPassword: user.Password
            );
        }

        public async Task ValidateRegisterationDuplicateEmail()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.SignUpAsync(
                name: "Test Account",
                email: user.Username,
                password: user.Password,
                confirmPassword: user.Password
            );
        }

        public async Task SignUpWithMismatchingPasswordsAsync()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.SignUpAsync(
                name: "Test Account",
                email: user.Username,
                password: user.Password,
                confirmPassword: user.Password + "1"
            );
        }

        public async Task SignUpWithEmptyOrganizationAsync()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("standard_user");

            await _basicSetup.SignUpAsync(
                name: "Test Account",
                email: user.Username,
                password: user.Password,
                confirmPassword: user.Password,
                isOrganizationEmpty: true
            );
        }
    }
}
