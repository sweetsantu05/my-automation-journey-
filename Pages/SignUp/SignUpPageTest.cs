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
            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_SIGNUP_01_SignUp_Page_Loaded"
            );
        }

        [AllureOwner("TC_SIGNUP_02")]
        [Fact]
        public async Task Validate_Registeration_Empty_UserName()
        {
            await _signUpPage.ValidateRegisterationEmptyUserName();
            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_SIGNUP_02_Empty_Username"
            );
        }

        [AllureOwner("TC_SIGNUP_03")]
        [Fact]
        public async Task Validate_Registeration_Empty_Password()
        {
            await _signUpPage.ValidateRegisterationEmptyPassword();
            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_SIGNUP_03_Empty_Password"
            );
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
            new PageWaitForURLOptions
            {
                Timeout = 15000
            }
        );
            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex("RegisterConfirmation"));
                
            await Assertions.Expect(Page)
            .ToHaveURLAsync("https://dev.ultima.wisework.in/Account/RegisterConfirmation");

            await Assertions.Expect(
                Page.GetByText("Registration Successful", new() { Exact = true })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByText("Please check your email to verify your account", new() { Exact = false })
            ).ToBeVisibleAsync();

            await Assertions.Expect(
                Page.GetByRole(AriaRole.Button, new() { Name = "Go to Login" })
            ).ToBeVisibleAsync();

            await ScreenshotHelper.TakeScreenshotAsync(
                Page,
                "TC_SIGNUP_04_Registration_Successful"
            );

            Logger.Info("TC_SIGNUP_04: Registration completed successfully.");
        }
    }
}
