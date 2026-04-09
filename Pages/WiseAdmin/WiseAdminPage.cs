using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;

namespace WiseUltimaTests.Pages.WiseAdmin
{
    public class WiseAdminPage
    {
        private readonly IPage _page;
        private readonly BasicSetup _basicSetup;

        public WiseAdminPage(IPage page)
        {
            _page = page;
            _basicSetup = new BasicSetup(page);
        }

        private ILocator WiseAdminMenu =>
            _page.GetByRole(AriaRole.Link, new() { Name = "Wise Admin" });

        private ILocator PageHeader =>
            _page.GetByRole(AriaRole.Heading, new() { Name = "Manage Users" });

        private ILocator SearchBox =>
            _page.GetByPlaceholder("Search");

        private ILocator UserCards =>
            _page.Locator(".mud-card");

        private ILocator FirstUserCard =>
            _page.Locator(".mud-card").First;

        private ILocator EditButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First;

        private ILocator DeleteButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Delete" }).First;


        public async Task NavigateToWiseAdminAsync()
        {
            await _basicSetup.NavMenuToggleButton();
            await WiseAdminMenu.ClickAsync();
            await _basicSetup.WaitForPageAsync(2);
        }

        public async Task VerifyWiseAdminPageLoadedAsync()
        {
            await Assertions.Expect(PageHeader)
                .ToBeVisibleAsync(new() { Timeout = 15000 });

            await FirstUserCard.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 20000
            });

            var text = await FirstUserCard.InnerTextAsync();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new Exception("Wise Admin page not loaded properly - user card is empty");
            }
            await Assertions.Expect(EditButton).ToBeVisibleAsync();
            await Assertions.Expect(DeleteButton).ToBeVisibleAsync();
        }
    }
}