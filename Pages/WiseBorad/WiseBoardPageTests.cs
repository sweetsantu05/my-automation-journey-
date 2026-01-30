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
        public async Task WiseBoard_Should_Load_With_All_Sections()
        {
            await _wiseBoardPage.OpenAsync();
            await _basicSetup.VerifyDashboardTabsAsync();
            await _basicSetup.VerifyDashboardFiltersAsync();
            await _basicSetup.WaitForWiseCardsToLoadAsync();
            await _basicSetup.VerifyWiseCardsAsync();

            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_WISEBOARD_01_WiseBoard_Page_Loaded"
            );

            Logger.Info("TC_WISEBOARD_01: Wise Board page loaded with all sections successfully.");
        }
    }
}
