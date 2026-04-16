using Microsoft.Playwright;
using WiseUltimaTests.Pages.AdminPanel;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.AdminPanel
{
    [Collection("Playwright collection")]
    [AllureSuite("Admin Panel Tests")]
    public class AdminPanelPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private AdminPanelPage _adminPage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _adminPage = new AdminPanelPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();

            await _basicSetup.WaitForPageAsync(3);
            await _adminPage.OpenAdminPanelAsync();
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_001_Open_AdminPanel")]
        public async Task TC_001_Open_AdminPanel()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _adminPage.OpenAdminPanel();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading).Filter(new() { HasText = "Welcome" })).ToBeVisibleAsync();

                Logger.Info("TC_001: Navigated to Admin Panel successfully.");

            }, nameof(TC_001_Open_AdminPanel));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_Verify_AdminPanel_Loaded")]
        public async Task TC_002_Verify_AdminPanel_Loaded()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _adminPage.OpenAdminPanel();
                await _adminPage.VerifyAdminPanelLoadedAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Heading).Filter(new() { HasText = "Welcome" })).ToBeVisibleAsync();


                Logger.Info("TC_002: Admin Panel loaded successfully.");

            }, nameof(TC_002_Verify_AdminPanel_Loaded));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_Open_Email_Trigger")]
        public async Task TC_003_Open_Email_Trigger()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _adminPage.OpenAdminPanel();
                await _adminPage.OpenEmailTriggerPopupAsync();

                Logger.Info("TC_003: Email Trigger popup opened successfully.");

            }, nameof(TC_003_Open_Email_Trigger));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Trigger_Email")]
        public async Task TC_004_Trigger_Email()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _adminPage.OpenAdminPanel();
                await _adminPage.OpenEmailTriggerPopupAsync();
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Trigger Email" })).ToBeVisibleAsync();

                Logger.Info("TC_004: Email trigger action executed successfully.");

            }, nameof(TC_004_Trigger_Email));
        }
    }
}