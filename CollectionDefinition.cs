using WiseUltimaTests.TestHooks;
using Xunit;

[CollectionDefinition("Playwright collection",DisableParallelization = false)]
public class PlaywrightCollection : ICollectionFixture<GlobalTestFixture>
{
    
}