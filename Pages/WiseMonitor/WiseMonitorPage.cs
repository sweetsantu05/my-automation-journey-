using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseMonitor
{
    public class WiseMonitorPage : BasicSetup
    {
        public WiseMonitorPage(IPage page) : base(page) { }

        private ILocator NavMenuToggle =>
            Page.Locator(".mud-navmenu > div:nth-child(2)");

        private ILocator WiseMonitoryNavItem =>
            Page.Locator(".mud-navmenu")
                .GetByText("Wise Monitory", new() { Exact = true });

        private ILocator MonitoringDashboardTitle =>
            Page.GetByText("Monitoring Dashboard", new() { Exact = true });

        private ILocator LiveStatus =>
            Page.GetByText("LIVE", new() { Exact = true });

        private ILocator CPUUtilizationCard =>
            Page.GetByText("CPU Utilization", new() { Exact = true });

        private ILocator MemoryUsageCard =>
            Page.GetByText("Memory Usage", new() { Exact = true });

        private ILocator DiskUtilizationCard =>
            Page.GetByText("Disk Utilization", new() { Exact = true });

        private ILocator NetworkTrafficCard =>
            Page.GetByText("Network Traffic", new() { Exact = true });

        public async Task OpenAsync()
        {
            await NavMenuToggle.ClickAsync();
            await WiseMonitoryNavItem.ClickAsync();

            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex(".*/wise-monitory"));
        }

        public async Task VerifyHeaderAsync()
        {
            await Assertions.Expect(MonitoringDashboardTitle).ToBeVisibleAsync();
            await Assertions.Expect(LiveStatus).ToBeVisibleAsync();
        }

        public async Task VerifyMetricCardsAsync()
        {
            await Assertions.Expect(CPUUtilizationCard).ToBeVisibleAsync();
            await Assertions.Expect(MemoryUsageCard).ToBeVisibleAsync();
            await Assertions.Expect(DiskUtilizationCard).ToBeVisibleAsync();
            await Assertions.Expect(NetworkTrafficCard).ToBeVisibleAsync();
        }
    }
}
