using Microsoft.Playwright;
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.Login
{
    [Collection("Playwright collection")]
    [AllureSuite("Login Page Tests")]
    public class LoginPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private BasicSetup _setup = null!;
        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _setup = new BasicSetup(Page);
            _loginPage = new LoginPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task LoginPage_Should_Load_Successfully()
        {
            await Assertions.Expect(Page.GetByText("Welcome Back!")).ToBeVisibleAsync(new(){Timeout=15000});

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_01_Login_Page_Loaded");

            Logger.Info("TC_LOGIN_01: Login page loaded successfully.");
        }

        
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task LoginPage_Should_Have_Clickable_Button()
        {
            await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Sign In" })).ToBeEnabledAsync(new(){Timeout=10000});

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_02_SignIn_Button_Clickable");

            Logger.Info("TC_LOGIN_02: Sign In button is clickable.");
        }

        
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_UserName_Field()
        {
            await _loginPage.ValidateEmptyUserName();

            await Assertions.Expect(Page.GetByText("Email is required")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_03_Empty_Username");

            Logger.Info("TC_LOGIN_03: Empty username validation successful.");
        }

       
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_Password_Field()
        {
            await _loginPage.ValidateEmptyPassword();

            await Assertions.Expect(Page.GetByText("Required")).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_04_Empty_Password");

            Logger.Info("TC_LOGIN_04: Empty password validation successful.");
        }

        
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_05")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Invalid_Credentials()
        {
            await _loginPage.ValidateInvalidLogin();

            await Assertions.Expect(Page.GetByText("Invalid email or password. Please check your credentials and try again", new() { Exact = false })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_05_Invalid_Credentials");

            Logger.Info("TC_LOGIN_05: Invalid credentials validation successful.");
        }

       
        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Valid_Login()
        {
            await _loginPage.ValidateValidLogin();

            await Assertions.Expect(Page.Locator(".alert-filled-success")).ToHaveTextAsync("You have logged in successfully.");

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_LOGIN_06_Valid_Login");
            Logger.Info("TC_LOGIN_06: Valid login successful.");
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_SuperAdmin_Account()
        {
            await Assertions.Expect(Page).ToHaveURLAsync(WiseUltimaTests.Utils.ConfigReader.Get("LoginPageUrl"));

            await _loginPage.ValidateSuperAdminAccount(Page);

            await _setup.WaitForPageAsync(3);

            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Ultima");

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_06_Login_With_Valid_SuperAdmin_Successful");
            Logger.Info("TC_LOGIN_06: Login with valid Super Admin credentials successful.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_LOGIN_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Ultima_Admin_Account()
        {
            await _loginPage.ValidateUltimaAdminAccount(Page);

            await _setup.WaitForPageAsync(3);

            await Assertions.Expect(Page).ToHaveTitleAsync("Wise Ultima");

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_LOGIN_05_Login_With_Valid_MigrateAdmin_Successful");
            Logger.Info("TC_LOGIN_05: Login with valid Migrate Admincredentials successful.");
        }
    }
}
