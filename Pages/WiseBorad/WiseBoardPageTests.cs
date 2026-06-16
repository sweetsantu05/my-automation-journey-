using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Pages.WiseBoard;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseBoard
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Board Page Tests")]
    public class WiseBoardPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseBoardPage _wiseBoardPage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseBoardPage = new WiseBoardPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEBOARD_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseBoard_Should_Load_Current()
        {
            await _wiseBoardPage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _basicSetup.VerifyServerLoadedAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEBOARD_01_WiseBoard_Current_Page_Loaded");
            Logger.Info("TC_WISEBOARD_01: Wise Board Current Pridiction Page loaded with all sections successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEBOARD_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseBoard_Should_Load_W_Pridict()
        {
            await _wiseBoardPage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _basicSetup.VerifyServerLoadedAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEBOARD_02_WiseBoard_W-Pridict_Page_Loaded");
            Logger.Info("TC_WISEBOARD_02: Wise Board Week Pridiction Page loaded with all sections successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEBOARD_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseBoard_Should_Load_M_Pridict()
        {
            await _wiseBoardPage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _basicSetup.VerifyServerLoadedAsync();

<<<<<<< Updated upstream
            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEBOARD_03_WiseBoard_M-Pridict_Page_Loaded");
            Logger.Info("TC_WISEBOARD_03: Wise Board Month Pridiction Page loaded with all sections successfully.");
=======
                Logger.Info("TC_WISEBOARD_02: Wise Board Week Pridiction Page loaded with all sections successfully.");
            }, nameof(TC_002_WiseBoard_Should_Load_W_Pridict));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_WiseBoard_Should_Load_M_Pridict")]
        [AllureTag("Smoke")]
        public async Task TC_003_WiseBoard_Should_Load_M_Pridict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _basicSetup.VerifyServerLoadedAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();

                Logger.Info("TC_WISEBOARD_03: Wise Board Month Pridiction Page loaded with all sections successfully.");
            }, nameof(TC_003_WiseBoard_Should_Load_M_Pridict));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Verify_Current_Server_Status_Green")]
        [AllureTag("Regression")]
        public async Task TC_004_Verify_Current_Server_Status_Green()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Green);
                await Assertions.Expect(Page.GetByText("Green")).ToBeVisibleAsync();
            }, nameof(TC_004_Verify_Current_Server_Status_Green));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Verify_Current_Server_Status_Amber")]
        [AllureTag("Regression")]
        public async Task TC_005_Verify_Current_Server_Status_Amber()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Amber);
                await Assertions.Expect(Page.GetByText("Amber")).ToBeVisibleAsync();
            }, nameof(TC_005_Verify_Current_Server_Status_Amber));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Verify_Current_Server_Status_Red")]
        [AllureTag("Regression")]
        public async Task TC_006_Verify_Current_Server_Status_Red()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Red);
                await Assertions.Expect(Page.GetByText("Red", new() { Exact = true })).ToBeVisibleAsync();
            }, nameof(TC_006_Verify_Current_Server_Status_Red));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Verify_W_Predict_Server_Status_Green")]
        [AllureTag("Regression")]
        public async Task TC_007_Verify_W_Predict_Server_Status_Green()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Green);
                await Assertions.Expect(Page.GetByText("Green")).ToBeVisibleAsync();
            }, nameof(TC_007_Verify_W_Predict_Server_Status_Green));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Verify_W_Predict_Server_Status_Amber")]
        [AllureTag("Regression")]
        public async Task TC_008_Verify_W_Predict_Server_Status_Amber()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Amber);
                await Assertions.Expect(Page.GetByText("Amber")).ToBeVisibleAsync();
            }, nameof(TC_008_Verify_W_Predict_Server_Status_Amber));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Verify_W_Predict_Server_Status_Red")]
        [AllureTag("Regression")]
        public async Task TC_009_Verify_W_Predict_Server_Status_Red()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Red);
                await Assertions.Expect(Page.GetByText("Red", new() { Exact = true })).ToBeVisibleAsync();
                 
            }, nameof(TC_009_Verify_W_Predict_Server_Status_Red));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Verify_M_Predict_Server_Status_Green")]
        [AllureTag("Regression")]
        public async Task TC_010_Verify_M_Predict_Server_Status_Green()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Green);
                await Assertions.Expect(Page.GetByText("Green")).ToBeVisibleAsync();

            }, nameof(TC_010_Verify_M_Predict_Server_Status_Green));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Verify_M_Predict_Server_Status_Amber")]
        [AllureTag("Regression")]
        public async Task TC_011_Verify_M_Predict_Server_Status_Amber()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Amber);
                await Assertions.Expect(Page.GetByText("Amber")).ToBeVisibleAsync();
            }, nameof(TC_011_Verify_M_Predict_Server_Status_Amber));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Verify_M_Predict_Server_Status_Red")]
        [AllureTag("Regression")]
        public async Task TC_012_Verify_M_Predict_Server_Status_Red()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateStatusAsync(StatusType.Red);
                await Assertions.Expect(Page.GetByText("Red", new() { Exact = true })).ToBeVisibleAsync();
            }, nameof(TC_012_Verify_M_Predict_Server_Status_Red));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_013_Verify_Current_Storage_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_013_Verify_Current_Storage_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Storage);
                await Assertions.Expect(Page.GetByText(new System.Text.RegularExpressions.Regex("^(Red|Green|Amber)$"))).ToBeVisibleAsync();
            }, nameof(TC_013_Verify_Current_Storage_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_014_Verify_W_Predict_Storage_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_014_Verify_W_Predict_Storage_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Storage);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_014_Verify_W_Predict_Storage_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_015_Verify_M_Predict_Storage_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_015_Verify_M_Predict_Storage_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Storage);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_015_Verify_M_Predict_Storage_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_016_Verify_Current_Database_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_016_Verify_Current_Database_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Database);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_016_Verify_Current_Database_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_017_Verify_W_Predict_Database_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_017_Verify_W_Predict_Database_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Database);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_017_Verify_W_Predict_Database_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_018_Verify_M_Predict_Database_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_018_Verify_M_Predict_Database_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Database);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_018_Verify_M_Predict_Database_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_019_Verify_Current_Network_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_019_Verify_Current_Network_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Network);
            }, nameof(TC_019_Verify_Current_Network_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_020_Verify_W_Predict_Network_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_020_Verify_W_Predict_Network_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" })).ToBeVisibleAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Network);
            }, nameof(TC_020_Verify_W_Predict_Network_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_021_Verify_M_Predict_Network_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_021_Verify_M_Predict_Network_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Network);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_021_Verify_M_Predict_Network_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_022_Verify_Current_Middleware_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_022_Verify_Current_Middleware_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Middleware);
            }, nameof(TC_022_Verify_Current_Middleware_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_023_Verify_W_Predict_Middleware_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_023_Verify_W_Predict_Middleware_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Middleware);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_023_Verify_W_Predict_Middleware_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_024_Verify_M_Predict_Middleware_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_024_Verify_M_Predict_Middleware_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Middleware);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_024_Verify_M_Predict_Middleware_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_025_Verify_Current_Backup_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_025_Verify_Current_Backup_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToCurrentAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Backup);
            }, nameof(TC_025_Verify_Current_Backup_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_026_Verify_W_Predict_Backup_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_026_Verify_W_Predict_Backup_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "W-Predict" })).ToBeVisibleAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Backup);
            }, nameof(TC_026_Verify_W_Predict_Backup_All_Status));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_027_Verify_M_Predict_Backup_All_Status")]
        [AllureTag("Regression")]
        public async Task TC_027_Verify_M_Predict_Backup_All_Status()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _wiseBoardPage.ValidateAllStatusesAsync(CardType.Backup);
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "M-Predict" })).ToBeVisibleAsync();
            }, nameof(TC_027_Verify_M_Predict_Backup_All_Status));
>>>>>>> Stashed changes
        }
    }
}
