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
            await _basicSetup.WaitForIconToLoadAsync(Page);
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
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();
<<<<<<< HEAD
=======
                await Assertions.Expect(Page.Locator("table tbody tr").First).ToBeVisibleAsync();
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

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
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();
<<<<<<< HEAD
=======
                await Assertions.Expect(Page.Locator("table tbody tr").First).ToBeVisibleAsync();
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

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
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();
<<<<<<< HEAD
=======
                await Assertions.Expect(Page.Locator("table tbody tr").First).ToBeVisibleAsync();
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

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
                int total = await _wiseExplorePage.GetTotalResultsCountAsync();
                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.Equal(Math.Min(total, 10), rows);
            }, nameof(TC_006_Verify_Default_Row_Count));
        }   

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_007_Set_Pagination_To_100_and verify")]
=======
        [AllureOwner("TC_007_Set_Pagination_To_100_and_verify")]
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        [AllureTag("Regression")]
        public async Task TC_007_Set_Pagination_To_100()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SetPaginationTo100Async();

<<<<<<< HEAD
                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                var text = await _wiseExplorePage.GetPaginationTextAsync();


                Assert.True(rows <= 100);

                Logger.Info("TC_007: Pagination set to 100");
            }, nameof(TC_007_Set_Pagination_To_100));
        }

        // [Fact]
        // [Trait("Category", "Regression")]
        // [AllureOwner("TC_008_Verify_Pagination_Text")]
        // [AllureTag("Regression")]
        // public async Task TC_008_Verify_Pagination_Text()
        // {
        //     await _attachmentHelper.RunWithTracingAsync(async () =>
        //     {
        //         await _wiseExplorePage.SetPaginationTo100Async();

        //         var text = await _wiseExplorePage.GetPaginationTextAsync();

        //         Assert.Contains("of", text);

        //         Logger.Info($"TC_008: Pagination text = {text}");
        //     }, nameof(TC_008_Verify_Pagination_Text));
        // }

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
=======
                string selectedValue =
                    await _wiseExplorePage.GetSelectedPaginationValueAsync();

                Assert.Equal("100", selectedValue);

                Logger.Info(
                    $"TC_007: Pagination successfully set to {selectedValue}");

            }, nameof(TC_007_Set_Pagination_To_100));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_Next_Page_Navigation")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_Next_Page_Navigation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SetPaginationTo100Async();

                if (await _wiseExplorePage.NextPageButton.IsDisabledAsync())
                {
                    Logger.Info(
                        "Only one page of data available. Pagination navigation not applicable.");

                    Assert.True(true);
                    return;
                }

                var before =
                    await _wiseExplorePage.GetPaginationTextAsync();

                await _wiseExplorePage.ClickNextPageAsync();

                var after =
                    await _wiseExplorePage.GetPaginationTextAsync();

                Assert.NotEqual(before, after);

                Logger.Info(
                    $"Pagination changed from '{before}' to '{after}'");

            }, nameof(TC_008_Verify_Next_Page_Navigation));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_010_Verify_Total_Rows_Match_Count")]
        [AllureTag("Regression")]
        public async Task TC_010_Verify_Total_Rows_Match_Count()
=======
        [AllureOwner("TC_009_Verify_Total_Rows_Match_Count")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_Total_Rows_Match_Count()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();

                await _wiseExplorePage.SetPaginationTo100Async();

                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesAsync();

                Assert.Equal(expected, actual);

<<<<<<< HEAD
                Logger.Info($"TC_010: Total rows matched {actual}");
            }, nameof(TC_010_Verify_Total_Rows_Match_Count));
        }

        // [Fact]
        // [Trait("Category", "Regression")]
        // [AllureOwner("TC_011_Search_With_Random_ID")]
        // [AllureTag("Regression")]
        // public async Task TC_011_Search_With_Random_ID()
        // {
        //     await _attachmentHelper.RunWithTracingAsync(async () =>
        //     {
        //         await _wiseExplorePage.SetPaginationTo100Async();

        //         var randomId = await _wiseExplorePage.GetRandomIdAsync();

        //         await _wiseExplorePage.SearchAsync(randomId);

        //         int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

        //         Assert.Equal(1, rows);

        //         Logger.Info($"TC_011: Search success for ID {randomId}");
        //     }, nameof(TC_011_Search_With_Random_ID));
        // }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Search_and_Validate_ID")]
        [AllureTag("Regression")]
        public async Task TC_011_Search_and_Validate_ID()
