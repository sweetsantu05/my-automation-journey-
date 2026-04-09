using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;

namespace WiseUltimaTests.Pages.AppMetrics
{
    public class AppMetricsPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _setup;

        public AppMetricsPage(IPage page)
        {
            _page = page;
            _setup = new BasicSetup(page);
        }

        private ILocator AppMetricsMenu =>
            _page.GetByRole(AriaRole.Link, new() { Name = "App Metrics" });

        private ILocator AppMetricsHeader =>
            _page.GetByText("App Metrics Dashboard");

        private ILocator TableRows =>
        _page.Locator("table tbody tr");
    private ILocator HighestCpuCard =>
            _page.GetByText("Highest CPU App");

        private ILocator HighestMemoryCard =>
            _page.GetByText("Highest Memory App");

        private ILocator ActiveProcessesCard =>
            _page.GetByText("Active Processes");

        private ILocator AggregateCpuCard =>
            _page.GetByText("Aggregate CPU Load");


        public async Task NavigateToAppMetricsAsync()
        {
            await _setup.NavMenuToggleButton();

            await AppMetricsMenu.ClickAsync();

            await _setup.WaitForPageAsync(2);
        }

        public async Task VerifyApplicationProcessTableLoadedAsync()
        {
            await TableRows.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 20000
            });

            var firstRow = TableRows.First;
            string rowText = await firstRow.InnerTextAsync();

            if (string.IsNullOrWhiteSpace(rowText) || rowText.Contains("skeleton"))
            {
                throw new Exception("Application Process Table not loaded properly (empty/skeleton row)");
            }
        }

        public async Task VerifyAppMetricsLoadedAsync()
        {
            await Assertions.Expect(AppMetricsHeader).ToBeVisibleAsync(new() { Timeout = 15000 });
            await VerifyApplicationProcessTableLoadedAsync();
        }
    }
}