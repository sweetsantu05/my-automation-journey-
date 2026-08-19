<<<<<<< HEAD
using WiseUltimaTests.Pages.Login;
using WiseUltimaTests.Pages.WiseMonitor;
using WiseUltimaTests.TestHooks;
using WiseUltimaTests.Utils;
using Xunit;
using Allure.Xunit.Attributes;
=======
using Microsoft.Playwright;
using WiseUltimaTests.Utils;
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3

namespace WiseUltimaTests.Tests.WiseMonitor
{
<<<<<<< HEAD
    [Collection("Playwright collection")]
    [AllureSuite("Wise Monitor Page Tests")]
    public class WiseMonitorPageTests : TestBaseFixture, IAsyncLifetime
=======
    public abstract class TestBaseFixture :IClassFixture<GlobalTestFixture>, IAsyncLifetime
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
    {
        private LoginPage _loginPage = null!;
        private WiseMonitorPage _wiseMonitorPage = null!;

<<<<<<< HEAD
        public new async Task InitializeAsync()
=======
        protected IBrowserContext Context = null!;
        protected IPage Page = null!;
        protected AttachmentHelper _attachmentHelper = null!;

        public async Task InitializeAsync()
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        {
            await base.InitializeAsync();

            _loginPage = new LoginPage(Page);
            _wiseMonitorPage = new WiseMonitorPage(Page);

<<<<<<< HEAD
            await _loginPage.NavigateToLoginPageAsync();
            await _loginPage.ValidateValidLogin();
=======
            Context = await _browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            _attachmentHelper = new AttachmentHelper(Context);
    
>>>>>>> 8e891f61be0eeed0d65dc19d3026ad3bc393e6f3
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [AllureOwner("TC_001_WiseMonitor_Should_Load_And_Display_Monitoring_Dashboard")]
        [AllureTag("Smoke")]
        public async Task TC_001_WiseMonitor_Should_Load_And_Display_Monitoring_Dashboard()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                
                Logger.Info("TC_WISEMONITOR_01: Wise Monitor dashboard loaded with all metrics successfully.");
            }, nameof(TC_001_WiseMonitor_Should_Load_And_Display_Monitoring_Dashboard));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_002_WiseMonitor_Verify_1Hour_Timeline_Filter")]
        [AllureTag("Regression")]
        public async Task TC_002_WiseMonitor_Verify_1Hour_Timeline_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {

                const string timeline = "1H";
                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                await _wiseMonitorPage.SelectTimelineAsync(timeline);

                string cpuUtilization = await _wiseMonitorPage.GetCpuUtilizationAsync();
                string diskUtilization = await _wiseMonitorPage.GetDiskUtilizationAsync();
                Console.WriteLine();
                Console.WriteLine($"Timeline Selected : {timeline}");
                Console.WriteLine($"CPU Utilization   : {cpuUtilization}");
                Console.WriteLine($"Disk Utilization  : {diskUtilization}");
                Console.WriteLine();
                Assert.False(string.IsNullOrWhiteSpace(cpuUtilization),"CPU Utilization should not be empty after selecting the 1H timeline.");
            }, nameof(TC_002_WiseMonitor_Verify_1Hour_Timeline_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_003_WiseMonitor_Verify_12Hour_Timeline_Filter")]
        [AllureTag("Regression")]
        public async Task TC_003_WiseMonitor_Verify_12Hour_Timeline_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                const string timeline = "12H";

                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                await _wiseMonitorPage.SelectTimelineAsync(timeline);

                string cpuUtilization = await _wiseMonitorPage.GetCpuUtilizationAsync();
                string diskUtilization = await _wiseMonitorPage.GetDiskUtilizationAsync();

                Console.WriteLine();
                Console.WriteLine($"Timeline Selected : {timeline}");
                Console.WriteLine($"CPU Utilization   : {cpuUtilization}");
                Console.WriteLine($"Disk Utilization  : {diskUtilization}");
                Console.WriteLine();

                Assert.False(string.IsNullOrWhiteSpace(cpuUtilization),
                    "CPU Utilization should not be empty after selecting the 12H timeline.");
            },nameof(TC_003_WiseMonitor_Verify_12Hour_Timeline_Filter));
        }
        
        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_004_WiseMonitor_Verify_24Hour_Timeline_Filter")]
        [AllureTag("Regression")]
        public async Task TC_004_WiseMonitor_Verify_24Hour_Timeline_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                const string timeline = "24H";

                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                await _wiseMonitorPage.SelectTimelineAsync(timeline);

                string cpuUtilization = await _wiseMonitorPage.GetCpuUtilizationAsync();
                string diskUtilization = await _wiseMonitorPage.GetDiskUtilizationAsync();

                Console.WriteLine();
                Console.WriteLine($"Timeline Selected : {timeline}");
                Console.WriteLine($"CPU Utilization   : {cpuUtilization}");
                Console.WriteLine($"Disk Utilization  : {diskUtilization}");
                Console.WriteLine();

                Assert.False(string.IsNullOrWhiteSpace(cpuUtilization),
                    "CPU Utilization should not be empty after selecting the 24H timeline.");
            },nameof(TC_004_WiseMonitor_Verify_24Hour_Timeline_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_005_WiseMonitor_Verify_1Week_Timeline_Filter")]
        [AllureTag("Regression")]
        public async Task TC_005_WiseMonitor_Verify_1Week_Timeline_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                const string timeline = "1W";

                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                await _wiseMonitorPage.SelectTimelineAsync(timeline);

                string cpuUtilization = await _wiseMonitorPage.GetCpuUtilizationAsync();
                string diskUtilization = await _wiseMonitorPage.GetDiskUtilizationAsync();

                Console.WriteLine();
                Console.WriteLine($"Timeline Selected : {timeline}");
                Console.WriteLine($"CPU Utilization   : {cpuUtilization}");
                Console.WriteLine($"Disk Utilization  : {diskUtilization}");
                Console.WriteLine();

                Assert.False(string.IsNullOrWhiteSpace(cpuUtilization),
                    "CPU Utilization should not be empty after selecting the 1W timeline.");
            },nameof(TC_005_WiseMonitor_Verify_1Week_Timeline_Filter));
        }

        [Fact]
        [Trait("Category", "Regression")]
        [AllureOwner("TC_006_WiseMonitor_Verify_1Month_Timeline_Filter")]
        [AllureTag("Regression")]
        public async Task TC_006_WiseMonitor_Verify_1Month_Timeline_Filter()
        {
            await _attachmentHelper.RunWithTracingAsync(async () =>
            {
                const string timeline = "1M";

                await _wiseMonitorPage.OpenAsync();
                await _wiseMonitorPage.Verifyserver();
                await _wiseMonitorPage.SelectTimelineAsync(timeline);

                

                Console.WriteLine();
                Console.WriteLine($"Timeline Selected : {timeline}");
                Console.WriteLine($"CPU Utilization   : {cpuUtilization}");
                Console.WriteLine($"Disk Utilization  : {diskUtilization}");
                Console.WriteLine();

                Assert.False(string.IsNullOrWhiteSpace(cpuUtilization),
                    "CPU Utilization should not be empty after selecting the 1M timeline.");
            },nameof(TC_006_WiseMonitor_Verify_1Month_Timeline_Filter));
        }
    }
}
