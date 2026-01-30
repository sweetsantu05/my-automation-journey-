using Microsoft.Playwright;
using WiseUltimaTests.Pages.SignUp;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Tests.SignUp
{
    [Collection("Playwright collection")]
    [AllureSuite("Sign Up Page Tests")]
    public class SignUpPageTests : TestBaseFixture, IAsyncLifetime
    {
        private SignUpPage _signUpPage = null!;
        private BasicSetup _setup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _setup = new BasicSetup(Page);
            _signUpPage = new SignUpPage(Page);

            await _signUpPage.NavigateToSignUpPageAsync();
            await _setup.WaitForPageAsync(2);
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_01")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Load_Successfully()
        {
            await Assertions.Expect(Page.GetByText("Create Your Account",new(){Exact=false})).ToBeVisibleAsync();
            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_01_SignUp_Page_Loaded");
            Logger.Info("TC_SIGNUP_01: Sign Up page loaded successfully.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_UserName()
        {
            await _signUpPage.ValidateRegisterationEmptyUserName();

            await Assertions.Expect(Page.GetByText("Name is required", new() { Exact = false })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_02_Empty_Username");

            Logger.Info("TC_SIGNUP_02: Empty username validation successful.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Password()
        {
            await _signUpPage.ValidateRegisterationEmptyPassword();

            var requiredMessages = Page.GetByText("Required");

            var count = await requiredMessages.CountAsync();

            Assert.True(count >= 1);

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_03_Empty_Password");

            Logger.Info("TC_SIGNUP_03:Empty Password validiation successful.");
        }


        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_04")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Valid_Credentials()
        {
            await _signUpPage.ValidateRegisterationValidCredentials();

            await Page.WaitForURLAsync(
                "**/Account/RegisterConfirmation",
                new PageWaitForURLOptions { Timeout = 15000 });

            await Assertions.Expect(Page).ToHaveURLAsync(new Regex("RegisterConfirmation"));

            await Assertions.Expect(Page.GetByText("Registration Successful", new() { Exact = true })).ToBeVisibleAsync();

            await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Go to Login" })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page, "TC_SIGNUP_04_Valid_Credentials");

            Logger.Info("TC_SIGNUP_04: Registration successful with valid credentials.");


        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_05")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Duplicate_Email()
        {
            await _signUpPage.ValidateRegisterationDuplicateEmail();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("already registered", new() { Exact = false })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_05_Duplicate_Email");

            Logger.Info("TC_SIGNUP_05: Duplicate email validation successful.");

        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Mismatching_Password()
        {
            await _signUpPage.SignUpWithMismatchingPasswordsAsync();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("Passwords do not match", new() { Exact = false })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_06_Password_Mismatch");

            Logger.Info("TC_SIGNUP_06: Password mismatch validation successful.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Organization()
        {
            await _signUpPage.SignUpWithEmptyOrganizationAsync();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("select an organization", new() { Exact = false })).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_07_Empty_Organization");

            Logger.Info("TC_SIGNUP_07: Empty organization validation successful.");
        }

        [AllureSeverity(Allure.Net.Commons.SeverityLevel.critical)]
        [AllureOwner("TC_SIGNUP_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task SignUpPage_Should_Have_Clickable_SignUp_Button()
        {
            await _signUpPage.VerifySignUpButtonIsClickableAsync();

            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_08_SignUp_Button_Clickable");

            Logger.Info("TC_SIGNUP_08: Sign Up button is clickable.");
        }
    }
}

