using Microsoft.Playwright;

namespace WiseUltimaTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        private readonly IPage _page;

        public BasicSetup(IPage page)
        {
            _page = page;
        }

        public async Task LoginAsync(string username, string password)
        {
            await _page.GetByPlaceholder("Enter your email").FillAsync(username);
            await _page.GetByPlaceholder("Enter your password").FillAsync(password);

            await _page.GetByRole(
                    AriaRole.Button,
                    new() { Name = "Sign In" }
                )
                .ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SignUpAsync(
            string name,
            string email,
            string password,
            string confirmPassword,
            bool isOrganizationEmpty = false)
        {
            await _page.GetByPlaceholder("Enter your Name").FillAsync(name);
            await _page.GetByPlaceholder("Enter your email").FillAsync(email);

            if (!isOrganizationEmpty)
            {
                await _page.GetByPlaceholder("Select your organization").ClickAsync();

                var popover = _page.Locator(".mud-popover-open");
                var orgOption = popover.GetByText("WiseWork", new() { Exact = true });

                await orgOption.WaitForAsync();
                await orgOption.ClickAsync();
            }

            await _page.GetByPlaceholder("Enter your password").FillAsync(password);
            await _page.GetByPlaceholder("Confirm your password").FillAsync(confirmPassword);

            await _page.GetByRole(
                    AriaRole.Button,
                    new() { Name = "Sign Up" }
                )
                .ClickAsync();

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task WaitForPageAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }
    }
}
