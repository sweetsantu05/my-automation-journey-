using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseAgents;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseAgents
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Agents Page Tests")]
    public class WiseAgentsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseAgentsPage _wiseAgentsPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseAgentsPage = new WiseAgentsPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseAgents_Should_Load_And_Display_All_Agents")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseAgents_Should_Load_And_Display_All_Agents()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseAgentsPage.OpenAsync();
                await _wiseAgentsPage.VerifyWiseAgnet();

                Logger.Info("TC_WISEAGENT_01: Wise Agents page loaded and all agents displayed successfully.");
            }, nameof(TC_001_WiseAgents_Should_Load_And_Display_All_Agents));
        }
    }
}
