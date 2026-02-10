using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseMonitor
{
    public class WiseMonitorPage : BasicSetup
    {
        public WiseMonitorPage(IPage page) : base(page) { }

        private ILocator NavMenuToggle =>Page.Locator(".mud-navmenu > div:nth-child(2)");

        private ILocator WiseMonitoryNavItem =>
            Page.Locator(".mud-navmenu").GetByText("Wise Monitory", new() { Exact = true });
        protected ILocator HostChip =>Page.GetByText("Host: wisemaestro-rdb-server", new() { Exact = false });

        public async Task OpenAsync()
        {
            await NavMenuToggle.ClickAsync();
            await WiseMonitoryNavItem.ClickAsync();
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
    }
}
