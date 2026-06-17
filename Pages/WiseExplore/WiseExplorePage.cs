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
        public ILocator ResultCountText => Page.Locator("h6.mud-typography-subtitle1").Filter(new() { HasText = "Number of results" });
        public ILocator TableRows => Page.Locator("table tbody tr");
        public ILocator PaginationDropdown => Page.Locator(".mud-table-pagination select");
        public ILocator PaginationText => Page.Locator(".mud-table-pagination-caption").Last;
        public ILocator NextPageButton => Page.Locator("button[aria-label='Next page']");
        public ILocator SearchBox => Page.GetByPlaceholder("Search for CID's");
        public ILocator IdChips => Page.Locator("span.mud-chip-content");
        public ILocator AlertMessages => Page.Locator("div.mud-alert-message");
        public ILocator CriticalCheckbox => Page.GetByText("Critical").Locator("..").Locator("input[type='checkbox']");
        public ILocator TrippingCheckbox => Page.GetByText("Tripping").Locator("..").Locator("input[type='checkbox']");
        public ILocator SafeCheckbox => Page.GetByText("Safe").Locator("..").Locator("input[type='checkbox']");
        public ILocator StorageCheckbox => Page.GetByText("Storage").Locator("..").Locator("input[type='checkbox']");
        public ILocator DatabaseCheckbox => Page.GetByText("Database").Locator("..").Locator("input[type='checkbox']");
        public ILocator NetworkCheckbox => Page.GetByText("Network").Locator("..").Locator("input[type='checkbox']");
        public ILocator ServerCheckbox => Page.GetByText("Server").Locator("..").Locator("input[type='checkbox']");
        public ILocator MiddlewareCheckbox => Page.GetByText("Middleware").Locator("..").Locator("input[type='checkbox']");
        public ILocator BackupCheckbox => Page.GetByText("Backup").Locator("..").Locator("input[type='checkbox']");
        public ILocator NoRecordsMessage => Page.Locator("text=No Records Found");
        private const string CriticalColor = "#FF4040";
        private const string TrippingColor = "#FF8F00";
        private const string SafeColor = "#38B000";
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
        public async Task<string> GetSelectedPaginationValueAsync()
        {
            var dropdown = Page.Locator("div.mud-table-pagination")
                            .Locator("div.mud-select-input")
                            .First;

            return (await dropdown.InnerTextAsync()).Trim();
        }
        public async Task ClickNextPageAsync()
        {
            await Task.Delay(5000);
            await NextPageButton.ClickAsync();
            await WaitForPageStableAsync();
        }

        // public async Task ClickNextPageAsync1()
        // {
        //     var before = await GetPaginationTextAsync();

        //     await NextPageButton.ClickAsync();

        //     await Page.WaitForFunctionAsync(
        //         @"(oldText) => {
        //             const el = document.querySelector('.mud-table-pagination-caption');
        //             return el && el.innerText !== oldText;
        //         }",
        //         before);

        //     await WaitForPageStableAsync();
        // }
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
            await WaitForResultsToLoadAsync();
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

        public async Task GoToFirstPageAsync()
        {
            var firstPageButton = Page.Locator("button[aria-label='First page']");

            if (await firstPageButton.IsVisibleAsync())
            {
                await firstPageButton.ClickAsync();
                await WaitForPageStableAsync();
            }
        }

        private ILocator RowTypeAlertByIndex(int index)
        {
            return Page.Locator("table tbody tr")
                .Nth(index)
                .Locator("div.mud-alert-message")
                .Last;
        }  

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

        public async Task<int> ValidateAllRowsTypeWithPaginationAsync(string expectedType)
        {
            if (await NoRecordsMessage.IsVisibleAsync())
                return 0;

            await SetPaginationTo100Async();

            int validatedRows = 0;

            while (true)
            {
                int rowCount = await TableRows.CountAsync();

                for (int i = 0; i < rowCount; i++)
                {
                    var typeText = await RowTypeAlertByIndex(i).InnerTextAsync();

                    if (!typeText.Trim().Equals(
                            expectedType,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception(
                            $"Row {i + 1} type mismatch. Expected: {expectedType}, Actual: {typeText}");
                    }

                    validatedRows++;
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await ClickNextPageAsync();
            }

            return validatedRows;
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

        // public async Task<int> GetTotalResultsCount()
        // {
        //     await WaitForResultsToLoadAsync();
        //     var text = await Page
        //         .Locator("h6.mud-typography")
        //         .Filter(new() { HasText = "Number of results:" })
        //         .InnerTextAsync();

        //     var countText = text.Replace("Number of results:", "").Trim();

        //     return int.Parse(countText);
        // }

        public async Task WaitForResultsToLoadAsync()
        {
            var searchingMessage = Page.GetByText("Searching system");

            try
            {
                if (await searchingMessage.IsVisibleAsync(new() { Timeout = 1000 }))
                {
                    await searchingMessage.WaitForAsync(new()
                    {
                        State = WaitForSelectorState.Hidden,
                        Timeout = 60000
                    });
                }
            }
            catch
            {
                // Searching message never appeared.
                // Continue execution normally.
            }

            await WaitForPageStableAsync();
        }

        public async Task<int> ValidateAllRowsEnvironmentWithPaginationAsync(string expectedEnvironment)
        {
            await WaitForResultsToLoadAsync();

            if (await NoRecordsMessage.IsVisibleAsync())
            {
                return 0;
            }

            await SetPaginationTo100Async();

            int validatedRows = 0;

            while (true)
            {
                await WaitForPageStableAsync();

                int rows = await TableRows.CountAsync();

                for (int i = 0; i < rows; i++)
                {
                    var environmentText = await Page
                        .Locator("tbody tr")
                        .Nth(i)
                        .Locator("div[role='alert']")
                        .First
                        .InnerTextAsync();

                    bool isValid;

                    if (expectedEnvironment.Equals(
                            "Production",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        isValid =
                            environmentText.Trim().Equals(
                                "Production",
                                StringComparison.OrdinalIgnoreCase)
                            ||
                            environmentText.Trim().Equals(
                                "PRD",
                                StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        isValid =
                            environmentText.Trim().Equals(
                                expectedEnvironment,
                                StringComparison.OrdinalIgnoreCase);
                    }

                    if (!isValid)
                    {
                        throw new Exception(
                            $"Row {i + 1} mismatch. Expected: {expectedEnvironment}, Actual: {environmentText}");
                    }

                    validatedRows++;
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await ClickNextPageAsync();
            }

            return validatedRows;
        }

        public async Task ValidateCriticalResultsAsync()
        {
            await ValidateFilterResultsAsync(CriticalColor);
        }

        public async Task ValidateTrippingResultsAsync()
        {
            await ValidateFilterResultsAsync(TrippingColor);
        }

        public async Task ValidateSafeResultsAsync()
        {
            await ValidateFilterResultsAsync(SafeColor);
        }

        public async Task ValidateFilterResultsAsync(string expectedColor)
        {
            int total = await GetTotalResultsCountAsync();

            // No Records scenario
            if (total == 0)
            {
                await Assertions.Expect(NoRecordsMessage)
                    .ToBeVisibleAsync();

                return;
            }

            await SetPaginationTo100Async();
            await WaitForPageStableAsync();

            int validatedRows = 0;

            while (true)
            {
                await WaitForPageStableAsync();

                int rowCount = await TableRows.CountAsync();

                for (int i = 0; i < rowCount; i++)
                {
                    var row = TableRows.Nth(i);

                    var chipStyle = await row
                        .Locator("div.mud-chip")
                        .First
                        .GetAttributeAsync("style");

                    Assert.NotNull(chipStyle);

                    var normalizedStyle = chipStyle!
                        .Replace(" ", "")
                        .ToUpperInvariant();

                    Assert.Contains(
                        $"COLOR:{expectedColor.ToUpperInvariant()}",
                        normalizedStyle);

                    validatedRows++;
                }

                if (await NextPageButton.IsDisabledAsync())
                    break;

                await ClickNextPageAsync();
            }

            Assert.Equal(total, validatedRows);
        }
    }
}
  