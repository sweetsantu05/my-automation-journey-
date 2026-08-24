using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseAgents;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;
using WiseUltimaTests.Pages.PreRequisites;
using Microsoft.Playwright;

namespace WiseUltimaTests.Tests.WiseAgents
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Agents Page Tests")]
    public class WiseAgentsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseAgentsPage _wiseAgentsPage = null!;
        private BasicSetup _basicsetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseAgentsPage = new WiseAgentsPage(Page);
            _basicsetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

       
    }
}
