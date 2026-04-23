using WiseUltimaTests.Pages.Login;
using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using WiseUltimaTests.Pages.WiseActions;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;

namespace WiseUltimaTests.Tests.WiseActions
{
    [Collection("Playwright collection")]
    [AllureSuite("Wise Actions Page Tests")]
    public class WiseActionsPageTests : TestBaseFixture, IAsyncLifetime
    {
        private LoginPage _loginPage = null!;
        private WiseActionsPage _wiseActionsPage = null!;
        private BasicSetup _basicSetup = null!;

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseActionsPage = new WiseActionsPage(Page);
            _basicSetup = new BasicSetup(Page);

            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
            await _wiseActionsPage.OpenAsync();
            await _basicSetup.ClickRandomCriticalAppAsync();
        }
        
        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseActions_Should_Load_Current")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseActions_Should_Load_Current()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchBasedOnAppAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.VerifyActButton();
                Logger.Info("TC_WISEACTION_01: Wise Actions page and action modal validated successfully.");
            }, nameof(TC_001_WiseActions_Should_Load_Current));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_002_WiseActions_Should_Load_W_Pridict")]
        [AllureTag("Smoke")]
        public async Task TC_002_WiseActions_Should_Load_W_Pridict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.VerifyActButton();
                Logger.Info("TC_WISEACTION_02: Wise Actions page and action modal validated successfully.");
            }, nameof(TC_002_WiseActions_Should_Load_W_Pridict));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_003_WiseActions_Should_Load_M_Pridict")]
        [AllureTag("Smoke")]
        public async Task TC_003_WiseActions_Should_Load_M_Pridict()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.VerifyActButton();
                Logger.Info("TC_WISEACTION_03: Wise Actions page and action modal validated successfully.");
            }, nameof(TC_003_WiseActions_Should_Load_M_Pridict));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_Server_Current_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_004_Server_Current_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchBasedOnAppAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Server);
            }, nameof(TC_004_Server_Current_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_Server_WPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_005_Server_WPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Server);
            }, nameof(TC_005_Server_WPredict_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_Server_MPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_006_Server_MPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Server);
            }, nameof(TC_006_Server_MPredict_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_007_Storage_Current_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_007_Storage_Current_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchBasedOnAppAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Storage);
            }, nameof(TC_007_Storage_Current_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_008_Storage_WPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_008_Storage_WPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Storage);
            }, nameof(TC_008_Storage_WPredict_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_009_Storage_MPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_009_Storage_MPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Storage);
            }, nameof(TC_009_Storage_MPredict_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_010_Database_Current_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_010_Database_Current_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchBasedOnAppAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Database);
            }, nameof(TC_010_Database_Current_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_011_Database_WPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_011_Database_WPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToWPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Database);
            }, nameof(TC_011_Database_WPredict_Data_Validation));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_012_Database_MPredict_Data_Validation")]
        [AllureTag("Regression")]
        public async Task TC_012_Database_MPredict_Data_Validation()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _basicSetup.SwitchToMPredictAsync();
                await _basicSetup.WaitForDashboardStableAsync();
                await _wiseActionsPage.ValidateCardDataConsistencyAsync(ActionCardType.Database);
            }, nameof(TC_012_Database_MPredict_Data_Validation));
        }
    }
}
