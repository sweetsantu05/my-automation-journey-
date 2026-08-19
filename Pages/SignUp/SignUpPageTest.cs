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

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_SignUpPage_Should_Load_Successfully")]
        [AllureTag("Smoke")]
        public async Task TC_001_SignUpPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.GetByText("Create Your Account",new(){Exact=false})).ToBeVisibleAsync();

                Logger.Info("TC_SIGNUP_01: Sign Up page loaded successfully.");
            }, nameof(TC_001_SignUpPage_Should_Load_Successfully));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_Validate_Registeration_Empty_UserName")]
        [AllureTag("Smoke")]
        public async Task TC_002_Validate_Registeration_Empty_UserName()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyUserName();

                await Assertions.Expect(Page.GetByText("Name is required", new() { Exact = false })).ToBeVisibleAsync();

                Logger.Info("TC_SIGNUP_02: Empty username validation successful.");
            }, nameof(TC_002_Validate_Registeration_Empty_UserName));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_Validate_Registeration_Empty_Password")]
        [AllureTag("Smoke")]
        public async Task TC_003_Validate_Registeration_Empty_Password()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationEmptyPassword();

                var requiredMessages = Page.GetByText("Required");

                var count = await requiredMessages.CountAsync();

                Assert.True(count>=1);

                Logger.Info("TC_SIGNUP_03:Empty Password validiation successful.");
            }, nameof(TC_003_Validate_Registeration_Empty_Password));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_004_Validate_Registeration_Valid_Credentials")]
        [AllureTag("Smoke")]
        public async Task TC_004_Validate_Registeration_Valid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationValidCredentials();

                await Page.WaitForURLAsync("**/Account/RegisterConfirmation",new PageWaitForURLOptions { Timeout = 15000 });

                await Assertions.Expect(Page).ToHaveURLAsync(new Regex("RegisterConfirmation"));

                await Assertions.Expect(Page.GetByText("Registration Successful", new() { Exact = true })).ToBeVisibleAsync();


                Logger.Info("TC_SIGNUP_04: Registration successful with valid credentials.");
            }, nameof(TC_004_Validate_Registeration_Valid_Credentials));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_005_Validate_Registeration_Duplicate_Email")]
        [AllureTag("Smoke")]
        public async Task TC_005_Validate_Registeration_Duplicate_Email()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.ValidateRegisterationDuplicateEmail();

                await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("already registered", new() { Exact = false })).ToBeVisibleAsync();

                Logger.Info("TC_SIGNUP_05: Duplicate email validation successful.");
            }, nameof(TC_005_Validate_Registeration_Duplicate_Email));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_006_Validate_Registeration_Mismatching_Password")]
        [AllureTag("Smoke")]
        public async Task TC_006_Validate_Registeration_Mismatching_Password()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.SignUpWithMismatchingPasswordsAsync();

                await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("Passwords do not match", new() { Exact = false })).ToBeVisibleAsync();

                Logger.Info("TC_SIGNUP_06: Password mismatch validation successful.");
            }, nameof(TC_006_Validate_Registeration_Mismatching_Password));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_007_Validate_Registeration_Empty_Organization")]
        [AllureTag("Smoke")]
        public async Task TC_007_Validate_Registeration_Empty_Organization()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.SignUpWithEmptyOrganizationAsync();

                await Assertions.Expect(Page.Locator(".mud-snackbar").GetByText("select an organization", new() { Exact = false })).ToBeVisibleAsync();

                Logger.Info("TC_SIGNUP_07: Empty organization validation successful.");
            }, nameof(TC_007_Validate_Registeration_Empty_Organization));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_008_SignUpPage_Should_Have_Clickable_SignUp_Button")]
        [AllureTag("Smoke")]
        public async Task TC_008_SignUpPage_Should_Have_Clickable_SignUp_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _signUpPage.VerifySignUpButtonIsClickableAsync();

                Logger.Info("TC_SIGNUP_08: Sign Up button is clickable.");
            }, nameof(TC_008_SignUpPage_Should_Have_Clickable_SignUp_Button));
        }
    }
}