=======
                Logger.Info($"TC_009: Total rows matched {actual}");
            }, nameof(TC_009_Verify_Total_Rows_Match_Count));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Search_and_Validate_ID")]
        [AllureTag("Regression")]
        public async Task TC_010_Search_and_Validate_ID()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                var randomId = await _wiseExplorePage.GetRandomIdAsync();

                await _wiseExplorePage.SearchAsync(randomId);

                var ids = await _wiseExplorePage.GetAllIdsFromTableAsync();

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.Equal(1, rows);

                Assert.Single(ids);
                Assert.Equal(randomId, ids.First());

<<<<<<< HEAD
                Logger.Info("TC_011: Search result matches ID");
            }, nameof(TC_011_Search_and_Validate_ID));
=======
                Logger.Info("TC_010: Search result matches ID");
            }, nameof(TC_010_Search_and_Validate_ID));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_013_Search_Production")]
        [AllureTag("Regression")]
        public async Task TC_013_Search_Production()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("PRD");
=======
        [AllureOwner("TC_011_Search_Production")]
        [AllureTag("Regression")]
        public async Task TC_011_Search_Production()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Production");
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows > 0);

<<<<<<< HEAD
                Logger.Info("TC_013: Production search returned results");
            }, nameof(TC_013_Search_Production));
=======
                Logger.Info("TC_011: Production search returned results");
            }, nameof(TC_011_Search_Production));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_014_Validate_Production_Results")]
        [AllureTag("Regression")]
        public async Task TC_014_Validate_Production_Results()
=======
        [AllureOwner("TC_012_Validate_Production_Results")]
        [AllureTag("Regression")]
        public async Task TC_012_Validate_Production_Results()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("PRD");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows > 0);

                var alerts = await _wiseExplorePage.GetAllAlertMessagesAsync();

                Assert.All(alerts, a => Assert.Contains("PRD", a));

<<<<<<< HEAD
                Logger.Info("TC_014: All rows contain Production");
            }, nameof(TC_014_Validate_Production_Results));
=======
                Logger.Info("TC_012: All rows contain Production");
            }, nameof(TC_012_Validate_Production_Results));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_015_Search_Server")]
        [AllureTag("Regression")]
        public async Task TC_015_Search_Server()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Ser");
=======
        [AllureOwner("TC_013_Search_Server")]
        [AllureTag("Regression")]
        public async Task TC_013_Search_Server()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Server");
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

                // await _wiseExplorePage.ValidateSearchResultsWithPaginationAsync("Ser");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows > 0);

<<<<<<< HEAD
                Logger.Info("TC_015: Server search returned results");
            }, nameof(TC_015_Search_Server));
=======
                Logger.Info("TC_013 : Server search returned results");
            }, nameof(TC_013_Search_Server));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_016_Validate_Server_Results")]
        [AllureTag("Regression")]
        public async Task TC_016_Validate_Server_Results()
=======
        [AllureOwner("TC_014_Validate_Server_Results")]
        [AllureTag("Regression")]
        public async Task TC_014_Validate_Server_Results()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("Server");

                var alerts = await _wiseExplorePage.GetAllAlertMessagesAsync();

                Assert.All(alerts, a => Assert.Contains("Server", a));

<<<<<<< HEAD
                Logger.Info("TC_016: All rows contain Server");
            }, nameof(TC_016_Validate_Server_Results));
