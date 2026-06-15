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
        public async Task ValidateAllRowsStatusWithPaginationAsync(string expectedText, string expectedColor)
        {
            int total = await GetTotalResultsCountAsync();

            if (total == 0)
            {
                var noData = Page.GetByText("No Records Found");
                await Assertions.Expect(noData).ToBeVisibleAsync();
                return;
            }

            await SetPaginationTo100Async();
            await WaitForPageStableAsync();

            while (true)
            {
                await WaitForPageStableAsync();

                var rows = Page.Locator("table tbody tr");

                int count = await rows.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var row = rows.Nth(i);

                    var text = await row.Locator("div.mud-alert-message").InnerTextAsync();
                    var style = await row.Locator("div.mud-alert").GetAttributeAsync("style");

                    if (string.IsNullOrEmpty(text))
                        continue;

                    if (!text.Contains(expectedText))
                        throw new Exception($"Row {i} text mismatch");

                    if (style == null || !style.Contains(expectedColor))
                        throw new Exception($"Row {i} color mismatch");
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await NextPageButton.ClickAsync();
            }
        }
        public async Task<int> GetTotalRowsAcrossPagesOptimizedAsync()
        {
            int total = await GetTotalResultsCountAsync();

            if (total == 0)
            {
                var noData = Page.GetByText("No Records Found");
                await Assertions.Expect(noData).ToBeVisibleAsync();
                return 0;
            }

            await SetPaginationTo100Async();
            await WaitForPageStableAsync();

            int count = 0;

            while (true)
            {
                await WaitForPageStableAsync();

                count += await IdChips.CountAsync();

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await NextPageButton.ClickAsync();
            }

            return count;
        }

                private ILocator RowTypeAlertByIndex(int index)
        {
            return Page.Locator("table tbody tr")
                .Nth(index)
                .Locator("div.mud-alert-message")
                .Last;
        }
                

        public ILocator StorageCheckbox => Page.GetByText("Storage").Locator("..").Locator("input[type='checkbox']");
        public ILocator DatabaseCheckbox => Page.GetByText("Database").Locator("..").Locator("input[type='checkbox']");
        public ILocator NetworkCheckbox => Page.GetByText("Network").Locator("..").Locator("input[type='checkbox']");
        public ILocator ServerCheckbox => Page.GetByText("Server").Locator("..").Locator("input[type='checkbox']");
        public ILocator MiddlewareCheckbox => Page.GetByText("Middleware").Locator("..").Locator("input[type='checkbox']");
        public ILocator BackupCheckbox => Page.GetByText("Backup").Locator("..").Locator("input[type='checkbox']");
        public ILocator NoRecordsMessage => Page.Locator("text=No Records Found");


        public async Task SelectStorageAsync()
        {
            await StorageCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectDatabaseAsync()
        {
            await DatabaseCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectNetworkAsync()
        {
            await NetworkCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectServerAsync()
        {
            await ServerCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectMiddlewareAsync()
        {
            await MiddlewareCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task SelectBackupAsync()
        {
            await BackupCheckbox.CheckAsync();
            await WaitForPageStableAsync();
        }

        public async Task ValidateAllRowsTypeWithPaginationAsync(string expectedType)
        {
            if (await NoRecordsMessage.IsVisibleAsync())
            {
                return;
            }
            
            int total = await GetTotalResultsCountAsync();

            if (total == 0)
            {
                var noData = Page.GetByText("No Records Found");
                await Assertions.Expect(noData).ToBeVisibleAsync();
                return;
            }

            await SetPaginationTo100Async();
            await WaitForPageStableAsync();

            int validated = 0;

            while (true)
            {
                await WaitForPageStableAsync();

                int rowCount = await TableRows.CountAsync();

                for (int i = 0; i < rowCount; i++)
                {
                    var typeText = await RowTypeAlertByIndex(i).InnerTextAsync();

                    if (!typeText.Trim().Equals(expectedType, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Row {i} type mismatch. Expected: {expectedType}, Actual: {typeText}");
                    }

                    validated++;
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await ClickNextPageAsync();
            }

            if (validated != total)
            {
                throw new Exception($"Row count mismatch. Expected: {total}, Actual: {validated}");
            }
        }

        public async Task SelectEnvironmentAsync(string environment)
        {
            var checkbox = Page
                .Locator("div.check")
                .Filter(new() { HasText = environment })
                .Locator("input.mud-checkbox-input");

            await checkbox.ClickAsync();

            await Page.WaitForTimeoutAsync(2000);
        }

        public async Task<int> GetTotalResultsCount()
        {
            var text = await Page
                .Locator("h6.mud-typography")
                .Filter(new() { HasText = "Number of results:" })
                .InnerTextAsync();

            var countText = text.Replace("Number of results:", "").Trim();

            return int.Parse(countText);
        }

        public async Task ValidateAllRowsEnvironmentWithPaginationAsync(string expectedEnvironment)
        {
            int totalResults = await GetTotalResultsCount();

            // Handle No Records Found
            if (totalResults == 0)
            {
                var noRecords = await Page
                    .Locator("h6.mud-typography")
                    .Filter(new() { HasText = "No Records Found" })
                    .IsVisibleAsync();

                Assert.True(noRecords,
                    "Expected No Records Found message.");

                return;
            }

            await SetPaginationTo100Async();

            int verifiedRows = 0;

            while (true)
            {
                await Page.WaitForTimeoutAsync(2000);

                var rows = await Page.Locator("tbody tr").CountAsync();

                for (int i = 0; i < rows; i++)
                {
                    // FIRST alert only
                    var environmentText = await Page
                        .Locator("tbody tr")
                        .Nth(i)
                        .Locator("div[role='alert']")
                        .First
                        .InnerTextAsync();

                    Assert.True(
                        environmentText.Trim()
                            .Equals(expectedEnvironment,
                            StringComparison.OrdinalIgnoreCase),

                        $"Row {i + 1} mismatch. " +
                        $"Expected: {expectedEnvironment}, " +
                        $"Actual: {environmentText}"
                    );

                    verifiedRows++;
                }

                var nextButton = Page.GetByRole(AriaRole.Button, new() { Name = "Next page" });

                if (await nextButton.IsDisabledAsync())
                    break;

                await nextButton.ClickAsync();

                await Page.WaitForTimeoutAsync(2000);
            }

            Assert.Equal(totalResults, verifiedRows);
        }
        public async Task ValidateSearchResultsWithPaginationAsync(string expectedText)
{
    while (true)
    {
        await WaitForPageStableAsync();

        int rowCount = await TableRows.CountAsync();

        for (int i = 0; i < rowCount; i++)
        {
            var row = TableRows.Nth(i);

            var values = await row
                .Locator("div.mud-alert-message")
                .AllTextContentsAsync();

            bool found = values.Any(v =>
                v.Contains(expectedText, StringComparison.OrdinalIgnoreCase));

            Assert.True(
                found,
                $"Row {i + 1} does not contain '{expectedText}'. Values: {string.Join(", ", values)}");
        }

        // if (await NextPageButton.IsDisabledAsync())
        //     break;

        // await ClickNextPageAsync();
    }
}
    }
}
  