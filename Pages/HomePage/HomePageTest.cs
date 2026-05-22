using Microsoft.Playwright;
using WiseUltimaTests.Pages.Home;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;


namespace WiseUltimaTests.Tests.Home
{
    [Collection("Playwright collection")]
    [AllureSuite("Home Page Tests")]
    public class HomePageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private HomePage _homePage = null!;
        private BasicSetup _setup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _homePage = new HomePage(Page);
            _setup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();

            await _setup.WaitForPageAsync(3);
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Verify_HomePage_Loaded")]
        [AllureTag("Regression")]
        public async Task TC_001_Verify_HomePage_Loaded()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.VerifyHomePageLoadedAsync();
                await Assertions.Expect(Page.GetByText(new Regex(@"Welcome"))).ToBeVisibleAsync();

                Logger.Info("TC_HOME_01: Home page loaded successfully.");
            }, nameof(TC_001_Verify_HomePage_Loaded));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Toggle_Dark_Mode")]
        [AllureTag("Regression")]
        public async Task TC_002_Toggle_Dark_Mode()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.ToggleDarkModeAsync();
                
                Logger.Info("TC_HOME_02: Dark mode toggled successfully.");
            }, nameof(TC_002_Toggle_Dark_Mode));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_Open_Notifications")]
        [AllureTag("Regression")]
        public async Task TC_003_Open_Notifications()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenNotificationsAsync();
                await Assertions.Expect(Page.GetByText("Notifications ✕")).ToBeVisibleAsync();

                Logger.Info("TC_HOME_03: Notifications opened successfully.");
            }, nameof(TC_003_Open_Notifications));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Close_Notifications")]
        [AllureTag("Regression")]
        public async Task TC_004_Close_Notifications()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenNotificationsAsync();
                await _homePage.CloseNotificationsAsync();

                Logger.Info("TC_HOME_04: Notifications closed successfully.");
            }, nameof(TC_004_Close_Notifications));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Open_Audit_Logs")]
        [AllureTag("Regression")]
        public async Task TC_005_Open_Audit_Logs()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "System Activity Monitor" })).ToBeVisibleAsync();

                Logger.Info("TC_HOME_05: Audit logs opened successfully.");
            }, nameof(TC_005_Open_Audit_Logs));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_Audit_Logs_Content")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_Audit_Logs_Content()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync(); 
                await _homePage.VerifyAuditLogsContentAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Columnheader, new() { Name = "Timestamp" })).ToBeVisibleAsync();

                Logger.Info("TC_HOME_06: Audit logs content verified successfully.");
            }, nameof(TC_006_Verify_Audit_Logs_Content));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Search_By_PerformedBy")]
        [AllureTag("Regression")]
        public async Task TC_007_Search_By_PerformedBy()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();
                var user = await _homePage.GetFirstPerformedByValueAsync();

                await _homePage.SearchByUserAsync(user);
                await _homePage.VerifySearchFilterAppliedAsync();

                Logger.Info($"TC_007: Search applied successfully for user: {user}");

            }, nameof(TC_007_Search_By_PerformedBy));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_Filtered_Result")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_Filtered_Result()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var user = await _homePage.GetFirstPerformedByValueAsync();

                await _homePage.SearchByUserAsync(user);
                await _homePage.VerifySearchFilterAppliedAsync();

                var results = Page.Locator("table tbody tr");

                int count = await results.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var rowUser = await results.Nth(i).Locator("td").Nth(1).InnerTextAsync();
                    Assert.Contains(user, rowUser);
                }

                Logger.Info("TC_008: Filtered results verified successfully.");

            }, nameof(TC_008_Verify_Filtered_Result));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Clear_Search_Filter")]
        [AllureTag("Regression")]
        public async Task TC_009_Clear_Search_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();
                var user = await _homePage.GetFirstPerformedByValueAsync();

                await _homePage.SearchByUserAsync(user);
                await _homePage.VerifySearchFilterAppliedAsync();

                await Page.GetByRole(AriaRole.Textbox, new() { Name = "Search..." }).FillAsync("");

                await Page.Keyboard.PressAsync("Enter");

                await Assertions.Expect(Page.GetByText("Active Filters: Search:")).Not.ToBeVisibleAsync();

                Logger.Info("TC_009: Search filter cleared successfully.");

            }, nameof(TC_009_Clear_Search_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_User_Filter_Selection")]
        [AllureTag("Regression")]
        public async Task TC_010_User_Filter_Selection()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var user = await _homePage.SelectRandomUserFromDropdownAsync();

                await _homePage.VerifyUserFilterAppliedAsync(user);

                Logger.Info($"TC_010: User filter applied successfully for {user}");

            }, nameof(TC_010_User_Filter_Selection));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Verify_User_Filter_Data")]
        [AllureTag("Regression")]
        public async Task TC_011_Verify_User_Filter_Data()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();
                var user = await _homePage.SelectRandomUserFromDropdownAsync();

                var rows = Page.Locator("table tbody tr");
                int count = await rows.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var rowUser = await rows.Nth(i).Locator("td").Nth(1).InnerTextAsync();

                    Assert.True(rowUser.Contains(user),
                        $"Mismatch found. Expected: {user}, Found: {rowUser}");
                }

                Logger.Info("TC_011: All filtered rows match selected user");

            }, nameof(TC_011_Verify_User_Filter_Data));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Clear_User_Filter")]
        [AllureTag("Regression")]
        public async Task TC_012_Clear_User_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();
                var user = await _homePage.SelectRandomUserFromDropdownAsync();

                await _homePage.VerifyUserFilterAppliedAsync(user);

                await _homePage.ClearUserFilterAsync();

                var rows = Page.Locator("table tbody tr");

                int count = await rows.CountAsync();

                bool multipleUsersFound = false;
                string firstUser = await rows.First.Locator("td").Nth(1).InnerTextAsync();

                for (int i = 1; i < count; i++)
                {
                    var rowUser = await rows.Nth(i).Locator("td").Nth(1).InnerTextAsync();

                    if (!rowUser.Equals(firstUser))
                    {
                        multipleUsersFound = true;
                        break;
                    }
                }

                Assert.True(multipleUsersFound, "Filter not cleared properly");
                Logger.Info("TC_012: User filter cleared and data restored successfully");

            }, nameof(TC_012_Clear_User_Filter));            
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Category_Filter")]
        [AllureTag("Regression")]
        public async Task TC_013_Category_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectCategoryAsync();

                await _homePage.VerifyColumnFilterAsyncs(3, value);

                Logger.Info($"Category filter applied: {value}");

            }, nameof(TC_013_Category_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Category_Clear")]
        [AllureTag("Regression")]
        public async Task TC_014_Category_Clear()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectCategoryAsync();

                await _homePage.ClearDropdownFilterAsync();

                await _homePage.Verify_clear_filter();
            }, nameof(TC_014_Category_Clear));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Category_Data_Verification")]
        [AllureTag("Regression")]
        public async Task TC_015_Category_Data_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectCategoryAsync();

                await _homePage.VerifyColumnFilterAsync(3, value);

                Logger.Info("Category data verified");

            }, nameof(TC_015_Category_Data_Verification));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Status_Filter")]
        [AllureTag("Regression")]
        public async Task TC_016_Status_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectStatusAsync();

                await _homePage.VerifyColumnFilterAsync(5, value);

                Logger.Info($"Status filter applied: {value}");

            }, nameof(TC_016_Status_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Status_Clear")]
        [AllureTag("Regression")]
        public async Task TC_017_Status_Clear()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                await _homePage.SelectStatusAsync();

                await _homePage.ClearDropdownFilterAsync();

                await _homePage.Verify_clear_filter();

            }, nameof(TC_017_Status_Clear));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Status_Data_Verification")]
        [AllureTag("Regression")]
        public async Task TC_018_Status_Data_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectStatusAsync();

                await _homePage.VerifyColumnFilterAsync(5, value);

                Logger.Info("Status data verified");

            }, nameof(TC_018_Status_Data_Verification));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_App_Filter")]
        [AllureTag("Regression")]
        public async Task TC_019_App_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectAppAsync();

                await _homePage.VerifyColumnFilterAsync(4, value);

                Logger.Info($"App filter applied: {value}");

            }, nameof(TC_019_App_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_App_Clear")]
        [AllureTag("Regression")]
        public async Task TC_020_App_Clear()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                await _homePage.SelectAppAsync();

                await _homePage.ClearDropdownFilterAsync();

                await _homePage.Verify_clear_filter();

            }, nameof(TC_020_App_Clear));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_App_Data_Verification")]
        [AllureTag("Regression")]
        public async Task TC_021_App_Data_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var value = await _homePage.SelectAppAsync();

                await _homePage.VerifyColumnFilterAsync(4, value);

                Logger.Info("App data verified");

            }, nameof(TC_021_App_Data_Verification));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_022_Date_Filter")]
        public async Task TC_022_Date_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var (start, end) = await _homePage.SelectLast7DaysRangeAsync();

                // await _homePage.VerifyDateRangeAsync(start, end);

                Logger.Info($"Date filter applied from {start} to {end}");

            }, nameof(TC_022_Date_Filter));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_023_Date_Filter_Verification")]
        public async Task TC_023_Date_Filter_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                var (start, end) = await _homePage.SelectLast7DaysRangeAsync();

                var rows = Page.Locator("table tbody tr");

                int count = await rows.CountAsync();

                for (int i = 0; i < count; i++)
                {
                    var text = await rows.Nth(i).Locator("td").Nth(0).InnerTextAsync();

                    DateTime date = DateTime.Parse(text.Split('\n')[0]);

                    Assert.True(date >= start && date <= end);
                }

                Logger.Info("Date filter data verified");

            }, nameof(TC_023_Date_Filter_Verification));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_024_Clear_Date_Filter")]
        public async Task TC_024_Clear_Date_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.OpenAuditLogsAsync();

                await _homePage.SelectLast7DaysRangeAsync();

                await _homePage.Verify_clear_filter();

                var rows = Page.Locator("table tbody tr");

                int count = await rows.CountAsync();

                Assert.True(count > 1);

                Logger.Info("Date filter cleared successfully");

            }, nameof(TC_024_Clear_Date_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_Navigate_WiseBoard")]
        [AllureTag("Regression")]
        public async Task TC_025_Navigate_WiseBoard()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.VerifyHomePageLoadedAsync();
                await Assertions.Expect(Page.GetByText(new Regex(@"Welcome"))).ToBeVisibleAsync();
                await _homePage.Clickwiseborad();
                await _setup.WaitForDashboardStableAsync();
                await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-board"));

                Logger.Info("TC_25: wise board page loaded successfully.");
            }, nameof(TC_025_Navigate_WiseBoard));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_026_Navigate_WiseAction")]
        [AllureTag("Regression")]
        public async Task TC_026_Navigate_WiseAction()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _homePage.VerifyHomePageLoadedAsync();
                await Assertions.Expect(Page.GetByText(new Regex(@"Welcome"))).ToBeVisibleAsync();
                await _homePage.ClickWiseAction();
                await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-actions"));

                Logger.Info("TC_26: wise Action page loaded successfully.");
            }, nameof(TC_026_Navigate_WiseAction));
        }

         
    }
}                  