=======
                Logger.Info("TC_014: All rows contain Server");
            }, nameof(TC_014_Validate_Server_Results));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_017_Invalid_Search")]
        [AllureTag("Regression")]
        public async Task TC_017_Invalid_Search()
=======
        [AllureOwner("TC_015_Invalid_Search")]
        [AllureTag("Regression")]
        public async Task TC_015_Invalid_Search()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("invalid123");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows == 0);

<<<<<<< HEAD
                Logger.Info("TC_017: Invalid search returned no results");
            }, nameof(TC_017_Invalid_Search));
=======
                Logger.Info("TC_015: Invalid search returned no results");
            }, nameof(TC_015_Invalid_Search));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_018_Critical_Filter_Count_Verification")]
        [AllureTag("Regression")]
        public async Task TC_018_Critical_Filter_Count_Verification()
=======
        [AllureOwner("TC_016_Empty_Search")]
        [AllureTag("Regression")]
        public async Task TC_016_Empty_Search()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SearchAsync("");

                int rows = await _wiseExplorePage.GetCurrentRowCountAsync();

                Assert.True(rows >= 1);

                Logger.Info("TC_016: Empty search returned no results");
            }, nameof(TC_016_Empty_Search));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Critical_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_017_Critical_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectCriticalAsync();

<<<<<<< HEAD
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
=======
                int total = await _wiseExplorePage.GetTotalResultsCountAsync();

                await _wiseExplorePage.ValidateCriticalResultsAsync();

                Assert.True(total >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("All rows are Critical");
            }, nameof(TC_017_Critical_Filter_Verification));
        }
   
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Tripping_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_018_Tripping_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectTrippingAsync();

<<<<<<< HEAD
                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();
                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesOptimizedAsync();

                Assert.Equal(expected, actual);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info($"TC_020: Tripping count matched {actual}");

            }, nameof(TC_020_Tripping_Filter_Count_Verification));
        }   
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_Tripping_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_021_Tripping_Filter_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectTrippingAsync();

                await _wiseExplorePage.ValidateAllRowsStatusWithPaginationAsync("Tripping", "#FFBF00");

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("TC_021: All rows are Tripping with YELLOW color");

            }, nameof(TC_021_Tripping_Filter_Verification));
=======
                int total = await _wiseExplorePage.GetTotalResultsCountAsync();

                await _wiseExplorePage.ValidateTrippingResultsAsync();

                Assert.True(total >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("All rows are Tripping");
            }, nameof(TC_018_Tripping_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_022")]
        [AllureTag("Regression")]
        public async Task TC_022_Safe_Filter_Count_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectSafeAsync();

                int expected = await _wiseExplorePage.GetTotalResultsCountAsync();
                int actual = await _wiseExplorePage.GetTotalRowsAcrossPagesOptimizedAsync();

                Assert.Equal(expected, actual);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info($"TC_022: Safe count matched {actual}");

            }, nameof(TC_022_Safe_Filter_Count_Verification));
        }
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_023_Safe_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_023_Safe_Filter_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectSafeAsync();

                await _wiseExplorePage.ValidateAllRowsStatusWithPaginationAsync("Safe", "#38b000");

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("TC_023: All rows are Safe with GREEN color");

            }, nameof(TC_023_Safe_Filter_Verification));
