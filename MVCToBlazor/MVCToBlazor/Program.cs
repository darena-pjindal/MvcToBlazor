using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MVCToBlazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorComponents().AddServerComponents();
//builder.Services.AddScoped<AuthenticationStateProvider, MvcAuthenticationStateProvider>();

builder.Services.AddTransient<ForwardCookieHandler>();
//We could inject the services directly in the components, than an API layer. This will keep it docoupled and ensure that all business logic
//that exists in API layer are consistently applied. Need to evaluate if there are any performance gains of using services directly like in MVC
builder.Services.AddSingleton<TasksRepository>();
builder.Services.AddHttpClient<MyTasksClient>( c =>
{
    c.BaseAddress = new Uri("https://localhost:7201");
}).AddHttpMessageHandler<ForwardCookieHandler>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    //https://demo.duendesoftware.com/
    .AddOpenIdConnect(options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.ClientId = "interactive.public.short";
        options.ResponseType = "code"; // for code flow; or "id_token" for implicit flow
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.SaveTokens = true;

        options.MapInboundClaims = false;

        // Handle the token response
        options.Events = new OpenIdConnectEvents
        {
            OnTokenResponseReceived = context =>
            {
                // Handle the token response here if needed.
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//This was required after adding blazor
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapGet("api/tasks",[Authorize] async (TasksRepository tasksService) =>
    Results.Ok(await tasksService.GetMyTasks()));

app.MapPost("api/tasks", [Authorize] async (MyTask myTask, TasksRepository tasksService) =>
    await tasksService.AddTask(myTask));

app.Run();
