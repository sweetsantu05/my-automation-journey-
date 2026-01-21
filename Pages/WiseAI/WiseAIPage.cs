using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseAI
{
    public class WiseAIPage : BasicSetup
    {
        public WiseAIPage(IPage page) : base(page) { }

        private ILocator WiseAICard =>
            Page.GetByText("Wise AI", new() { Exact = true });

        private ILocator App1Tab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "App 1" });

        private ILocator App2Tab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "App 2" });

        private ILocator SearchButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Search" });

        private ILocator PopularQueriesHeader =>
            Page.GetByText("Popular Queries", new() { Exact = true });

        public async Task OpenAsync()
        {
            await WiseAICard.ClickAsync();
            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex(".*/wise-ai"));
        }

        public async Task VerifyAppTabsAsync()
        {
            await Assertions.Expect(App1Tab).ToBeVisibleAsync();
            await Assertions.Expect(App2Tab).ToBeVisibleAsync();
        }

        public async Task VerifySearchAsync()
        {
            await Assertions.Expect(SearchButton).ToBeVisibleAsync();
        }

        public async Task VerifyPopularQueriesAsync()
        {
            await Assertions.Expect(PopularQueriesHeader).ToBeVisibleAsync();
        }
    }
}
