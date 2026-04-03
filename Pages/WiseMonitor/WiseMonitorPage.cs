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
    }
}
