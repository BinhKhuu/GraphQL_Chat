using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Tests.GraphQL_Chat.CustomWebApplicationFactory;

public class TestWebApplicationFactory : WebApplicationFactory<API.GraphQL_Chat.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {


    }
}