using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;
using System.Linq;

namespace WiseUltimaTests.Pages.WiseExplore
{
    public class WiseExplorePage : BasicSetup
    {
        public WiseExplorePage(IPage page) : base(page) { }
        private ILocator WiseExplorecard => Page.GetByRole(AriaRole.Link, new() { Name = "Wise Explore" });
        private ILocator FirstSystemRow =>Page.Locator("table tbody tr").First;
        public async Task OpenAsync()
        {
            await NavMenuToggleButton();
            await WiseExplorecard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-explore"));
            await WaitForDashboardStableAsync();
        }
        public async Task VerifyAtLeastOneResultAsync()
        {
            await Assertions.Expect(FirstSystemRow).ToBeVisibleAsync(new() { Timeout = 25000 });
        }
        public ILocator ResultCountText => Page.Locator("h6.mud-typography-subtitle1").Filter(new() { HasText = "Number of results" });
        public ILocator TableRows => Page.Locator("table tbody tr");

        public ILocator PaginationDropdown => Page.Locator(".mud-table-pagination select");

        public ILocator PaginationText => Page.Locator(".mud-table-pagination-caption");

        public ILocator NextPageButton => Page.Locator("button[aria-label='Next page']");

        public ILocator SearchBox => Page.GetByPlaceholder("Search for CID's");

        public ILocator IdChips => Page.Locator("span.mud-chip-content");

        public ILocator AlertMessages => Page.Locator("div.mud-alert-message");

        public async Task<int> GetTotalResultsCountAsync()
        {
            var text = await ResultCountText.InnerTextAsync();
            var match = Regex.Match(text, @"\d+");
            return int.Parse(match.Value);
        }

        public async Task<int> GetCurrentRowCountAsync()
        {
            return await TableRows.CountAsync();
        }
        public async Task<string> GetPaginationTextAsync()
        {
            return await PaginationText.InnerTextAsync();
        }
        public async Task ClickNextPageAsync()
        {
            await Task.Delay(5000);
            await NextPageButton.ClickAsync();
            await WaitForPageStableAsync();
        }
        public async Task<string> GetRandomIdAsync()
        {
            var ids = await IdChips.AllTextContentsAsync();
            var random = new Random();
            return ids[random.Next(ids.Count)];
        }
        public async Task SearchAsync(string value)
        {
            await SearchBox.FillAsync("");
            await SearchBox.FillAsync(value);
            await Page.Keyboard.PressAsync("Enter");
            await WaitForPageStableAsync();
        }
        public async Task<List<string>> GetAllIdsFromTableAsync()
        {
            var ids = await IdChips.AllTextContentsAsync();
            return ids.ToList();
        }

        public async Task<List<string>> GetAllAlertMessagesAsync()
        {
            var alerts = await AlertMessages.AllTextContentsAsync();
            return alerts.ToList();
        }
        public async Task<int> GetTotalRowsAcrossPagesAsync()
        {
            int totalRows = 0;

            while (true)
            {
                totalRows += await GetCurrentRowCountAsync();

                var nextDisabled = await NextPageButton.IsDisabledAsync();
                if (nextDisabled)
                    break;

                await ClickNextPageAsync();
            }

            return totalRows;
        }
        public async Task SetPaginationTo100Async()
        {
            var dropdown = Page.Locator("div.mud-table-pagination")
                            .Locator("div.mud-select")
                            .First;

            await dropdown.ClickAsync();

            var optionsContainer = Page.Locator("div.mud-popover-open");

            await optionsContainer.WaitForAsync(new() { Timeout = 5000 });

            var option100 = optionsContainer
                .Locator("div.mud-list-item")
                .Filter(new() { HasTextString = "100" });

            await option100.First.ClickAsync();

            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public ILocator CriticalCheckbox => Page.GetByText("Critical").Locator("..").Locator("input[type='checkbox']");
        public ILocator TrippingCheckbox => Page.GetByText("Tripping").Locator("..").Locator("input[type='checkbox']");
        public ILocator SafeCheckbox => Page.GetByText("Safe").Locator("..").Locator("input[type='checkbox']");

        public ILocator AlertMessage => Page.Locator("div.mud-alert");

        public ILocator RedAlerts => Page.Locator("div.mud-alert[style*='#FF4040']");
        public ILocator YellowAlerts => Page.Locator("div.mud-alert[style*='#FFBF00']");
        public ILocator GreenAlerts => Page.Locator("div.mud-alert[style*='#38b000']");

        public async Task SelectCriticalAsync()
        {
            await CriticalCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectTrippingAsync()
        {
            await TrippingCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectSafeAsync()
        {
            await SafeCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task<int> GetAlertCountAsync()
        {
            return await AlertMessages.CountAsync();
        }

        public async Task<int> GetRedAlertCountAsync()
        {
            return await RedAlerts.CountAsync();
        }

        public async Task<int> GetYellowAlertCountAsync()
        {
            return await YellowAlerts.CountAsync();
        }

        public async Task<int> GetGreenAlertCountAsync()
        {
            return await GreenAlerts.CountAsync();
        }
        public async Task<int> GetTotalAlertsAcrossPagesAsync()
        {
            int total = 0;

            while (true)
            {
                await WaitForPageStableAsync();

                total += await AlertMessages.CountAsync();

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await NextPageButton.ClickAsync();
            }

            return total;
        }

        public ILocator AlertMessageInRow(ILocator row) =>
            row.Locator("div.mud-alert-message");

        public ILocator AlertContainerInRow(ILocator row) =>
            row.Locator("div.mud-alert");

            public async Task ValidateAllRowsHaveExpectedStatusAsync(string expectedText, string expectedColor)
        {
            while (true)
            {
                await WaitForPageStableAsync();

                var rows = TableRows;
                int count = await rows.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var row = rows.Nth(i);

                    var alertText = await AlertMessageInRow(row).InnerTextAsync();
                    var alertStyle = await AlertContainerInRow(row).GetAttributeAsync("style");

                    if (!alertText.Contains(expectedText))
                        throw new Exception($"Row {i} does not contain {expectedText}");

                    if (alertStyle == null || !alertStyle.Contains(expectedColor))
                        throw new Exception($"Row {i} does not have color {expectedColor}");
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await NextPageButton.ClickAsync();
            }
        }
        public async Task GoToFirstPageAsync()
        {
            var firstPageButton = Page.Locator("button[aria-label='First page']");

            if (await firstPageButton.IsVisibleAsync())
            {
                await firstPageButton.ClickAsync();
                await WaitForPageStableAsync();
            }
        }
        
    }
}
