using Microsoft.Playwright;
using WiseUltimaTests.Pages.Home;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;


namespace WiseUltimaTests.Tests.Home
{
    [Collection("Playwright collection")]
    [AllureSuite("Home Page Tests")]
    public class HomePageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private HomePage _homePage = null!;
        private BasicSetup _setup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _attachmentHelper = new AttachmentHelper(Context);

            _loginPage = new LoginPage(Page);
            _homePage = new HomePage(Page);
            _setup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();

            await _setup.WaitForPageAsync(3);
        }

         
    }
}                  