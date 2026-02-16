using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;
using WiseUltimaTests.Utils;

namespace WiseUltimaTests.Pages.WiseExplore
{
    public class WiseExplorePage : BasicSetup
    {
        public WiseExplorePage(IPage page) : base(page) { }

        // ✅ More robust locators with case-insensitive matching
        private ILocator WiseExplorecard =>
            Page.GetByText("Wise Explore", new() { Exact = true }).First;

        // ✅ Case-insensitive text headers
        private ILocator SystemNameHeader =>
            Page.Locator("text=/System Name/i").First;

        private ILocator DescriptionHeader =>
            Page.Locator("text=/Description/i").First;

        private ILocator IssuesHeader =>
            Page.Locator("text=/Issues/i").First;

        private ILocator FirstSystemRow =>
            Page.Locator("table tbody tr").First;

        // ✅ FIXED: More defensive OpenAsync
        public async Task OpenAsync()
        {
            // Wait for menu card with longer timeout
            await WiseExplorecard.WaitForAsync(new() { Timeout = 30000 });
            await WiseExplorecard.ClickAsync();

            // Verify URL with flexible regex
            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex(@".*/wise-explore/?$", RegexOptions.IgnoreCase));

            await WaitForDashboardStableAsync();

            // Wait for any textbox/search input to appear
            await Page.Locator("input[role='textbox'], input:not([type='hidden'])")
                .First
                .WaitForAsync(new() { Timeout = 15000 });
        }

        // ✅ IMPROVED: Better error handling and logging
        public async Task VerifyAtLeastOneResultAsync()
        {
            try
            {
                // Wait for table first
                await Page.WaitForSelectorAsync("table", new PageWaitForSelectorOptions { Timeout = 25000 });

                // Verify first row is visible
                await Assertions.Expect(FirstSystemRow)
                    .ToBeVisibleAsync(new() { Timeout = 25000 });

                Logger.Info("✅ Wise Explore table loaded with at least one row");
            }
            catch (PlaywrightException e) when (e.Message.Contains("Timeout"))
            {
                await ScreenshotHelper.TakeScreenshotAsync(Page, "WiseExplore_NoData_Failure");
                Logger.Error($"❌ System list failed to load: {e.Message}");
                throw;
            }
        }

        // ✅ IMPROVED: More comprehensive page load verification
        public async Task VerifyWiseExplorePageLoadedAsync()
        {
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Assertions.Expect(SystemNameHeader)
                .ToBeVisibleAsync(new() { Timeout = 15000 });

            await Assertions.Expect(DescriptionHeader).ToBeVisibleAsync();
            await Assertions.Expect(IssuesHeader).ToBeVisibleAsync();

            Logger.Info("✅ Wise Explore headers verified: System Name, Description, Issues");
        }
    }
}



