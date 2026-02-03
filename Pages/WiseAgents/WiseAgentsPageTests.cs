using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseAgents;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseAgents
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Agents Page Tests")]
    public class WiseAgentsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseAgentsPage _wiseAgentsPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseAgentsPage = new WiseAgentsPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEAGENT_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseAgents_Should_Load_And_Display_All_Agents()
        {
            await _wiseAgentsPage.OpenAsync();
            await _wiseAgentsPage.VerifyWiseAgnet();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEAGENT_01_All_Agents_Displayed");
            Logger.Info("TC_WISEAGENT_01: Wise Agents page loaded and all agents displayed successfully.");
        }
    }
}
