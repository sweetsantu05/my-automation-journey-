using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Pages.WiseActions;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseActions
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Actions Page Tests")]
    public class WiseActionsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseActionsPage _wiseActionsPage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseActionsPage = new WiseActionsPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_WISEACTION_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task WiseActions_Should_Load_And_Action_Modal_Should_Work()
        {
            await _wiseActionsPage.OpenAsync();
            await _basicSetup.VerifyDashboardTabsAsync();
            await _basicSetup.VerifyDashboardFiltersAsync();
            await _basicSetup.WaitForWiseCardsToLoadAsync();
            await _basicSetup.VerifyWiseCardsAsync();
            await _wiseActionsPage.OpenActionModalAsync();
            await _wiseActionsPage.VerifyActionButtonsAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_WISEACTION_01_Action_Modal_Validated");

            Logger.Info("TC_WISEACTION_01: Wise Actions page and action modal validated successfully.");
        }
    }
}
