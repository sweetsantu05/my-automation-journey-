using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseBoard
{
    public class WiseBoardPage : BasicSetup
    {
        public WiseBoardPage(IPage page) : base(page) { }

        private ILocator WiseBoardCard =>
            Page.GetByRole(AriaRole.Link, new() { Name = "Wise Board" });

        public async Task OpenAsync()
        {
            await NavMenuToggleButton();
            await WiseBoardCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-board"));
            await WaitForDashboardStableAsync();
        }
    }           
}
