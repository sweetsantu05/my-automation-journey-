using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;

namespace WiseUltimaTests.Pages.WiseAI
{
    public class WiseAIPage : BasicSetup
    {
        public WiseAIPage(IPage page) : base(page) { }

        private ILocator WiseAICard =>
            Page.GetByText("Wise AI", new() { Exact = true });

        private ILocator App1Tab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "App 1" });

        private ILocator App2Tab =>
            Page.GetByRole(AriaRole.Button, new() { Name = "App 2" });

        private ILocator SearchButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Search" });

        private ILocator PopularQueriesHeader =>
            Page.GetByText("Popular Queries", new() { Exact = true });

        private ILocator PopularQueryChips =>
            Page.Locator("div.popular-chip-container > div.popular-chip");

        private ILocator AiMessageBubble =>
         Page.Locator("div.ai-message-bubble");

        private ILocator MessageBubbles =>
            Page.Locator("div.message-bubble");

        private ILocator ChatView =>
            Page.Locator("div.professional-chat-view");

        private ILocator RightSidebar =>
            Page.Locator("div.right-sidebar");

        public async Task OpenAsync()
        {
            await WiseAICard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/wise-ai"));
        }

        public async Task VerifyAppTabsAsync()
        {
            await Assertions.Expect(App1Tab).ToBeVisibleAsync();
            await Assertions.Expect(App2Tab).ToBeVisibleAsync();
        }

        public async Task VerifySearchAsync()
        {
            await Assertions.Expect(SearchButton).ToBeVisibleAsync();
        }

        public async Task VerifyPopularQueriesAsync()
        {
            await Assertions.Expect(PopularQueriesHeader).ToBeVisibleAsync();
        }

        public async Task ClickRandomPopularQueryAsync()
        {
            await Assertions.Expect(PopularQueriesHeader)
                .ToBeVisibleAsync();

            int count = await PopularQueryChips.CountAsync();
            Assert.True(count == 6, $"Expected 6 popular queries, but found {count}");

            int randomIndex = new Random().Next(0, count);

            var selectedText =
                await PopularQueryChips.Nth(randomIndex).InnerTextAsync();

            await PopularQueryChips.Nth(randomIndex).ClickAsync();

        }

        public async Task VerifyAiResponse()
        {
            await Assertions.Expect(ChatView)
                .ToBeVisibleAsync(new() { Timeout = 30000 });

            await Assertions.Expect(RightSidebar)
                .ToBeVisibleAsync();
            
            var userBubble = MessageBubbles.First;
            await Assertions.Expect(userBubble).ToBeVisibleAsync();

            await Assertions.Expect(AiMessageBubble)
                .ToBeVisibleAsync(new() { Timeout = 30000 });

            try
            {
                await Assertions.Expect(AiMessageBubble).Not.ToHaveTextAsync("",new() { Timeout = 20000 });
            }
            catch (PlaywrightException)
            {
                throw new Xunit.Sdk.XunitException("AI is taking more than the average response time (Avg time: 20 seconds).");
            }

            var aiText = await AiMessageBubble.InnerTextAsync();

            Assert.False(string.IsNullOrWhiteSpace(aiText),"AI response bubble is visible but text is empty.");
        }
    }
}
