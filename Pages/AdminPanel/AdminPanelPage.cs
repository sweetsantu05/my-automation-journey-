using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.AdminPanel
{
    public class AdminPanelPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        public AdminPanelPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
        }

        private ILocator ProfileIcon =>
            _page.Locator("div:nth-child(2) > div > .mud-tooltip-root");

        private ILocator AdminPanelButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Admin Panel" });

        private ILocator AdminPanelHeader =>
            _page.GetByText("Infrastructure Tasks");

        private ILocator EmailTriggerButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Email Trigger" });

        private ILocator EmailPopup =>
            _page.GetByText("Email Trigger Management");

        private ILocator TriggerEmailButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Trigger Email" });

        private ILocator ClosePopupButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Close" });


        public async Task OpenAdminPanelAsync()
        {
            await _basicSetup.NavMenuToggleButton();
            await ProfileIcon.ClickAsync();
            // await AdminPanelButton.ClickAsync();
            
        }
        public async Task OpenAdminPanel()
        {
            await AdminPanelButton.ClickAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task VerifyAdminPanelLoadedAsync()
        {
            await Assertions.Expect(AdminPanelHeader)
                .ToBeVisibleAsync(new() { Timeout = 15000 });
        }

        public async Task OpenEmailTriggerPopupAsync()
        {
            await EmailTriggerButton.ClickAsync();

            await Assertions.Expect(EmailPopup)
                .ToBeVisibleAsync();
        }

        public async Task TriggerEmailAsync()
        {
            await TriggerEmailButton.ClickAsync();
        }

        public async Task CloseEmailPopupAsync()
        {
            await ClosePopupButton.ClickAsync();
        }
    }
}