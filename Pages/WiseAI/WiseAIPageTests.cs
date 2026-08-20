using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseAI;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;
using Microsoft.Playwright;

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
            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _wiseAIPage = new WiseAIPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseAI_Should_Load_With_All_Core_Sections")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseAI_Should_Load_With_All_Core_Sections()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();
                await _wiseAIPage.SearchInputRow();
                await Assertions.Expect(Page.GetByText("Popular Queries", new() { Exact = true })).ToBeVisibleAsync();

                Logger.Info("TC_WISEAI_01: Wise AI page loaded with all core sections successfully.");
            }, nameof(TC_001_WiseAI_Should_Load_With_All_Core_Sections));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_WiseAI_Should_Respond_When_Popular_Query_Is_Selected")]
        [AllureTag("Regression")]
        public async Task TC_002_WiseAI_Should_Respond_When_Popular_Query_Is_Selected()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();
                await _wiseAIPage.ClickRandomPopularQueryAsync();
                await _wiseAIPage.VerifyAiResponse();
                await Assertions.Expect(Page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.circle-icon-button")).ToBeVisibleAsync();

                Logger.Info("TC_WISEAI_02: AI responded successfully after selecting a popular query.");
            }, nameof(TC_002_WiseAI_Should_Respond_When_Popular_Query_Is_Selected));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_WiseAI_Should_Handle_Multiple_Popular_Queries")]
        [AllureTag("Regression")]
        public async Task TC_003_WiseAI_Should_Handle_Multiple_Popular_Queries()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();
                await _wiseAIPage.ClickRandomPopularQueryAsync();
                await _wiseAIPage.VerifyAiResponse();

                await _wiseAIPage.MultiplePopularQueriesFromSidebarAsync(3);
                await Assertions.Expect(Page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.circle-icon-button")).ToBeVisibleAsync();


                Logger.Info("TC_003: Multiple popular queries handled successfully.");
            }, nameof(TC_003_WiseAI_Should_Handle_Multiple_Popular_Queries));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_VM_Context_Should_Respond_To_Query")]
        [AllureTag("Regression")]
        public async Task TC_004_VM_Context_Should_Respond_To_Query()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();
                await _wiseAIPage.SelectVMAndAskRandomQuestionAsync();
                await _wiseAIPage.VerifyAiResponse();
                await Assertions.Expect(Page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.circle-icon-button")).ToBeEnabledAsync();

                Logger.Info("TC_004: AI responded correctly after selecting VM and asking question.");
            }, nameof(TC_004_VM_Context_Should_Respond_To_Query));
        }
    
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_VM_Context_Should_Handle_Multiple_Queries")]
        [AllureTag("Regression")]
        public async Task TC_005_VM_Context_Should_Handle_Multiple_Queries()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();

                await _wiseAIPage.SelectVMAndAskRandomQuestionAsync();
                await _wiseAIPage.VerifyAiResponse();

                await _wiseAIPage.MultipleQuestionWithValidation(3);

                Logger.Info("TC_005: AI handled multiple queries successfully.");
            }, nameof(TC_005_VM_Context_Should_Handle_Multiple_Queries));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_006_VM_Should_Be_Selected_Before_Query")]
        [AllureTag("Regression")]
        public async Task TC_006_VM_Should_Be_Selected_Before_Query()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAIPage.OpenAsync();

                await Assertions.Expect(_wiseAIPage.AddContextButton)
                    .ToBeVisibleAsync();

                await _wiseAIPage.SelectVMAndAskRandomQuestionAsync();

                await _wiseAIPage.VerifyAiResponse();
                await Assertions.Expect(Page.Locator(".mud-button-root.mud-icon-button.mud-ripple.mud-ripple-icon.circle-icon-button")).ToBeEnabledAsync();


                Logger.Info("TC_006: VM selection and query flow verified.");
            }, nameof(TC_006_VM_Should_Be_Selected_Before_Query));
        }
    }
}
