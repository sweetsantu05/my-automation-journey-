using Microsoft.Playwright;

namespace WiseUltimaTests.Pages.Login
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        // Inputs
        private ILocator EmailInput =>
            _page.GetByPlaceholder("Enter your email");

        private ILocator PasswordInput =>
            _page.GetByPlaceholder("Enter your password");

        private ILocator SignInButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Sign In" });

        // Validation messages
        private ILocator EmailRequiredMessage =>
            _page.GetByText("Email is required", new() { Exact = true });

        private ILocator PasswordRequiredMessage =>
            _page.GetByText("Required", new() { Exact = true });

        private ILocator InvalidCredentialToast =>
            _page.GetByText("Invalid email or password", new() { Exact = false });

        public async Task NavigateToLoginPageAsync(string loginUrl)
        {
            await _page.GotoAsync(loginUrl);
            await EmailInput.WaitForAsync();
        }

        public async Task LoginAsync(string username, string password)
        {
            await EmailInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await SignInButton.ClickAsync();
        }

        public async Task<bool> IsEmailRequiredDisplayedAsync()
        {
            return await EmailRequiredMessage.IsVisibleAsync();
        }

        public async Task<bool> IsPasswordRequiredDisplayedAsync()
        {
            return await PasswordRequiredMessage.IsVisibleAsync();
        }

        public async Task<bool> IsInvalidCredentialToastDisplayedAsync()
        {
            await _page.WaitForSelectorAsync(
                "text=Invalid email or password",
                new PageWaitForSelectorOptions
                {
                    Timeout = 5000,
                    State = WaitForSelectorState.Visible
                });

            return await _page
                .GetByText("Invalid email or password", new() { Exact = false })
                .IsVisibleAsync();
        }

        public async Task<bool> IsSignInButtonClickableAsync()
        {
            return await _page
                .GetByRole(AriaRole.Button, new() { Name = "Sign In" })
                .IsEnabledAsync();
        }

        public async Task LoginAsMigrateAdminAsync(string username, string password)
        {
            await LoginAsync(username, password);
        }

        public async Task LoginAsSuperAdminAsync(string username, string password)
        {
            await LoginAsync(username, password);
        }

    }
}
