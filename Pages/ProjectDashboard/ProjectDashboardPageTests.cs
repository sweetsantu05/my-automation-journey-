using Microsoft.Playwright;
using WiseUltimaTests.Pages.Dashboard;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.Dashboard
{
    [Collection("Playwright collection")]
    [AllureSuite("Project Dashboard Tests")]
    public class ProjectDashboardTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private BasicSetup _setup = null!;
        private ProjectDashboardPage _dashboardPage = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _setup = new BasicSetup(Page);
            _loginPage = new LoginPage(Page);
            _dashboardPage = new ProjectDashboardPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_DASHBOARD_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Project_Dashboard_Should_Load_With_All_Core_Sections()
        {
            await _dashboardPage.WaitForDashboardToLoadAsync();

            await Assertions.Expect(
                Page.GetByText("Wise Board", new() { Exact = true })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByText("Wise Actions", new() { Exact = true })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByText("Wise AI", new() { Exact = true })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByText("Wise Explore", new() { Exact = true })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByText("Wise Agents", new() { Exact = true })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_DASHBOARD_01_Dashboard_Core_Sections_Validated"
            );

            Logger.Info(
                "TC_DASHBOARD_01: Project dashboard loaded and all core sections are visible."
            );
        }
    }
}