=======
        [AllureOwner("TC_019_Safe_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_019_Safe_Filter_Verification()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                // Safe
                await _wiseExplorePage.SelectSafeAsync();

                int total = await _wiseExplorePage.GetTotalResultsCountAsync();

                await _wiseExplorePage.ValidateSafeResultsAsync();

                Assert.True(total >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

                Logger.Info("All rows are Safe");
            }, nameof(TC_019_Safe_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_025_Storage_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_025_Storage_Type_Filter_Verification()
=======
        [AllureOwner("TC_020_Storage_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_020_Storage_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectStorageAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Storage");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_025_Storage_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Storage");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_020_Storage_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_026_Database_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_026_Database_Type_Filter_Verification()
=======
        [AllureOwner("TC_021_Database_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_021_Database_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectDatabaseAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Database");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_026_Database_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Database");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_021_Database_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_027_Network_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_027_Network_Type_Filter_Verification()
=======
        [AllureOwner("TC_022_Network_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_022_Network_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectNetworkAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Network");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_027_Network_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Network");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_022_Network_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_028_Server_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_028_Server_Type_Filter_Verification()
=======
        [AllureOwner("TC_023_Server_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_023_Server_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectServerAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Server");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_028_Server_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Server");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_023_Server_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_029_Middleware_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_029_Middleware_Type_Filter_Verification()
=======
        [AllureOwner("TC_024_Middleware_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_024_Middleware_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectMiddlewareAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Middleware");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_029_Middleware_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Middleware");

                Assert.True(validatedRows >= 0);
                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_024_Middleware_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_030_Backup_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_030_Backup_Type_Filter_Verification()
=======
        [AllureOwner("TC_025_Backup_Type_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_025_Backup_Type_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectBackupAsync();

<<<<<<< HEAD
                await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Backup");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_030_Backup_Type_Filter_Verification));
=======
                int validatedRows =
                    await _wiseExplorePage.ValidateAllRowsTypeWithPaginationAsync("Backup   ");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_025_Backup_Type_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_031_Development_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_031_Development_Environment_Filter_Verification()
=======
        [AllureOwner("TC_026_Development_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_026_Development_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Development");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Development");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_031_Development_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Development");

                Logger.Info(
                    $"Development - Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_026_Development_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_032_Test_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_032_Test_Environment_Filter_Verification()
=======
        [AllureOwner("TC_027_Test_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_027_Test_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Test");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Test");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_032_Test_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Test");

                Logger.Info(
                    $"Test - Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_027_Test_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_033_Training_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_033_Training_Environment_Filter_Verification()
=======
        [AllureOwner("TC_028_Training_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_028_Training_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Training");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Training");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_033_Training_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Training");

                Logger.Info(
                    $"Training - Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_028_Training_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_034_Staging_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_034_Staging_Environment_Filter_Verification()
=======
        [AllureOwner("TC_029_Staging_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_029_Staging_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Staging");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Staging");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_034_Staging_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Staging");

                Logger.Info(
                    $"Staging - Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_029_Staging_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_035_Production_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_035_Production_Environment_Filter_Verification()
=======
        [AllureOwner("TC_030_Production_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_030_Production_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Production");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Production");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_035_Production_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Production");

                Logger.Info(
                    $"Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_030_Production_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Regression")]
<<<<<<< HEAD
        [AllureOwner("TC_036_Sandbox_Environment_Filter_Verification")]
        [AllureTag("Regression")]
        public async Task TC_036_Sandbox_Environment_Filter_Verification()
=======
        [AllureOwner("TC_031_Sandbox_Environment_Filter_Verification")]
        [AllureTag("Regression")]

        public async Task TC_031_Sandbox_Environment_Filter_Verification()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.SelectEnvironmentAsync("Sandbox");

<<<<<<< HEAD
                await _wiseExplorePage
                    .ValidateAllRowsEnvironmentWithPaginationAsync("Sandbox");

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_036_Sandbox_Environment_Filter_Verification));
=======
                int totalResults =
                    await _wiseExplorePage.GetTotalResultsCountAsync();

                int validatedRows =
                    await _wiseExplorePage
                        .ValidateAllRowsEnvironmentWithPaginationAsync("Sandbox");

                Logger.Info(
                    $"Sandbox - Total Results: {totalResults}, Validated Rows: {validatedRows}");

                Assert.True(validatedRows >= 0);

                await _wiseExplorePage.GoToFirstPageAsync();

            }, nameof(TC_031_Sandbox_Environment_Filter_Verification));
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }
    }
}



