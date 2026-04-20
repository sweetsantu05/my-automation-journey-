using Microsoft.Playwright;
using WiseUltimaTests.Pages.PreRequisites;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace WiseUltimaTests.Pages.WiseAI
{
    public class WiseAIPage : BasicSetup
    {
        public WiseAIPage(IPage page) : base(page) { }

        private ILocator WiseAICard =>
            Page.GetByRole(AriaRole.Link, new() { Name = "Wise AI" });

        private ILocator AiMessageBubble =>
         Page.Locator("div.ai-message-bubble");

        private ILocator MessageBubbles =>
            Page.Locator("div.message-bubble");

        private ILocator ChatView =>
            Page.Locator("div.professional-chat-view");

        private ILocator RightSidebar =>
            Page.Locator("div.right-sidebar");
        
        public ILocator AddContextButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Add context" });

        private ILocator AddMenuOption =>
            Page.GetByText("Choose System");

        private ILocator ActiveSystemItem =>
            Page.GetByText("ww-ultima-chat-vm");

        private ILocator SearchBox =>
            Page.Locator(".search-input-row input, .search-input-row textarea, .search-input-row [contenteditable='true']");
        private ILocator AiInputS =>
            Page.Locator(".mud-input.mud-input-outlined input, .mud-input.mud-input-outlined textarea");
        private static readonly Random _random = new();
        // private ILocator RightPanelPopularQueries =>
        // Page.Locator(".right-sidebar >> div")
        //     .Filter(new() { HasTextRegex = new Regex(@".+\?") });
        

        private ILocator RightPanelPopularQueries =>
    Page.Locator(".right-sidebar")
        .Locator(".sidebar-section")
        .Filter(new LocatorFilterOptions { HasText = "POPULAR QUERIES" })
        .Locator(".sidebar-list")
        .Locator(".popular-query-item");

        public async Task OpenAsync()
        {
            await NavMenuToggleButton();
            await WiseAICard.ClickAsync();
            await Assertions.Expect(Page).ToHaveURLAsync(new Regex(".*/new-chat"));
        }

        public async Task SearchInputRow()
        {
            await Page.Locator(".search-input-row").ClickAsync();
        }

        private ILocator DropdownItems =>
            Page.Locator(".dropdown-item");

        public async Task ClickRandomPopularQueryAsync()
        {
            await SearchInputRow();

            await Assertions.Expect(DropdownItems.First).ToBeVisibleAsync(new() { Timeout = 5000 });

            int count = await DropdownItems.CountAsync();

            Assert.True(count >= 6, $"Expected at least 6 dropdown items, but found {count}");

            int randomIndex = new Random().Next(0, count);

            var selectedText = await DropdownItems.Nth(randomIndex).InnerTextAsync();

            await DropdownItems.Nth(randomIndex).ClickAsync();

        }

        public async Task VerifyAiResponse()
        {
            await Assertions.Expect(ChatView).ToBeVisibleAsync(new() { Timeout = 30000 });

            await Assertions.Expect(RightSidebar).ToBeVisibleAsync();
            
            var userBubble = MessageBubbles.First;
            await Assertions.Expect(userBubble).ToBeVisibleAsync();

            await Assertions.Expect(AiMessageBubble).ToBeVisibleAsync(new() { Timeout = 30000 });

            try
            {
                await Assertions.Expect(AiMessageBubble).Not.ToHaveTextAsync("",new() { Timeout = 20000 });
            }
            catch (PlaywrightException)
            {
                throw new Xunit.Sdk.XunitException("AI is taking more than the average response time (Avg time: 30 seconds).");
            }

            var aiText = await AiMessageBubble.InnerTextAsync();

            Assert.False(string.IsNullOrWhiteSpace(aiText),"AI response bubble is visible but text is empty.");
        }

        private readonly List<string> _questions = new()
        {
            "Check CPU usage of the system",
            "Show memory usage details",
            "Check disk storage usage",
            "Show recent logs",
            "What is the system health status?",
            "Show top processes consuming CPU"
        };


        public async Task SelectVMAndAskRandomQuestionAsync()
        {
            await Assertions.Expect(AddContextButton).ToBeVisibleAsync();
            await AddContextButton.ClickAsync();

            await Assertions.Expect(AddMenuOption).ToBeVisibleAsync();
            await AddMenuOption.ClickAsync();

            await Assertions.Expect(ActiveSystemItem)
                .ToBeVisibleAsync(new() { Timeout = 5000 });
            await ActiveSystemItem.ClickAsync();
            
            var random = new Random();
            int index = random.Next(_questions.Count);
            string question = _questions[index];
            await Assertions.Expect(SearchBox).ToBeVisibleAsync();
            await SearchBox.ClickAsync();
            await SearchBox.FillAsync(question);
            await SearchBox.PressAsync("Enter");
        }

        public async Task MultipleQuestionWithValidation(int numberOfQuestions = 3)
        {
            for (int i = 0; i < numberOfQuestions; i++)
            {
            var random = new Random();
            string question = _questions[random.Next(_questions.Count)];

            int previousCount = await AiMessageBubble.CountAsync();

            await Assertions.Expect(AiInputS).ToBeVisibleAsync();
            await AiInputS.ClickAsync();
            await AiInputS.FillAsync(question);
            await AiInputS.PressAsync("Enter");
            await Page.WaitForFunctionAsync(
                @"(prev) => document.querySelectorAll('.ai-message-bubble').length > prev",
                previousCount,
                new() { Timeout = 30000 }
            );

            var latestResponse = AiMessageBubble.Last;

            await Assertions.Expect(latestResponse).ToBeVisibleAsync();

            await Assertions.Expect(latestResponse)
                .Not.ToHaveTextAsync("", new() { Timeout = 30000 });

            var text = await latestResponse.InnerTextAsync();

            Assert.False(string.IsNullOrWhiteSpace(text),
                "AI response is empty after waiting.");

            Assert.False(string.IsNullOrWhiteSpace(text),
                $"AI response is empty for question: {question}");
            }
        }

        public async Task ClickRandomPopularQueryWithValidationAsync()
        {
            await SearchInputRow();

            await Assertions.Expect(DropdownItems.First)
                .ToBeVisibleAsync(new() { Timeout = 5000 });

            int count = await DropdownItems.CountAsync();
            Assert.True(count >= 1, "No popular queries found.");

            int previousCount = await AiMessageBubble.CountAsync();

            int randomIndex = _random.Next(count);

            var selectedText = await DropdownItems.Nth(randomIndex).InnerTextAsync();

            await DropdownItems.Nth(randomIndex).ClickAsync();

            await Page.WaitForFunctionAsync(
                @"(prev) => document.querySelectorAll('.ai-message-bubble').length > prev",
                previousCount,
                new() { Timeout = 30000 }
            );

            var latestResponse = AiMessageBubble.Last;

            await Assertions.Expect(latestResponse).ToBeVisibleAsync();

            await Assertions.Expect(latestResponse)
                .ToHaveTextAsync(new Regex(@"\S"), new() { Timeout = 30000 });

            var text = await latestResponse.InnerTextAsync();
        }

        public async Task MultiplePopularQueriesWithValidationAsync(int numberOfQueries = 3)
        {
            for (int i = 0; i < numberOfQueries; i++)
            {
                await ClickRandomPopularQueryWithValidationAsync();
            }
        }

        public async Task MultiplePopularQueriesFromSidebarAsync(int count = 3)
        {
            for (int i = 0; i < count; i++)
            {
                await Assertions.Expect(RightPanelPopularQueries.First)
                    .ToBeVisibleAsync(new() { Timeout = 5000 });

                int total = await RightPanelPopularQueries.CountAsync();

                Assert.True(total > 0, "No popular queries found");

                int randomIndex = _random.Next(total);

                var selected = RightPanelPopularQueries.Nth(randomIndex);

                await selected.ScrollIntoViewIfNeededAsync();

                var text = await selected.InnerTextAsync();

                int previousCount = await AiMessageBubble.CountAsync();

                await selected.ClickAsync();


                await Page.WaitForFunctionAsync(
                    @"(prev) => document.querySelectorAll('.ai-message-bubble').length > prev",
                    previousCount,
                    new() { Timeout = 30000 }
                );

                var latest = AiMessageBubble.Last;

                await Assertions.Expect(latest).ToBeVisibleAsync();

                await Assertions.Expect(latest)
                    .ToHaveTextAsync(new Regex(@"\S"), new() { Timeout = 30000 });

            }
        }
    }
}
