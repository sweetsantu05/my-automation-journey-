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
        }

        // ========== SMOKE TESTS ==========

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_Current()
        {
            await _basicSetup.ClickRandomCriticalAppAsync();  // ✅ Now fixed
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_WISEEXPLORE_01_System_List_Validated");
            Logger.Info("✅ TC_WISEEXPLORE_01: Wise Explore Current data validated");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_W_Predict()
        {
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_WISEEXPLORE_02_System_List_Validated");
            Logger.Info("✅ TC_WISEEXPLORE_02: Wise Explore W-Predict data validated");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_M_Predict()
        {
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_WISEEXPLORE_03_System_List_Validated");
            Logger.Info("✅ TC_WISEEXPLORE_03: Wise Explore M-Predict data validated");
        }

        // ========== REGRESSION TESTS ==========

        [AllureOwner("TC_WISEEXPLORE_REG_04")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Page_Reload_Should_Work()
        {
            await _wiseExplorePage.OpenAsync();
            await Page.ReloadAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_05")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Current_Data_Persists_On_Refresh()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await Page.ReloadAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_06")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_WPredict_Data_Persists_On_Refresh()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await Page.ReloadAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_07")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_MPredict_Data_Persists_On_Refresh()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await Page.ReloadAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_08")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Switch_Current_To_WPredict()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_09")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Switch_WPredict_To_MPredict()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_10")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Switch_MPredict_To_Current()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_11")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Multiple_Environment_Switch_Stability()
        {
            await _wiseExplorePage.OpenAsync();

            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _basicSetup.SwitchToCurrentAsync();

            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }

        [AllureOwner("TC_WISEEXPLORE_REG_12")]
        [AllureTag("regression")]
        [Fact]
        public async Task WiseExplore_Data_Should_Always_Be_Present()
        {
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();
        }
    }
}



