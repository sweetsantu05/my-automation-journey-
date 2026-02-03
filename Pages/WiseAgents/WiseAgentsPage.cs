using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseAgents
{
    public class WiseAgentsPage : BasicSetup
    {
        public WiseAgentsPage(IPage page) : base(page) { }

        private ILocator WiseAgentsCard =>
            Page.GetByText("Wise Agents", new() { Exact = true });

        private ILocator HealthMonitorCard =>
            Page.GetByText("Health Monitor", new() { Exact = true });

        private ILocator PerformanceOptimizerCard =>
            Page.GetByText("Performance Optimizer", new() { Exact = true });

        private ILocator SecuritySentinelCard =>
            Page.GetByText("Security Sentinel", new() { Exact = true });

        private ILocator CostControllerCard =>
            Page.GetByText("Cost Controller", new() { Exact = true });

        private ILocator ComplianceGuardianCard =>
            Page.GetByText("Compliance Guardian", new() { Exact = true });

        private ILocator ResourceManagerCard =>
            Page.GetByText("Resource Manager", new() { Exact = true });

        private ILocator BackupButlerCard =>
            Page.GetByText("Backup Butler", new() { Exact = true });

        public async Task OpenAsync()
        {
            await WiseAgentsCard.ClickAsync();
            await Assertions.Expect(Page)
                .ToHaveURLAsync(new Regex(".*/wise-agents"));
        }

        public async Task VerifyWiseAgnet()
        {
            await Assertions.Expect(WiseAgentsCard).ToBeVisibleAsync();
        
        }
    }
}
