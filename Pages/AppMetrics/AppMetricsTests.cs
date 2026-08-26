using Microsoft.Playwright;
using WiseUltimaTests.Pages.AppMetrics;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.AppMetrics
{
    [Collection("Playwright collection")]
    [AllureSuite("App Metrics Tests")]
    public class AppMetricsTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private AppMetricsPage _appMetricsPage = null!;
        private BasicSetup _setup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _appMetricsPage = new AppMetricsPage(Page);
            _setup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
            await _setup.WaitForPageAsync(3);
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Navigate_AppMetrics")]
        [AllureTag("Regression")]
        public async Task TC_001_Navigate_AppMetrics()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _appMetricsPage.NavigateToAppMetricsAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Link, new() { Name = "App Metrics" })).ToBeVisibleAsync();

                Logger.Info("TC_001: Navigated to App Metrics page successfully.");

            }, nameof(TC_001_Navigate_AppMetrics));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Verify_AppMetrics_Loaded")]
        [AllureTag("Regression")]
        public async Task TC_002_Verify_AppMetrics_Loaded()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _appMetricsPage.NavigateToAppMetricsAsync();
                await _appMetricsPage.VerifyAppMetricsLoadedAsync();
                await Assertions.Expect(Page.GetByText("Apps monitored:")).ToBeVisibleAsync();

                Logger.Info("TC_026: App Metrics page loaded successfully.");

            }, nameof(TC_002_Verify_AppMetrics_Loaded));
        }
    }
}