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

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseBoard_Should_Load_Current")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseBoard_Should_Load_Current()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseBoardPage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToCurrentAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _basicSetup.VerifyServerLoadedAsync();

                Logger.Info("TC_WISEBOARD_01: Wise Board Current Pridiction Page loaded with all sections successfully.");
            }, nameof(TC_001_WiseBoard_Should_Load_Current));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_WiseBoard_Should_Load_W_Pridict")]
        [AllureTag("Smoke")]
        public async Task TC_002_WiseBoard_Should_Load_W_Pridict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseBoardPage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _basicSetup.VerifyServerLoadedAsync();

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
                await _wiseBoardPage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _basicSetup.VerifyServerLoadedAsync();

                Logger.Info("TC_WISEBOARD_03: Wise Board Month Pridiction Page loaded with all sections successfully.");
            }, nameof(TC_003_WiseBoard_Should_Load_M_Pridict));
        }
    }
}
