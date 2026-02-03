using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseBoard
{
    public class WiseBoardPage : BasicSetup
    {
        public WiseBoardPage(IPage page) : base(page) { }

        private ILocator WiseBoardCard =>
            Page.GetByText("Wise Board", new() { Exact = true });

        public async Task OpenAsync()
        {
            await WiseBoardCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-board"));
            await WaitForDashboardStableAsync();
        }
    }           
}
