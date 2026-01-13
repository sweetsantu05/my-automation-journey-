using Microsoft.Playwright;

namespace WiseUltimaTests.Pages.Dashboard
{
    public class ProjectDashboardPage
    {
        private readonly IPage _page;

        public ProjectDashboardPage(IPage page)
        {
            _page = page;
        }

        // Page Identifiers
        private ILocator WelcomeUserText =>
            _page.GetByText("Welcome", new() { Exact = false });

        private ILocator WiseBoardCard =>
            _page.GetByText("Wise Board", new() { Exact = true });

        private ILocator WiseActionsCard =>
            _page.GetByText("Wise Actions", new() { Exact = true });

        private ILocator WiseAICard =>
            _page.GetByText("Wise AI", new() { Exact = true });

        private ILocator WiseExploreCard =>
            _page.GetByText("Wise Explore", new() { Exact = true });

        private ILocator WiseAgentsCard =>
            _page.GetByText("Wise Agents", new() { Exact = true });

        public async Task WaitForDashboardToLoadAsync()
        {
            await WelcomeUserText.WaitForAsync(
                new LocatorWaitForOptions
                {
                    Timeout = 15000
                });
        }

        public async Task VerifyDashboardCardsAsync()
        {
            await WiseBoardCard.WaitForAsync();
            await WiseActionsCard.WaitForAsync();
            await WiseAICard.WaitForAsync();
            await WiseExploreCard.WaitForAsync();
            await WiseAgentsCard.WaitForAsync();
        }
    }
}
