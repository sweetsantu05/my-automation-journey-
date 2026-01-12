using Microsoft.Playwright;
using Xunit;
using WiseUltimaTests.Pages.SignUp;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Utils;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.SignUp
{
    [Collection("Playwright collection")]
    [AllureSuite("Sign Up Page Tests")]
    public class SignUpPageTests : IAsyncLifetime
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;
        private IPage _page = null!;
        private SignUpPage _signUpPage = null!;
        private BasicSetup _setup = null!;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(
                new() { Headless = false });

            _page = await _browser.NewPageAsync();
            _setup = new BasicSetup(_page);
            _signUpPage = new SignUpPage(_page);

            await _signUpPage.NavigateToSignUpPageAsync();
        }

        public async Task DisposeAsync()
        {
            await _page.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Load_Successfully()
        {
            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_01_SignUpPage_Loaded"
            );

            Logger.Info("TC_SIGNUP_01: Sign up page loaded successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Have_Clickable_SignUp_Button()
        {
            Assert.True(
                await _page.GetByRole(AriaRole.Button, new() { Name = "Sign Up" })
                    .IsEnabledAsync()
            );

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_02_SignUp_Button_Clickable"
            );

            Logger.Info("TC_SIGNUP_02: Sign Up button is clickable.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_UserName()
        {
            await _signUpPage.ValidateEmptyUserName();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_03_Empty_Username"
            );

            Logger.Info("TC_SIGNUP_03: Empty username validation triggered.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_Password()
        {
            await _signUpPage.ValidateEmptyPassword();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_04_Empty_Password"
            );

            Logger.Info("TC_SIGNUP_04: Empty password validation triggered.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_05")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Duplicate_Email()
        {
            await _signUpPage.ValidateDuplicateEmail();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_05_Duplicate_Email"
            );

            Logger.Info("TC_SIGNUP_05: Duplicate email validation triggered.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Mismatching_Password()
        {
            await _signUpPage.ValidateMismatchingPassword();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_06_Mismatching_Password"
            );

            Logger.Info("TC_SIGNUP_06: Mismatching password validation triggered.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Empty_Organization()
        {
            await _signUpPage.ValidateEmptyOrganization();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_07_Empty_Organization"
            );

            Logger.Info("TC_SIGNUP_07: Empty organization validation triggered.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Valid_Registration()
        {
            await _signUpPage.ValidateValidRegistration();
            await _setup.WaitForPageAsync(3);

            await ScreenshotHelper.TakeScreenshotAsync(
                _page,
                "TC_SIGNUP_08_Valid_Registration"
            );

            Logger.Info("TC_SIGNUP_08: Valid registration completed successfully.");
        }
    }
}
