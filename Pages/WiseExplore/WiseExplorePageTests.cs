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

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseExplore_Should_Load_Current")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseExplore_Should_Load_Current()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToCurrentAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info(" TC_WISEEXPLORE_01: Wise Explore Current data validated");
            }, nameof(TC_001_WiseExplore_Should_Load_Current));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_WiseExplore_Should_Load_W_Predict")]
        [AllureTag("Smoke")]
        public async Task TC_002_WiseExplore_Should_Load_W_Predict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info("TC_WISEEXPLORE_02: Wise Explore W-Predict data validated");
            }, nameof(TC_002_WiseExplore_Should_Load_W_Predict));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_WiseExplore_Should_Load_M_Predict")]
        [AllureTag("Smoke")]
        public async Task TC_003_WiseExplore_Should_Load_M_Predict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseExplorePage.OpenAsync();
                await _basicSetup.ClickRandomCriticalAppAsync();
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseExplorePage.VerifyAtLeastOneResultAsync();

                Logger.Info("TC_WISEEXPLORE_03: Wise Explore M-Predict data validated");
            }, nameof(TC_003_WiseExplore_Should_Load_M_Predict));
        }
    }
}



