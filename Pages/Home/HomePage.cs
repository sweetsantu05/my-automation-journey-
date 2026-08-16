using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Utils;
using System.Text.RegularExpressions;


namespace WiseUltimaTests.Pages.Home
{
    public class HomePage
    {
        private readonly IPage _page;
        private readonly BasicSetup _setup;

        public HomePage(IPage page)
        {
            _page = page;
            _setup = new BasicSetup(page);
        }

        private ILocator HomeContainer =>
            _page.Locator(".mud-paper.mud-elevation-0.home-page-container");

        private ILocator DarkModeToggle =>
            _page.Locator(".topnav-actions > div");

        private ILocator NotificationIcon =>
            _page.Locator(".mud-badge-root > .mud-tooltip-root");

        private ILocator NotificationHeader =>
            _page.GetByText("Notifications", new() { Exact = true });

        private ILocator CloseNotificationButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "✕" });

        private ILocator ViewAuditLogsButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "View Audit Logs" });

        public async Task VerifyHomePageLoadedAsync()
        {
            await Assertions.Expect(HomeContainer)
                .ToBeVisibleAsync(new() { Timeout = 15000 });
        }

        public async Task ToggleDarkModeAsync()
        {
            await DarkModeToggle.ClickAsync();
        }

        public async Task OpenNotificationsAsync()
        {
            await NotificationIcon.ClickAsync();
            await NotificationHeader.WaitForAsync();
        }

        public async Task CloseNotificationsAsync()
        {
            await CloseNotificationButton.ClickAsync();
        }
        public async Task OpenAuditLogsAsync()
        {
            await ViewAuditLogsButton.ClickAsync();

            var dialog = _page.GetByRole(AriaRole.Dialog)
                .Filter(new() { HasText = "System Activity Monitor" });

            await dialog.WaitForAsync();

            var rows = dialog.Locator("table tbody tr");

            // await Assertions.Expect(rows).ToHaveCountAsync(1, new() { Timeout = 20000 });

            var firstRow = rows.First;

            await Assertions.Expect(firstRow).Not.ToHaveTextAsync("", new()
            {
                Timeout = 30000
            });
        }

        public async Task VerifyAuditLogsContentAsync()
        {
            var rows = _page.GetByRole(AriaRole.Dialog).Locator("table tbody tr");

            await rows.First.WaitForAsync(new() { Timeout = 15000 });

            int count = await rows.CountAsync();

            if (count == 0)
                throw new Exception("Audit logs table is empty.");

            for (int i = 0; i < count; i++)
            {
                var row = rows.Nth(i);
                await Assertions.Expect(row).ToBeVisibleAsync();
            }
        }

        // public async Task WaitForTableToLoadAsync(IPage page)
        // {
        //     await page.Locator(".skeleton-row").First.WaitForAsync(new()
        //     {
        //         State = WaitForSelectorState.Detached,
        //         Timeout = 20000
        //     });

        //     await Assertions.Expect(page.Locator("tr.mud-table-row").First)
        //         .Not.ToHaveTextAsync("", new()
        //         {
        //             Timeout = 20000
        //         });
        // }

        private ILocator SearchTextbox =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "Search..." });

        private ILocator PerformedByColumnValues =>
            _page.GetByRole(AriaRole.Cell).Nth(1); 

        private ILocator ActiveFilterText =>
            _page.GetByText("Active Filters: Search:");

        public async Task<string> GetFirstPerformedByValueAsync()
        {
            var cellText = await _page.Locator("table tbody tr")
                .First
                .Locator("td")
                .Nth(1)
                .InnerTextAsync();

            var cleaned = cellText.Trim();

            // Remove initials like "SA", "SR", etc. from start
            var name = System.Text.RegularExpressions.Regex
                .Replace(cleaned, @"^[A-Z]{1,3}\s+", "");

            return name.Trim();
        }

        public async Task SearchByUserAsync(string user)
        {
            await SearchTextbox.FillAsync(user);
            await _page.Keyboard.PressAsync("Enter");
        }

        public async Task VerifySearchFilterAppliedAsync()
        {
            await Assertions.Expect(ActiveFilterText).ToBeVisibleAsync();
        }

        private ILocator UserDropdownIcon =>
            _page.Locator(".mud-icon-root.mud-icon-default").First;

        private ILocator UserOptions =>
            _page.Locator(".mud-popover-open .mud-list-item");

        private ILocator TableRows =>
            _page.GetByRole(AriaRole.Dialog).Locator("table tbody tr");

        public async Task<string> SelectRandomUserFromDropdownAsync()
        {
            await UserDropdownIcon.ClickAsync();

            await UserOptions.First.WaitForAsync();

            int count = await UserOptions.CountAsync();

            if (count <= 1)
                throw new Exception("No users available in dropdown");

            // Skip "All" → start from index 1
            int randomIndex = Random.Shared.Next(1, count);

            var selectedUser = UserOptions.Nth(randomIndex);

            string userName = await selectedUser.InnerTextAsync();

            await selectedUser.ClickAsync();

            return userName.Trim();
        }

        public async Task VerifyUserFilterAppliedAsync(string user)
        {
            int count = await TableRows.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var rowUser = await TableRows.Nth(i).Locator("td").Nth(1).InnerTextAsync();

                if (!rowUser.Contains(user))
                    throw new Exception($"Row does not match filter. Expected: {user}, Found: {rowUser}");
            }
        }

        public async Task ClearUserFilterAsync()
        {
            await UserDropdownIcon.ClickAsync();

            var allOption = _page.Locator(".mud-popover-open").GetByText("All", new() { Exact = true });

            await allOption.ClickAsync();
        }

        public async Task<string> SelectRandomDropdownValueAsync(ILocator dropdown)
        {
            await dropdown.ClickAsync();

            var options = _page.Locator(".mud-popover-open .mud-list-item");

            await options.First.WaitForAsync();

            int count = await options.CountAsync();

            if (count <= 1)
                throw new Exception("No options available in dropdown");

            int randomIndex = Random.Shared.Next(1, count);

            var selectedOption = options.Nth(randomIndex);

            string value = (await selectedOption.InnerTextAsync()).Trim();

            await selectedOption.ClickAsync();
            
            return value;
        }

        private ILocator CategoryDropdown =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "All" }).Nth(1);

        private ILocator StatusDropdown =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "All" }).Nth(2);

        private ILocator AppDropdown =>
            _page.GetByRole(AriaRole.Textbox, new() { Name = "All" }).Nth(3);


        public async Task<string> SelectCategoryAsync()
        {
            return await SelectRandomDropdownValueAsync(CategoryDropdown);
        }

        public async Task<string> SelectStatusAsync()
        {
            return await SelectRandomDropdownValueAsync(StatusDropdown);
        }

        public async Task<string> SelectAppAsync()
        {
            return await SelectRandomDropdownValueAsync(AppDropdown);
        }

        public async Task VerifyColumnFilterAsync(int columnIndex, string expectedValue)
        {
            var rows = _page.GetByRole(AriaRole.Dialog).Locator("table tbody tr");

            int count = await rows.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var cellText = await rows.Nth(i).Locator("td").Nth(columnIndex).InnerTextAsync();

                if (!cellText.Contains(expectedValue))
                    throw new Exception($"Filter mismatch. Expected: {expectedValue}, Found: {cellText}");
            }
        }
        public async Task ClearDropdownFilterAsync()
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Clear All" }).ClickAsync();
            
        }

        public async Task VerifyColumnFilterAsyncs(int columnIndex, string expectedValue)
        {
            int count = await TableRows.CountAsync();

            for (int i = 0; i < count; i++)
            {
                var cellText = await TableRows.Nth(i)
                    .Locator("td")
                    .Nth(columnIndex)
                    .InnerTextAsync();

                if (!cellText.Contains(expectedValue))
                    throw new Exception($"Filter mismatch. Expected: {expectedValue}, Found: {cellText}");
            }
        }

        public async Task Verify_clear_filter()
            {
                var rows = _page.Locator("table tbody tr");

                int count = await rows.CountAsync();

                Assert.True(count > 1);

                Logger.Info("All filter cleared");
            }

        private ILocator DateRangeButton =>
        _page.GetByRole(AriaRole.Button, new() { Name = "Open Date Range Picker" });
        
        private async Task ClickDateAsync(DateTime date)
        {
            string day = date.Day.ToString();

            for (int i = 0; i < 12; i++) // max 12 months safety
            {
                var dayLocator = _page.Locator(".mud-day")
                    .GetByText(day, new() { Exact = true });

                if (await dayLocator.CountAsync() > 0)
                {
                    await dayLocator.First.ClickAsync();
                    return;
                }

                await _page.Locator("button").Filter(new() { HasText = "" }).Nth(0).ClickAsync();
            }

            throw new Exception($"Date not found: {date}");
        }
        public async Task<(DateTime start, DateTime end)> SelectLast7DaysRangeAsync()
        {
            await DateRangeButton.ClickAsync();

            var today = DateTime.Now.Date;
            var startDate = today.AddDays(-1);

            await ClickDateAsync(today);

            await ClickDateAsync(startDate);

            await ClickDateAsync(today);

            return (startDate, today);
        }
        private ILocator WiseBoardCard =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Wise Board" });

        public async Task Clickwiseborad()
        {
            await WiseBoardCard.ClickAsync();
        }
        private ILocator WiseActionsCard =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Wise Actions" });

        public async Task ClickWiseAction ()
        {
            await WiseActionsCard.ClickAsync();
            await _setup.WaitForDashboardStableAsync();
        }

        private ILocator WiseExplorecard => _page.GetByRole(AriaRole.Heading, new() { Name = "Wise Explore" });

        public async Task ClickWiseExplore()
        {
            await WiseExplorecard.ClickAsync();
            await _setup.WaitForDashboardStableAsync();
        }

        private ILocator WiseAICard =>
            _page.GetByRole(AriaRole.Heading, new() {  Name = "Wise AI" });

        public async Task ClickWiseAi()
        {
            await WiseAICard.ClickAsync();
            await _setup.WaitForDashboardStableAsync();
        }

        private ILocator WiseAgentsCard =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Wise Agents" });

        public async Task ClickWiseAgnet()
        {
            await WiseAgentsCard.ClickAsync();
        }
    }
}