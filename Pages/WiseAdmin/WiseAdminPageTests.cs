using Microsoft.Playwright;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Pages.WiseAdmin;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseAdmin
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Admin Tests")]
    public class WiseAdminPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseAdminPage _wiseAdminPage = null!;
        private BasicSetup _setup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _wiseAdminPage = new WiseAdminPage(Page);
            _setup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
            await _setup.WaitForPageAsync(3);
        }


        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Navigate_WiseAdmin")]
        public async Task TC_002_Navigate_WiseAdmin()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAdminPage.NavigateToWiseAdminAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Current page" })).ToBeVisibleAsync();

                Logger.Info("TC_002: Navigated to Wise Admin page successfully.");

            }, nameof(TC_002_Navigate_WiseAdmin));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Verify_WiseAdmin_Loaded")]
        public async Task TC_002_Verify_WiseAdmin_Loaded()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAdminPage.NavigateToWiseAdminAsync();
                await _wiseAdminPage.VerifyWiseAdminPageLoadedAsync();
                await Assertions.Expect(Page.Locator(".mud-card-content").First).ToBeVisibleAsync();
                Logger.Info("TC_002: Wise Admin page loaded successfully.");

            }, nameof(TC_002_Verify_WiseAdmin_Loaded));
        }
    }
}