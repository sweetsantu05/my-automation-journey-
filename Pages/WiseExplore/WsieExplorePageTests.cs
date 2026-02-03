
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
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_Currtet()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToCurrentAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEEXPLORE_01_System_List_Validated");
            Logger.Info("TC_WISEEXPLORE_01: Wise Explore page loaded and system data displayed successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_W_Pridict()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToWPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEEXPLORE_02_System_List_Validated");
            Logger.Info("TC_WISEEXPLORE_02: Wise Explore page loaded and system data displayed successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEEXPLORE_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseExplore_Should_Load_M_Priict()
        {
            await _wiseExplorePage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
            await _basicSetup.SwitchToMPredictAsync();
            await _basicSetup.WaitForDashboardStableAsync();
            await _wiseExplorePage.VerifyAtLeastOneResultAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEEXPLORE_03_System_List_Validated");
            Logger.Info("TC_WISEEXPLORE_03: Wise Explore page loaded and system data displayed successfully.");
        }
    }
}
