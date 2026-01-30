using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseAI;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseAI
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise AI Page Tests")]
    public class WiseAIPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseAIPage _wiseAIPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseAIPage = new WiseAIPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEAI_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseAI_Should_Load_With_All_Core_Sections()
        {
            await _wiseAIPage.OpenAsync();
            await _wiseAIPage.VerifyAppTabsAsync();
            await _wiseAIPage.VerifySearchAsync();
            await _wiseAIPage.VerifyPopularQueriesAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEAI_01_Core_Sections_Validated");

            Logger.Info("TC_WISEAI_01: Wise AI page loaded with all core sections successfully.");
        }
    }
}
