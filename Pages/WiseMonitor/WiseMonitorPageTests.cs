using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseMonitor;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseMonitor
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Monitor Page Tests")]
    public class WiseMonitorPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseMonitorPage _wiseMonitorPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseMonitorPage = new WiseMonitorPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEMONITOR_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseMonitor_Should_Load_And_Display_Monitoring_Dashboard()
        {
            await _wiseMonitorPage.OpenAsync();
            await _wiseMonitorPage.VerifyHeaderAsync();
            await _wiseMonitorPage.VerifyMetricCardsAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEMONITOR_01_Monitoring_Dashboard_Loaded");

            Logger.Info("TC_WISEMONITOR_01: Wise Monitor dashboard loaded with all metrics successfully.");
        }
    }
}
