using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseActions
{
    public class WiseActionsPage : BasicSetup
    {
        public WiseActionsPage(IPage page) : base(page) { }

        private ILocator WiseActionsCard =>
            Page.GetByText("Wise Actions", new() { Exact = true });
        
        private ILocator ActButton =>
            Page.Locator(".mud-chip").Filter(new() { HasText = "Act" }).First;

        public async Task OpenAsync()
        {
            await WiseActionsCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-actions"));
            await WaitForDashboardStableAsync();
        }
        public async Task VerifyActButton()
        {
            await Assertions.Expect(ActButton).ToBeVisibleAsync(new() {Timeout=25000});
        }
    }
}
