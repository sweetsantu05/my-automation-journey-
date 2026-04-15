using Allure.Xunit.Attributes.Steps;
using Microsoft.Playwright;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

using System.Text.RegularExpressions;


namespace WiseUltimaTests.Pages.PreRequisites
{
    public class BasicSetup
    {
        protected readonly IPage Page;

        public BasicSetup(IPage page)
        {
            Page = page;
        }

        public async Task LoginAsync(string username, string password)
        {
            await Page.GetByPlaceholder("Enter your email").FillAsync(username);
            await Page.GetByPlaceholder("Enter your password").FillAsync(password);
            await Page.GetByRole(AriaRole.Button,new() { Name = "Sign In" }).ClickAsync();        
        }

        public async Task SignUpAsync(
            string name,
            string email,
            string password,
            string confirmPassword,
            bool isOrganizationEmpty = false)
        {
            await Page.GetByPlaceholder("Enter your Name").FillAsync(name);
            await Page.GetByPlaceholder("Enter your email").FillAsync(email);

            if (!isOrganizationEmpty)
            {
                await Page.GetByPlaceholder("Select your organization").ClickAsync();

                var popover = Page.Locator(".mud-popover-open");
                var orgOption = popover.GetByText("WiseWork", new() { Exact = true });

                await orgOption.WaitForAsync();
                await orgOption.ClickAsync();
            }

            await Page.GetByPlaceholder("Enter your password").FillAsync(password);
            await Page.GetByPlaceholder("Confirm your password").FillAsync(confirmPassword);

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "Sign Up" }
            ).ClickAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        protected ILocator CurrentTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Current" });

        protected ILocator WPredictTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" });

        protected ILocator MPredictTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" });

        protected ILocator ApplicationOptions =>
            Page.GetByText("Critical App");

        protected ILocator ServerCard =>
            Page.GetByText("Server", new() { Exact = true }).First;

        public async Task WaitForDashboardStableAsync()
        {
            await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task SwitchToCurrentAsync()
        {
            await CurrentTab.ClickAsync();
            await WaitForDashboardStableAsync();
        }

        public async Task SwitchToWPredictAsync()
        {
            await WPredictTab.ClickAsync();
            await WaitForDashboardStableAsync();
        }

        public async Task SwitchToMPredictAsync()
        {
            await MPredictTab.ClickAsync();
            await WaitForDashboardStableAsync();
        }

        public async Task ClickRandomCriticalAppAsync()
        {
            await WaitForIconToLoadAsync(Page);
            await WaitForPageStableAsync();
            await ApplicationOptions.ClickAsync();
            var apps = new[]
            {
                Page.GetByText("Critical App 1", new() { Exact = true }).First,
                Page.GetByText("Critical App 2", new() { Exact = true }).First
            };

            await apps[Random.Shared.Next(apps.Length)].ClickAsync();
        }
       public async Task WaitForIconToLoadAsync(IPage page)
        {
            await page.WaitForFunctionAsync(@"
                () => {
                    const images = Array.from(document.images);

                    const target = images.find(img => 
                        img.src.includes('Backup.svg') || 
                        img.src.includes('Backup.png')
                    );

                    return ta  rget && target.complete && target.naturalWidth > 0;
                }
            ", new PageWaitForFunctionOptions
            {
                Timeout = 25000
            });
        }

        public async Task WaitForPageStableAsync()
        {
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Page.WaitForFunctionAsync(@"
                () => {
                    const loaders = document.querySelectorAll(
                        '.mud-progress-circular, .loading, .spinner'
                    );
                    return loaders.length === 0;
                }
            ");

            await Page.WaitForTimeoutAsync(500);
        }

        public async Task VerifyServerLoadedAsync()
        {
            await Assertions.Expect(ServerCard)
                .ToBeVisibleAsync(new() { Timeout = 20000 });
        }

        public async Task SwitchBasedOnAppAsync()
        {
            var currentTab = Page.Locator("button:has-text('CURRENT')");

            if (await currentTab.CountAsync() > 0 && await currentTab.IsVisibleAsync())
            {
                await currentTab.ClickAsync();
            }
            else
            {
                Console.WriteLine(" CURRENT not available → switching to D-PREDICT");
                await Page.Locator("button:has-text('D-PREDICT')").ClickAsync();
            }

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task WaitForPageAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }
        public async Task NavMenuToggleButton()
        {
            await Page.Locator(".mud-navmenu > div:nth-child(2)").ClickAsync();
        }
    }
}
