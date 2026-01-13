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
            await ScreenshotHelper.TakeScreenshotAsync(Page,"TC_SIGNUP_01_SignUp_Page_Loaded");
        }

        [AllureOwner("TC_SIGNUP_02")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_UserName()
        {
            await _signUpPage.ValidateRegisterationEmptyUserName();

            await Assertions.Expect(Page.GetByText("Name is required", new() { Exact = false })).ToBeVisibleAsync();
        }

        [AllureOwner("TC_SIGNUP_03")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Password()
        {
            await _signUpPage.ValidateRegisterationEmptyPassword();

            var requiredMessages = Page.GetByText("Required");

            var count = await requiredMessages.CountAsync();

            Assert.True(count >= 1);
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
        }

        [AllureOwner("TC_SIGNUP_06")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Duplicate_Email()
        {
            await _signUpPage.ValidateRegisterationDuplicateEmail();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("already registered", new() { Exact = false })).ToBeVisibleAsync();
        }

        [AllureOwner("TC_SIGNUP_07")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Mismatching_Password()
        {
            await _signUpPage.SignUpWithMismatchingPasswordsAsync();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("Passwords do not match", new() { Exact = false })).ToBeVisibleAsync();
        }

        [AllureOwner("TC_SIGNUP_08")]
        [AllureTag("smoke")]
        [Fact]
        public async Task Validate_Registeration_Empty_Organization()
        {
            await _signUpPage.SignUpWithEmptyOrganizationAsync();

            await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("select an organization", new() { Exact = false })).ToBeVisibleAsync();
        }
    }
}
