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

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEBOARD_03_WiseBoard_M-Pridict_Page_Loaded");
            Logger.Info("TC_WISEBOARD_03: Wise Board Month Pridiction Page loaded with all sections successfully.");
        }
    }
}
