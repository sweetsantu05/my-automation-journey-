using Microsoft.Playwright;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Pages.WiseExplore;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseExplore
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Explore Page Tests")]
    public class WiseExplorePageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseExplorePage _wiseExplorePage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseExplorePage = new WiseExplorePage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseExplore_Should_Load_Current")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseExplore_Should_Load_Current()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                
                await _basicSetup.SwitchToCurrentAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info(" TC_WISEEXPLORE_01: Wise Explore Current data validated");
            }, nameof(TC_001_WiseExplore_Should_Load_Current));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_WiseExplore_Should_Load_W_Predict")]
        [AllureTag("Smoke")]
        public async Task TC_002_WiseExplore_Should_Load_W_Predict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info("TC_WISEEXPLORE_02: Wise Explore W-Predict data validated");
            }, nameof(TC_002_WiseExplore_Should_Load_W_Predict));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_WiseExplore_Should_Load_M_Predict")]
        [AllureTag("Smoke")]
        public async Task TC_003_WiseExplore_Should_Load_M_Predict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info("TC_WISEEXPLORE_03: Wise Explore M-Predict data validated");
            }, nameof(TC_003_WiseExplore_Should_Load_M_Predict));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Verify_Total_Result_Count_Visible")]
        [AllureTag("Regression")]
        public async Task TC_004_Verify_Total_Result_Count_Visible()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var text = await _wiseExplorePage.ResultCountText.InnerTextAsync();

                Assert.Contains("Number of results", text);

                Logger.Info("TC_004: Total result count is visible");
            }, nameof(TC_004_Verify_Total_Result_Count_Visible));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Verify_Total_Count_Greater_Than_Zero")]
        [AllureTag("Regression")]
        public async Task TC_005_Verify_Total_Count_Greater_Than_Zero()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                int total = await _wiseExplorePage.GetTotalResultsCountAsync();

                Assert.True(total > 0);

                Logger.Info($"TC_005: Total count is {total}");
            }, nameof(TC_005_Verify_Total_Count_Greater_Than_Zero));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_Default_Row_Count")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_Default_Row_Count()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.Equal(10, rows);

                Logger.Info("TC_006: Default row count is 10");
            }, nameof(TC_006_Verify_Default_Row_Count));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Set_Pagination_To_100")]
        [AllureTag("Regression")]
        public async Task TC_007_Set_Pagination_To_100()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SetPaginationTo100Async();

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows <= 100);

                Logger.Info("TC_007: Pagination set to 100");
            }, nameof(TC_007_Set_Pagination_To_100));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_Pagination_Text")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_Pagination_Text()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SetPaginationTo100Async();

                var text = await _wiseExplorePage.GetPaginationTextAsync();

                Assert.Contains("of", text);

                Logger.Info($"TC_008: Pagination text = {text}");
            }, nameof(TC_008_Verify_Pagination_Text));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Verify_Next_Page_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_Next_Page_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var firstPageIds = await _wiseExplorePage.GetAllIdsFromTableAsync();

                await _wiseExplorePage.ClickNextPageAsync();

                var secondPageIds = await _wiseExplorePage.GetAllIdsFromTableAsync();

                Assert.NotEqual(firstPageIds.First(), secondPageIds.First());

                Logger.Info("TC_009: Pagination next page works");
            }, nameof(TC_009_Verify_Next_Page_Navigation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Verify_Total_Rows_Match_Count")]
        [AllureTag("Regression")]
        public async Task TC_010_Verify_Total_Rows_Match_Count()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();

                await _wiseExplorePage.SetPaginationTo100Async();

                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesAsync();

                Assert.Equal(expected, actual);

                Logger.Info($"TC_010: Total rows matched {actual}");
            }, nameof(TC_010_Verify_Total_Rows_Match_Count));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Search_With_Random_ID")]
        [AllureTag("Regression")]
        public async Task TC_011_Search_With_Random_ID()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SetPaginationTo100Async();

                var randomId = await _wiseExplorePage.GetRandomIdAsync();

                await _wiseExplorePage.SearchAsync(randomId);

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.Equal(1, rows);

                Logger.Info($"TC_011: Search success for ID {randomId}");
            }, nameof(TC_011_Search_With_Random_ID));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Validate_Search_Result_ID")]
        [AllureTag("Regression")]
        public async Task TC_012_Validate_Search_Result_ID()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var randomId = await _wiseExplorePage.GetRandomIdAsync();

                await _wiseExplorePage.SearchAsync(randomId);

                var ids = await _wiseExplorePage.GetAllIdsFromTableAsync();

                Assert.Single(ids);
                Assert.Equal(randomId, ids.First());

                Logger.Info("TC_012: Search result matches ID");
            }, nameof(TC_012_Validate_Search_Result_ID));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Search_Production")]
        [AllureTag("Regression")]
        public async Task TC_013_Search_Production()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Production");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows > 0);

                Logger.Info("TC_013: Production search returned results");
            }, nameof(TC_013_Search_Production));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Validate_Production_Results")]
        [AllureTag("Regression")]
        public async Task TC_014_Validate_Production_Results()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Production");

                var alerts = await _wiseExplorePage.GetAllAlertMessagesAsync();

                Assert.All(alerts, a => Assert.Contains("Production", a));

                Logger.Info("TC_014: All rows contain Production");
            }, nameof(TC_014_Validate_Production_Results));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Search_Server")]
        [AllureTag("Regression")]
        public async Task TC_015_Search_Server()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Server");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows > 0);

                Logger.Info("TC_015: Server search returned results");
            }, nameof(TC_015_Search_Server));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Validate_Server_Results")]
        [AllureTag("Regression")]
        public async Task TC_016_Validate_Server_Results()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Server");

                var alerts = await _wiseExplorePage.GetAllAlertMessagesAsync();

                Assert.All(alerts, a => Assert.Contains("Server", a));

                Logger.Info("TC_016: All rows contain Server");
            }, nameof(TC_016_Validate_Server_Results));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Invalid_Search")]
        [AllureTag("Regression")]
        public async Task TC_017_Invalid_Search()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("invalid123");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows == 0);

                Logger.Info("TC_017: Invalid search returned no results");
            }, nameof(TC_017_Invalid_Search));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Critical_Filter_Count_Verification")]
        [AllureTag("Regression")]
        public async Task TC_018_Critical_Filter_Count_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectCriticalAsync();

                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();
                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesOptimizedAsync();

                Assert.Equal(expected, actual);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info($"TC_018: Critical count matched {actual}");

            }, nameof(TC_018_Critical_Filter_Count_Verification));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Critical_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_019_Critical_Filter_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectCriticalAsync();

                await _wiseExplorePage.ValidateAllRowsStatusWithPaginationAsync("Critical", "#FF4040");

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("TC_019: All rows are Critical with RED color");

            }, nameof(TC_019_Critical_Filter_Verification));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Tripping_Filter_Count_Verification")]
        [AllureTag("Regression")]
        public async Task TC_020_Tripping_Filter_Count_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectTrippingAsync();

                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();
                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesOptimizedAsync();

                Assert.Equal(expected, actual);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info($"TC_020: Tripping count matched {actual}");

            }, nameof(TC_020_Tripping_Filter_Count_Verification));
        }   
        
    }
}



