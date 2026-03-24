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
                await _wiseAIPage.VerifyPopularQueriesAsync();

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

                Logger.Info("TC_WISEAI_02: AI responded successfully after selecting a popular query.");
            }, nameof(TC_002_WiseAI_Should_Respond_When_Popular_Query_Is_Selected));
        }
    }
}
