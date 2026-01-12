using Microsoft.Playwright;
using Xunit;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.Login
{
    [Collection("Playwright collection")]
    [AllureSuite("Login Page Tests")]
    public class LoginPageTests : IAsyncLifetime
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;
        private LoginPage _loginPage = null!;
        private BasicSetup _setup = null!;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(
                new() { Headless = false });

            _page = await _browser.NewPageAsync();
            _setup = new BasicSetup(_page);
            _loginPage = new LoginPage(_page);

            await _loginPage.NavigateToLoginPageAsync(_setup.LoginPageUrl);
        }

        public async Task DisposeAsync()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_01_LoginPage_Loads_Successfully()
        {
            await Assertions.Expect(
                _page.GetByText("Welcome Back!", new() { Exact = true })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_01_LoginPage_Loads_Successfully"
            );

            Logger.Info("TC_LOGIN_01: Login page loaded successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_02_Empty_Email_Shows_Validation_Message()
        {
            await _loginPage.LoginAsync("", "dummyPassword");

            Assert.True(await _loginPage.IsEmailRequiredDisplayedAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_02_Empty_Email_Shows_Validation_Message"
            );

            Logger.Info("TC_LOGIN_02: Empty email validation message displayed.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_03_Empty_Password_Shows_Validation_Message()
        {
            await _loginPage.LoginAsync("test@wisework.in", "");

            Assert.True(await _loginPage.IsPasswordRequiredDisplayedAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_03_Empty_Password_Shows_Validation_Message"
            );

            Logger.Info("TC_LOGIN_03: Empty password validation message displayed.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_04_Invalid_Credentials_Show_Error_Toast()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("invalid");

            await _loginPage.LoginAsync(user.Username, user.Password);

            Assert.True(await _loginPage.IsInvalidCredentialToastDisplayedAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_04_Invalid_Credentials_Show_Error_Toast"
            );

            Logger.Info("TC_LOGIN_04: Invalid credentials error toast displayed.");
        }
[AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
[AllureOwner("TC_LOGIN_05")]
[AllureTag("smoke")]
[Fact]
public async Task TC_LOGIN_05_Valid_Login_Successful()
{
    var user = _setup.defaultCredential;

    await _loginPage.LoginAsync(user.Username, user.Password);

    // ✅ Wait for post-login Welcome message
    var welcomeText = _page.GetByText("Welcome,", new() { Exact = false });
    await welcomeText.WaitForAsync();
    var wiseultima = _page.GetByText("wise ultima", new() { Exact = false }); await wiseultima.WaitForAsync();

    Assert.True(await welcomeText.IsVisibleAsync());

    await ScreenshotHelper.TakeScreenshotAsync(
        _page,
        "TC_LOGIN_05_Valid_Login_Successful"
    );

    Logger.Info("TC_LOGIN_05: Valid login successful.");
}



        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_06_LoginPage_Should_Have_Clickable_SignIn_Button()
        {
            Assert.True(await _loginPage.IsSignInButtonClickableAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_06_LoginPage_Should_Have_Clickable_SignIn_Button"
            );

            Logger.Info("TC_LOGIN_06: Sign In button is clickable.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_07_Login_With_Valid_MigrateAdmin()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("migrateadmin");

            await _loginPage.LoginAsMigrateAdminAsync(user.Username, user.Password);

            var welcomeText = _page.GetByText("Welcome", new() { Exact = false });
            await welcomeText.WaitForAsync();

            Assert.True(await welcomeText.IsVisibleAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_07_Login_With_Valid_MigrateAdmin"
            );

            Logger.Info("TC_LOGIN_07: Login with valid Migrate Admin credentials successful.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task TC_LOGIN_08_Login_With_Valid_SuperAdmin()
        {
            var user =
                WiseUltimaTests.Utils.ConfigReader.GetCredential("superadmin");

            await _loginPage.LoginAsSuperAdminAsync(user.Username, user.Password);

            var welcomeText = _page.GetByText("Welcome", new() { Exact = false });
            await welcomeText.WaitForAsync();

            Assert.True(await welcomeText.IsVisibleAsync());

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_LOGIN_08_Login_With_Valid_SuperAdmin"
            );

            Logger.Info("TC_LOGIN_08: Login with valid Super Admin credentials successful.");
        }

    }
}
