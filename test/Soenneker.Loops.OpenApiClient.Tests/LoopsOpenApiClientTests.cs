using Soenneker.Tests.HostedUnit;

namespace Soenneker.Loops.OpenApiClient.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class LoopsOpenApiClientTests : HostedUnitTest
{
    public LoopsOpenApiClientTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
