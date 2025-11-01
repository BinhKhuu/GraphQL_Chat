using API.GraphQL_Chat.AppConfigurators;
using API.GraphQL_Chat.BuilderConfigurators;

var builder = WebApplication.CreateBuilder(args);

// you will need this if newtonsoft is used when configuration graphql in the builder configurator
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.AllowSynchronousIO = true;
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularLocalHost", policy =>
    {
        policy.WithOrigins("http://localhost:4200","https://cdn.jsdelivr.net")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

GraphQLBuilderConfigurator.Configure(builder);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AngularLocalHost");
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();

}

GraphQLAppConfigurator.Configure(app);

app.MapGet("/altair", async context =>
{
  var html = @"
<!DOCTYPE html>
<html><head>
  <meta charset=""utf-8"">
  <title>Altair</title>
  <base href=""//cdn.jsdelivr.net/npm/altair-static/build/dist/"">
  <meta name=""viewport"" content=""width=device-width,initial-scale=1"">
  <link rel=""icon"" type=""image/x-icon"" href=""//cdn.jsdelivr.net/npm/altair-static/build/dist/favicon.ico"">
  <link href=""//cdn.jsdelivr.net/npm/altair-static/build/dist/styles.css"" rel=""stylesheet"">
</head>

<body>
  <app-root>
    <style>
      .loading-screen {
        display: none;
      }

    </style>
    <div class=""loading-screen styled"">
      <div class=""loading-screen-inner"">
        <div class=""loading-screen-logo-container"">
          <img src=""assets/img/logo_350.svg"" alt=""Altair"">
        </div>
        <div class=""loading-screen-loading-indicator"">
          <span class=""loading-indicator-dot""></span>
          <span class=""loading-indicator-dot""></span>
          <span class=""loading-indicator-dot""></span>
        </div>
      </div>
    </div>
  </app-root>
  <script rel=""preload"" as=""script"" type=""text/javascript"" src=""//cdn.jsdelivr.net/npm/altair-static@4.0.2//build/dist/runtime.js""></script>
  <script rel=""preload"" as=""script"" type=""text/javascript"" src=""//cdn.jsdelivr.net/npm/altair-static@4.0.2//build/dist/polyfills.js""></script>
  <script rel=""preload"" as=""script"" type=""text/javascript"" src=""//cdn.jsdelivr.net/npm/altair-static@4.0.2//build/dist/main.js""></script>

  <script>
    function getSubscriptionsEndPoint() {
      let subscriptionsEndPoint = ""/graphql"";
      if (/^(?:[a-z]+:)?\/\//i.test(subscriptionsEndPoint)) {
        // if location includes protocol (e.g. ""wss://"") then return exact string
        return subscriptionsEndPoint;
      } else if (subscriptionsEndPoint[0] != '/') {
        // if location is relative (e.g. ""api"") then prepend host and current path
        let currentUrl = /^[^?]*(?=\/)/.exec(window.location.pathname);
        currentUrl = currentUrl ? currentUrl[0] : '';
        while (subscriptionsEndPoint.substring(0, 3) == '../') {
          subscriptionsEndPoint = subscriptionsEndPoint.substring(3);
          currentUrl = /^[^?]*(?=\/)/.exec(currentUrl);
          currentUrl = currentUrl ? currentUrl[0] : '';
        }
        return (window.location.protocol === ""http:"" ? ""ws://"" : ""wss://"") + window.location.host + currentUrl + '/' + subscriptionsEndPoint;
      }
      // if location is absolute (e.g. ""/api"") then prepend host only
      return (window.location.protocol === ""http:"" ? ""ws://"" : ""wss://"") + window.location.host + subscriptionsEndPoint;
    }

    var altairOptions = {
        endpointURL: ""/graphql"",
        subscriptionsEndpoint: getSubscriptionsEndPoint(),
        initialHeaders: {""Accept"":""application/json"",""Content-Type"":""application/json""},
        initialSubscriptionsPayload: null,
        initialSettings: null,
    };

    window.addEventListener(""load"", function() {
      AltairGraphQL.init(altairOptions);
    });
  </script>



</body></html>

";
  
  context.Response.ContentType = "text/html";
  await context.Response.WriteAsync(html);
});

app.Run();
