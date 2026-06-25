using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;

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
        // public async Task ServiceNow_button()
        // {
        //     await Page.GetByText("ServiceNow").ClickAsync();
        // }
        private  ILocator servicenow_button=>Page.GetByText("ServiceNow");
        public async Task Click_ServiceNow_Button()
        {
            await servicenow_button.ClickAsync();
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
        public async Task ValidateCardDataConsistencyAsync123(ActionCardType card)
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
            
            int actualRows = await Page
                .Locator(".mud-table-body tr")
                .CountAsync();

            Assert.Equal(expectedTotal, actualRows);
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
            await WaitForPageStableAsync();
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
            await Task.Delay(10000);
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

        // main funtion 
        public async Task ValidateCardDataConsistencyAsync(ActionCardType card)
        {
            await ClickCard(card);

            var (red, amber) =
                await GetRedAmberCountAsync(card);

            int expectedCount = red + amber;

            var healthyMessage =
                Page.GetByText("All systems are healthy!");

            if (expectedCount == 0)
            {
                await Assertions.Expect(healthyMessage)
                    .ToBeVisibleAsync();

                return;
            }

            await Assertions.Expect(
                Page.Locator(".mud-table-body tr").First
            ).ToBeVisibleAsync();

            await SetPaginationTo100Async();

            int actualRows =
                await CountRowsAcrossAllPagesAsync();

            Console.WriteLine(
                $"[{card}] Expected={expectedCount}, Actual={actualRows}"
            );

            Assert.Equal(expectedCount, actualRows);
        }
        private async Task SetPaginationTo100Async()
        {
            var paginationDropdown =
                Page.Locator(".mud-table-pagination")
                    .GetByText("10", new() { Exact = true });

            await paginationDropdown.ClickAsync();

            await Page.GetByText("100", new() { Exact = true })
            .WaitForAsync();

            await Page.GetByText("100", new() { Exact = true })
                .ClickAsync();
            await Page.WaitForTimeoutAsync(10000);

            await WaitForTableToLoadAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        private async Task<int> CountRowsAcrossAllPagesAsync()
        {
            int totalRows = 0;

            while (true)
            {
                await Page.WaitForTimeoutAsync(10000);

                int rows =
                    await Page.Locator(".mud-table-body tr")
                        .CountAsync();

                totalRows += rows;

                string beforePagination =
                    await TablePaginationText.InnerTextAsync();

                var nextButton =
                    Page.Locator(".mud-table-pagination-actions button")
                        .Nth(2);

                await nextButton.ClickAsync(new()
                {
                    Force = true
                });

                await Page.WaitForTimeoutAsync(10000);

                string afterPagination =
                    await TablePaginationText.InnerTextAsync();

                Console.WriteLine(
                    $"Before: {beforePagination}"
                );

                Console.WriteLine(
                    $"After : {afterPagination}"
                );

                if (beforePagination == afterPagination)
                {
                    Console.WriteLine(
                        "Last page reached."
                    );

                    break;
                }
            }

            return totalRows;
        }
        public async Task<int> GetDisplayedTableCountAsync()
        {
            return await GetTotalTableCountAsync();
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
