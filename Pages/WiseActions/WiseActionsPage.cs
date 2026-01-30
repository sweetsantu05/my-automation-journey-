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

        private ILocator RaiseTicketButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Raise Ticket" });

        private ILocator ApplyFixButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Apply Fix" });

        public async Task OpenAsync()
        {
            await WiseActionsCard.ClickAsync();
            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex(".*/wise-actions"));
        }

        public async Task OpenActionModalAsync()
        {
            await ActButton.ClickAsync();
        }
        
        public async Task VerifyActionButtonsAsync()
        {
            await Assertions.Expect(RaiseTicketButton).ToBeVisibleAsync();
            await Assertions.Expect(ApplyFixButton).ToBeVisibleAsync();
        }
    }
}
