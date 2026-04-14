// using Microsoft.Playwright;
// using WiseUltimaTests.Pages.PreRequisites;
// using System.Text.RegularExpressions;

// namespace WiseUltimaTests.Pages.WiseBoard
// {
//     public class WiseBoardPage : BasicSetup
//     {
//         public WiseBoardPage(IPage page) : base(page) { }

//         private ILocator WiseBoardCard =>
//             Page.GetByText("Wise Board", new() { Exact = true });

//         public async Task OpenAsync()
//         {
//             await WiseBoardCard.ClickAsync();
//             await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-board"));
//             await WaitForDashboardStableAsync();
//         }
//     }           
// }


using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;


namespace WiseUltimaTests.Pages.WiseBoard
{
    public enum StatusType
    {
        Green,
        Amber,
        Red
    }
    public enum CardType
    {
        Server,
        Storage,
        Database,
        Network,
        Middleware,
        Backup
    }

    public class WiseBoardPage : BasicSetup
    {
        public WiseBoardPage(IPage page) : base(page) { }

        private ILocator WiseBoardCard =>
            Page.GetByRole(AriaRole.Link, new() { Name = "Wise Board" });

        public async Task OpenAsync()
        {
            // await NavMenuToggleButton();
            await WiseBoardCard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-board"));
            await WaitForDashboardStableAsync();
        }

        private ILocator ServerSection => Page.Locator("text=Server").Locator("..").Locator("..");
        private ILocator GreenChip => ServerSection.Locator(".mud-chip-color-success").First;
        private ILocator AmberChip => ServerSection.Locator(".mud-chip-color-warning").First;
        private ILocator RedChip => ServerSection.Locator(".mud-chip-color-error").First;
        private ILocator GreenCount => ServerSection.Locator(".mud-chip-color-success").Nth(1);
        private ILocator AmberCount => ServerSection.Locator(".mud-chip-color-warning").Nth(1);
        private ILocator RedCount => ServerSection.Locator(".mud-chip-color-error").Nth(1);

        private async Task<int> GetCountAsync(ILocator locator)
        {
            var text = await locator.InnerTextAsync();
            return int.Parse(text.Trim());
        }
        private ILocator IssueTotalCount =>
            Page.Locator("text=Total Instances").Locator("..").GetByText(new Regex(@"\d+"));

        public async Task ValidateStatusAsync(StatusType status)
        {
            ILocator chip;
            ILocator countLocator;

            switch (status)
            {
                case StatusType.Green:
                    chip = GreenChip;
                    countLocator = GreenCount;
                    break;

                case StatusType.Amber:
                    chip = AmberChip;
                    countLocator = AmberCount;
                    break;

                case StatusType.Red:
                    chip = RedChip;
                    countLocator = RedCount;
                    break;

                default:
                    throw new Exception("Invalid status");
            }

            int expectedCount = await GetCountAsync(countLocator);

            await chip.ClickAsync();

            await Page.WaitForSelectorAsync("text=Issue Details");

            var actualText = await IssueTotalCount.InnerTextAsync();
            int actualCount = int.Parse(Regex.Match(actualText, @"\d+").Value);

            Assert.Equal(expectedCount, actualCount);

            await Page.GetByRole(AriaRole.Button, new() { Name = "close" }).ClickAsync();
        } 

        private ILocator GetCardSection(CardType cardType)
        {
            return Page
                .GetByText(cardType.ToString(), new() { Exact = true })
                .Locator("..")   // go up
                .Locator("..");  // reach card container
        }

        private ILocator GetStatusChip(CardType card, StatusType status, int index)
        {
            string className = status switch
            {
                StatusType.Green => "mud-chip-color-success",
                StatusType.Amber => "mud-chip-color-warning",
                StatusType.Red => "mud-chip-color-error",
                _ => throw new Exception("Invalid status")
            };

            return GetCardSection(card).Locator($".{className}").Nth(index);
        }

        public async Task ValidateCardStatusAsync(CardType card, StatusType status)
        {
            var chip = GetStatusChip(card, status, 0);
            var countLocator = GetStatusChip(card, status, 1);

            int expectedCount = await GetCountAsync(countLocator);

            await chip.ClickAsync();

            await Page.WaitForSelectorAsync("text=Issue Details");

            var actualText = await IssueTotalCount.InnerTextAsync();
            int actualCount = int.Parse(Regex.Match(actualText, @"\d+").Value);

            Assert.Equal(expectedCount, actualCount);

            await Page.GetByRole(AriaRole.Button, new() { Name = "close" }).ClickAsync();
        }

        public async Task ValidateAllStatusesAsync(CardType card)
        {
            await ValidateCardStatusAsync(card, StatusType.Green);
            await ValidateCardStatusAsync(card, StatusType.Amber);
            await ValidateCardStatusAsync(card, StatusType.Red);
        }
    }           
}
