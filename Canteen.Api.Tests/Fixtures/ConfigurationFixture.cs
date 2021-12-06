using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Canteen.Api.Tests.Fixtures;

public class ConfigurationFixture
{
    public IConfiguration Configuration { get; }

    public ConfigurationFixture()
    {
        Configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            {"Jwt:Key", "BovinaCokanovicHansenKristensen"}
        }).Build();
    }
}