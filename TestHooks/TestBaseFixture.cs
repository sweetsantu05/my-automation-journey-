using Microsoft.Playwright;
using WiseUltimaTests.Utils;

namespace WiseUltimaTests.TestHooks
{
    public abstract class TestBaseFixture :IClassFixture<GlobalTestFixture>, IAsyncLifetime
    {
        private IPlaywright _playwright = null!;
        private IBrowser _browser = null!;

        protected IBrowserContext Context = null!;
        protected IPage Page = null!;
        protected AttachmentHelper _attachmentHelper = null!;

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();

            _browser = await _playwright.Chromium.LaunchAsync(new()
            {
                // Headless = false   
            });

            Context = await _browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            _attachmentHelper = new AttachmentHelper(Context);
    
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
