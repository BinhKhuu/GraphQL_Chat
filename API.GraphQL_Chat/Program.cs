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
        policy.WithOrigins("http://localhost:4200")
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


app.Run();
