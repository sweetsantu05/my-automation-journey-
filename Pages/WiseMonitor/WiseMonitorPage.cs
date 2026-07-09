using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseMonitor
{
    public class WiseMonitorPage : BasicSetup
    {
        public WiseMonitorPage(IPage page) : base(page) { }
        private ILocator WiseMonitoryCard =>
            Page.GetByRole(AriaRole.Link, new() { Name = "Wise Monitory" });

        protected ILocator HostChip =>Page.GetByText("Host: wisemaestro-rdb-server", new() { Exact = false });

        public async Task OpenAsync()
        {
            await NavMenuToggleButton();
            await WiseMonitoryCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-monitory"));
            await WaitForDashboardStableAsync();
        }

        public async Task Verifyserver()
        {
        await HostChip.WaitForAsync(new()
        {
            State = WaitForSelectorState.Visible,
            Timeout = 50000
        });

        await Assertions.Expect(HostChip).ToBeVisibleAsync();
        }

        private ILocator TimelineDropdown =>
            Page.GetByText("1H");
            
        public async Task SelectTimelineAsync(string timeline)
        {
            await TimelineDropdown.ClickAsync();

            await Page
            .Locator(".mud-list")
            .GetByText("1H", new() { Exact = true })
            .ClickAsync();

            await WaitForDashboardStableAsync();
        }
        public async Task<string> GetCpuUtilizationAsync()
        {
            return (await Page
                .GetByRole(AriaRole.Heading, new() { Name = "CPU Utilization" })
                .Locator("..")
                .Locator("h4")
                .InnerTextAsync()).Trim();
        }

        public async Task<string> GetDiskUtilizationAsync()
        {
            return (await Page
                .GetByRole(AriaRole.Heading, new() { Name = "Disk Utilization" })
                .Locator("..")
                .Locator("h4")
                .InnerTextAsync()).Trim();
        }
    }
}
