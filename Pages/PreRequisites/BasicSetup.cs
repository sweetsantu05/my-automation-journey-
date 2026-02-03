using Allure.Xunit.Attributes.Steps;
using Microsoft.Playwright;
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

        /* ---------------- LOGIN ---------------- */

        public async Task LoginAsync(string username, string password)
        {
            await Page.GetByPlaceholder("Enter your email").FillAsync(username);
            await Page.GetByPlaceholder("Enter your password").FillAsync(password);

            await Page.GetByRole(
                AriaRole.Button,
                new() { Name = "Sign In" }
            ).ClickAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task LoginIfNeededAsync(string username, string password)
        {
            var emailField = Page.GetByPlaceholder("Enter your email");

            if (await emailField.IsVisibleAsync())
            {
                await LoginAsync(username, password);
            }
        }


        /* ---------------- SIGN UP ---------------- */

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

        /* ---------------- DASHBOARD COMMON ---------------- */

        // protected ILocator CurrentTab =>Page.GetByRole(AriaRole.Button, new() { Name = "Current" });
        // protected ILocator WPredictTab =>Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" });
        // protected ILocator MPredictTab =>Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" });

        // protected ILocator RegionDropdown =>Page.GetByText("Region", new() { Exact = false });
        // protected ILocator ApplicationDropdown =>Page.GetByText("Application", new() { Exact = false }).First;
        // private ILocator ServerCard => Page.GetByText("Server", new() { Exact = true }).First;
        // private ILocator StorageCard => Page.GetByText("Storage", new() { Exact = true });
        // private ILocator DatabaseCard => Page.GetByText("Database", new() { Exact = true });
        // private ILocator NetworkCard => Page.GetByText("Network", new() { Exact = true });
        // private ILocator MiddlewareCard => Page.GetByText("Middleware", new() { Exact = true });
        // private ILocator BackupCard => Page.GetByText("Backup", new() { Exact = true });

        // ---------------- DASHBOARD COMMON FLOW ----------------

        protected ILocator CurrentTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Current" });

        protected ILocator WPredictTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" });

        protected ILocator MPredictTab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" });

        protected ILocator ApplicationDropdown =>
            Page.GetByText("Application", new() { Exact = false });

        protected ILocator ApplicationOptions =>
            Page.GetByRole(AriaRole.Textbox, new(){Name="Critical app"});
        
        protected ILocator CriticalApp1 =>
            Page.GetByText("Critical App 1", new() { Exact = true }).First;
        protected ILocator CriticalApp2 =>
            Page.GetByText("Critical App 2", new() { Exact = true }).First;

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

        public async Task SelectRandomCriticalApplicationAsync()
        {
            await ApplicationOptions.ClickAsync();

            
        }
        public async Task ClickRandomCriticalAppAsync()
        {
            await ApplicationOptions.ClickAsync();
            var apps = new[]
            {
                Page.GetByText("Critical App 1", new() { Exact = true }).First,
                Page.GetByText("Critical App 2", new() { Exact = true }).First
            };

            await apps[Random.Shared.Next(apps.Length)].ClickAsync();
        }

        public async Task VerifyServerLoadedAsync()
        {
            await Assertions.Expect(ServerCard)
                .ToBeVisibleAsync(new() { Timeout = 20000 });
        }

        // public async Task VerifyDashboardTabsAsync()
        // {
        //     await Assertions.Expect(CurrentTab).ToBeVisibleAsync();
        //     await Assertions.Expect(WPredictTab).ToBeVisibleAsync();
        //     await Assertions.Expect(MPredictTab).ToBeVisibleAsync();
        // }

        // public async Task VerifyDashboardFiltersAsync()
        // {
        //     await Assertions.Expect(RegionDropdown).ToBeVisibleAsync();
        //     await Assertions.Expect(ApplicationDropdown).ToBeVisibleAsync();
        // }

        public async Task WaitForWiseCardsToLoadAsync()
        {
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Page.GetByText("Server", new() { Exact = true })
                .First
                .WaitForAsync(new LocatorWaitForOptions
                {
                    Timeout = 40000
                });
        }

        public async Task VerifyWiseCardsAsync()
        {
            await Assertions.Expect(ServerCard).ToBeVisibleAsync();
            // await Assertions.Expect(StorageCard).ToBeVisibleAsync();
            // await Assertions.Expect(DatabaseCard).ToBeVisibleAsync();
            // await Assertions.Expect(NetworkCard).ToBeVisibleAsync();
            // await Assertions.Expect(MiddlewareCard).ToBeVisibleAsync();
            // await Assertions.Expect(BackupCard).ToBeVisibleAsync();
        }

        public async Task WaitForPageAsync(int seconds)
        {
            await Task.Delay(seconds * 1000);
        }
    }
}
