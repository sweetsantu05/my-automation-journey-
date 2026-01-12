using Microsoft.Playwright;
using System.Text.Json;
using AppConfig = WiseUltimaTests.Utils.ConfigReader;
using WiseUltimaTests.Utils;

namespace WiseUltimaTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;

        public string AppUrl => AppConfig.Get("AppUrl");
        public string LoginPageUrl => AppConfig.Get("LoginPageUrl");

        public AppConfig.Credential defaultCredential =
            AppConfig.GetCredential("standard_user");

        public BasicSetup(IPage page)
        {
            _page = page;
        }

        public async Task NavigateToAppAsync()
        {
            await _page.GotoAsync(AppUrl);
        }

        // ✅ FINAL, UI-ALIGNED SIGN UP FLOW
        public async Task SignUpAsync(
            string name,
            string username,
            string password,
            string confirmPassword,
            bool isOrganizationEmpty = false)
        {
            // Name
            var nameInput = _page.GetByPlaceholder("Enter your Name");
            await nameInput.WaitForAsync();
            await nameInput.FillAsync(name);

            // Email
            var emailInput = _page.GetByPlaceholder("Enter your email");
            await emailInput.WaitForAsync();
            await emailInput.FillAsync(username);

            // Organization (MANDATORY)
            if (!isOrganizationEmpty)
            {
                var organizationDropdown =
                    _page.GetByText("Select your organization", new() { Exact = true });

                await SelectRandomFromSelectAsync(organizationDropdown);
            }

            // Password & Confirm Password
            var passwordInputs = _page.Locator("input[type='password']");
            await passwordInputs.First.WaitForAsync();
            await passwordInputs.First.FillAsync(password);
            await passwordInputs.Last.FillAsync(confirmPassword);

            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task LoginAsync()
        {
            await LoginAsync(defaultCredential.Username, defaultCredential.Password);
        }

        public async Task LoginAsync(string username, string password)
        {
            var emailInput = _page.GetByPlaceholder("Enter your email");
            await emailInput.WaitForAsync();
            await emailInput.FillAsync(username);

            var passwordInput = _page.GetByPlaceholder("Enter your password");
            await passwordInput.WaitForAsync();
            await passwordInput.FillAsync(password);

            await _page.ClickAsync("button[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        // ✅ CUSTOM DROPDOWN HANDLING (Wise Ultima)
        public async Task SelectRandomFromSelectAsync(ILocator dropdown)
        {
            // Open dropdown
            await dropdown.ClickAsync();

            // Wait for options to appear
            var options = _page.Locator("role=option");
            await options.First.WaitForAsync();

            // Prefer WiseWork / Wise if available
            var wiseOption = options.Filter(new() { HasText = "Wise" });

            if (await wiseOption.CountAsync() > 0)
            {
                await wiseOption.First.ClickAsync();
                Logger.Info("Selected organization: Wise");
                return;
            }

            // Fallback: select first available option
            await options.First.ClickAsync();
            Logger.Info("Selected first available organization");
        }

        public async Task WaitForPageAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }
    }
}
