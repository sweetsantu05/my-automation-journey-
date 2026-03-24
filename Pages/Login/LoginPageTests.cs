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
            _attachmentHelper = new AttachmentHelper(Context);
            // await base.InitializeAsync();
            _setup = new BasicSetup(Page);
            _loginPage = new LoginPage(Page);

            await _loginPage.NavigateToLoginPageAsync();
        }

        
        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_LoginPage_Should_Load_Successfully")]
        [AllureTag("Smoke")]
        public async Task TC_001_LoginPage_Should_Load_Successfully()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.GetByText("Welcome Back!")).ToBeVisibleAsync(new(){Timeout=15000});

                Logger.Info("TC_LOGIN_01: Login page loaded successfully.");
            }, nameof(TC_001_LoginPage_Should_Load_Successfully));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_LoginPage_Should_Have_Clickable_Button")]
        [AllureTag("Smoke")]
        public async Task TC_002_LoginPage_Should_Have_Clickable_Button()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Sign In" })).ToBeEnabledAsync(new(){Timeout=10000});

                Logger.Info("TC_LOGIN_02: Sign In button is clickable.");
            }, nameof(TC_002_LoginPage_Should_Have_Clickable_Button));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_Validate_Empty_UserName_Field")]
        [AllureTag("Smoke")]
        public async Task TC_003_Validate_Empty_UserName_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateEmptyUserName();

                await Assertions.Expect(Page.GetByText("Email is required")).ToBeVisibleAsync();

                Logger.Info("TC_LOGIN_03: Empty username validation successful.");
            }, nameof(TC_003_Validate_Empty_UserName_Field));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_004_Validate_Empty_Password_Field")]
        [AllureTag("Smoke")]
        public async Task TC_004_Validate_Empty_Password_Field()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateEmptyPassword();

                await Assertions.Expect(Page.GetByText("Required")).ToBeVisibleAsync();

                Logger.Info("TC_LOGIN_04: Empty password validation successful.");
            }, nameof(TC_004_Validate_Empty_Password_Field));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_005_Validate_Invalid_Credentials")]
        [AllureTag("Smoke")]
        public async Task TC_005_Validate_Invalid_Credentials()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateInvalidLogin();

                await Assertions.Expect(Page.GetByText("Invalid email or password. Please check your credentials and try again", new() { Exact = false })).ToBeVisibleAsync();

                Logger.Info("TC_LOGIN_05: Invalid credentials validation successful.");
            }, nameof(TC_005_Validate_Invalid_Credentials));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_006_Validate_Valid_Login")]
        [AllureTag("Smoke")]
        public async Task TC_006_Validate_Valid_Login()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateValidLogin();

                await Assertions.Expect(Page.Locator("div").Filter(new() { HasText = "You have logged in" }).Nth(2)).ToBeVisibleAsync();

                Logger.Info("TC_LOGIN_06: Valid login successful.");
            }, nameof(TC_006_Validate_Valid_Login));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_007_Validate_SuperAdmin_Account")]
        [AllureTag("Smoke")]
        public async Task TC_007_Validate_SuperAdmin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await Assertions.Expect(Page).ToHaveURLAsync(WiseUltimaTests.Utils.ConfigReader.Get("LoginPageUrl"));

                await _loginPage.ValidateSuperAdminAccount(Page);

                await _setup.WaitForPageAsync(3);

                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Ultima");

                Logger.Info("TC_LOGIN_06: Login with valid Super Admin credentials successful.");
            }, nameof(TC_007_Validate_SuperAdmin_Account));
        }


        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_008_Validate_Ultima_Admin_Account")]
        [AllureTag("Smoke")]
        public async Task TC_008_Validate_Ultima_Admin_Account()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _loginPage.ValidateUltimaAdminAccount(Page);

                await _setup.WaitForPageAsync(3);

                await Assertions.Expect(Page).ToHaveTitleAsync("Wise Ultima");

                Logger.Info("TC_LOGIN_05: Login with valid Migrate Admincredentials successful.");
            }, nameof(TC_008_Validate_Ultima_Admin_Account));
        }
    }
}
