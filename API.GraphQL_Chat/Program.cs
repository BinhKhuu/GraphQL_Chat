using API.GraphQL_Chat.AppConfigurators;
using API.GraphQL_Chat.BuilderConfigurators;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

GraphQLBuilderConfigurator.Configure(builder);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

GraphQLAppConfigurator.Configure(app);


app.Run();
