using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseActions
{
    public class WiseActionsPage : BasicSetup
    {
        public WiseActionsPage(IPage page) : base(page) { }

        private ILocator WiseActionsCard =>
            Page.GetByRole(AriaRole.Link, new() { Name = "Wise Actions" });

        private ILocator ActButton =>
            Page.Locator(".mud-chip").Filter(new() { HasText = "Act" }).First;

        public async Task OpenAsync()
        {
            await NavMenuToggleButton();
            await WiseActionsCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-actions"));
            await WaitForDashboardStableAsync();
        }
        public async Task VerifyActButton()
        {
            await Assertions.Expect(ActButton).ToBeVisibleAsync(new() {Timeout=25000});
        }

        private ILocator GetCardSection(ActionCardType card)
        {
            return Page.Locator(".mud-card")
                .Filter(new() { HasText = card.ToString() })
                .First;
        }

        private async Task<int> GetStatusCountAsync(ActionCardType card, string colorClass, int index)
        {
            var locator = GetCardSection(card)
                .Locator($".{colorClass}")
                .Nth(index);

            var text = await locator.InnerTextAsync();
            return int.Parse(text.Trim());
        }

        private async Task<int> GetTableRowCountAsync()
        {
            return await Page
                .Locator(".mud-table-body tr")
                .CountAsync();
        }
        private async Task ClickCard(ActionCardType card)
        {
            var cardSection = Page.Locator(".mud-card")
                .Filter(new() { HasText = card.ToString() })
                .First;

            await cardSection.GetByText(card.ToString(), new() { Exact = true }).ClickAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await Page.WaitForTimeoutAsync(1000);
            // await cardSection.ClickAsync();
        }
        public async Task ValidateCardDataConsistencyAsync(ActionCardType card)
        {
            await ClickCard(card);

            var (red, amber) = await GetRedAmberCountAsync(card);

            int expectedTotal = red + amber;

            var healthyMessage = Page.Locator("text=All systems are healthy");

            if (expectedTotal == 0)
            {
                if (await healthyMessage.IsVisibleAsync())
                {
                    Console.WriteLine($"[{card}] No issues → System healthy");
                    Assert.True(true);
                    return;
                }
                else
                {
                    throw new Exception($"[{card}]  Expected healthy state but not found");
                }
            }

            int actualTotal = await GetTotalTableCountAsync();

            Console.WriteLine($"[{card}] Red: {red}, Amber: {amber}, Expected: {expectedTotal}, Actual: {actualTotal}");

            Assert.Equal(expectedTotal, actualTotal);
        }

        private async Task<(int red, int amber)> GetRedAmberCountAsync(ActionCardType card)
        {
            var cardSection = GetCardSection(card);

            var values = cardSection.Locator("h6");

            int count = await values.CountAsync();

            if (count < 2)
                return (0, 0);

            int red = int.Parse(await values.Nth(0).InnerTextAsync());
            int amber = int.Parse(await values.Nth(1).InnerTextAsync());

            return (red, amber);
        }

        private ILocator TablePaginationText =>
            Page.Locator(".mud-table-pagination").First;

        private async Task<int> GetTotalTableCountAsync()
        {
            var text = await TablePaginationText.InnerTextAsync();

            var match = Regex.Match(text, @"of\s+(\d+)");

            if (!match.Success)
                return 0;

            return int.Parse(match.Groups[1].Value);
        }

        public async Task ClickRandomRowActAsync()
        {
            await WaitForTableToLoadAsync();

            var actButtons = Page.Locator("div.mud-chip:has(span.mud-chip-content:has-text('Act'))");

            int count = await actButtons.CountAsync();

            Console.WriteLine($"Act buttons found: {count}");

            if (count == 0)
                throw new Exception(" No Act buttons found");

            int index = new Random().Next(count);

            var button = actButtons.Nth(index);

            await button.ScrollIntoViewIfNeededAsync();

            await Page.WaitForTimeoutAsync(300);

            await button.ClickAsync(new() { Force = true });
        }

        public async Task WaitForTableToLoadAsync()
        {
            await Page.Locator(".mud-table-body").WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible
            });

            await Page.WaitForFunctionAsync(@"
                () => document.querySelectorAll('.mud-table-body tr').length > 0
            ");
        }
        public async Task<bool> IsRaiseTicketDisabledAsync()
        {
            var button = Page.Locator("button:has-text('RAISE TICKET')");
            return await button.IsDisabledAsync();
        }

        public async Task ClickRaiseTicketAsync()
        {
            await Page.Locator("button:has-text('RAISE TICKET')").ClickAsync();
        }

        public async Task<string> SelectRandomTicketPlatformAsync()
        {
            var options = Page.Locator(".mud-popover-open .mud-list-item:visible");

            await options.First.WaitForAsync();

            int count = await options.CountAsync();

            if (count == 0)
                throw new Exception(" No visible ticket options");

            int index = new Random().Next(count);

            await options.Nth(index).ClickAsync();

            await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

            var snackbar = Page.Locator(".mud-snackbar-content-message");
            await snackbar.WaitForAsync();

            var ticket = await Page.Locator(".ticket-number-value").InnerTextAsync();

            return ticket.Trim();
        }

        public async Task<string> GetRaisedTicketNumberAsync()
        {
            var ticket = Page.Locator(".ticket-number-value");

            await ticket.WaitForAsync();

            return (await ticket.InnerTextAsync()).Trim();
        }

        public async Task CloseActionPopupAsync()
        {
            await Page.Locator("button[aria-label='close']").ClickAsync();
        }

        public async Task GoToMyTicketsAsync()
        {
            await Page.Locator("button:has-text('MY TICKETS')").ClickAsync();
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task<bool> VerifyTicketInMyTicketsAsync(string ticketNumber)
        {
            var rows = Page.Locator(".mud-table-body tr");

            int count = await rows.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var rowText = await rows.Nth(i).InnerTextAsync();

                if (rowText.Contains(ticketNumber))
                    return true;
            }

            return false;
        }
    }

    public enum ActionCardType
    {
        Server,
        Storage,
        Database,
        Network,
        Middleware,
        Backup
    }
}
