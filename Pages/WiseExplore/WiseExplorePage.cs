using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseExplore
{
    public class WiseExplorePage : BasicSetup
    {
        public WiseExplorePage(IPage page) : base(page) { }
        private ILocator WiseExplorecard => Page.GetByText("Wise Explore", new() {Exact=true});

        private ILocator SystemNameHeader =>Page.GetByText("System Name", new() { Exact = true });
        private ILocator DescriptionHeader =>Page.GetByText("Description", new() { Exact = true });
        private ILocator IssuesHeader => Page.GetByText("Issues", new() { Exact = true });
        private ILocator FirstSystemRow =>Page.Locator("table tbody tr").First;

        public async Task OpenAsync()
        {
            await WiseExplorecard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-explore"));
            await WaitForDashboardStableAsync();
        }
        // public async Task VerifyExploreTableHeadersAsync()
        // {
        //     await Assertions.Expect(SystemNameHeader).ToBeVisibleAsync();
        //     await Assertions.Expect(DescriptionHeader).ToBeVisibleAsync();
        //     await Assertions.Expect(IssuesHeader).ToBeVisibleAsync();
        // }
        public async Task VerifyAtLeastOneResultAsync()
        {
            await Assertions.Expect(FirstSystemRow).ToBeVisibleAsync(new() { Timeout = 25000 });
        }
    }
}
