// using Microsoft.Playwright;
// using WiseUltimaTests.TestHooks;
// using Xunit;

// namespace WiseUltimaTests.TestHooks
// {
//     public class TestBaseFixture : IClassFixture<GlobalTestFixture>, IAsyncLifetime
//     {
//         protected IBrowserContext Context { get; private set; } = null!;
//         protected IPage Page { get; private set; } = null!;

//         public async Task InitializeAsync()
//         {
//             Context = await GlobalTestFixture.Browser!.NewContextAsync();
//             Page = await Context.NewPageAsync();
//         }

//         public async Task DisposeAsync()
//         {
//             await Context.CloseAsync();
//         }
//     }
// }

using Microsoft.Playwright;

namespace WiseUltimaTests.TestHooks
{
    public abstract class TestBaseFixture : IAsyncLifetime
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;

        protected IBrowserContext Context = null!;
        protected IPage Page = null!;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new()
            {
                // Headless = false   
            });

            Context = await _browser.NewContextAsync();
            Page = await Context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            if (Page != null)
                await Page.CloseAsync();

            if (Context != null)
                await Context.CloseAsync();

            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
        }
    }
}
