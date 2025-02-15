using Langua.WebUI.Pages;
using Langua.WebUI.Data;
using Langua.DataContext.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Langua.Repositories.Interfaces;
using Langua.Repositories.Services;
using Langua.Account;
using System.Reflection;
using Langua.DAL;
using Microsoft.IdentityModel.Tokens;
using Langua.Models;
using System.Text;
using Langua.Api.Shared.ApiHelper;
using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Langua.ApiControllers.LanguaHub;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Langua.WebUI.Logging;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<IdentityUserAccessor>();
//builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, Langua.WebUI.Data.PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>(serviceProvider =>
{
    return new SqlDataAccess(builder.Configuration.GetConnectionString("sqlConnection"));
});
builder.Services.AddLocalization();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<MemoryCache>();
builder.Services.AddTransient(typeof(IRepositoryCrudBase<>), typeof(BaseRepositoryCrud<>));

builder.Services.AddServerSideBlazor()
.AddCircuitOptions(o =>
{
    o.DetailedErrors = true;
})
.AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 900 * 1024 * 1024;
    o.EnableDetailedErrors = true;
});

builder.Services.AddControllers()
    .AddOData(optio =>
    {
        var odataBuilder = new ODataConventionModelBuilder();
        optio.AddRouteComponents("odata/langua", odataBuilder.GetEdmModel())
        .Filter()
        .Select()
        .Expand().OrderBy().SetMaxTop(null)
        .Count();
    })
    .AddApplicationPart(Assembly.Load(new AssemblyName("Langua.Auth")));
builder.Services.AddControllers();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<BaseService>();
builder.Services.AddScoped(typeof(IGroupCandidateService<>), typeof(GroupCandidateService<>));
builder.Services.AddRadzenComponents();
builder.Services.AddLocalization();
builder.Services.AddScoped<SecurityService>();
builder.Services.AddScoped<ApiHelper>();
builder.Services.AddScoped<IMailService,MailService>();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

})
    .AddJwtBearer(opts =>
    {
        byte[] SigninKey = Encoding.ASCII.GetBytes(builder.Configuration["AuthSettings:Key"]);
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
            ValidAudience = builder.Configuration["AuthSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(SigninKey)
        };

    })
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Login";
        opt.LogoutPath = "/Logout";
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    })
    .AddIdentityCookies();

//builder.Services.AddRazorComponents();
builder.Services.AddAuthorization();
//builder.Services.AddScoped<AuthenticationStateProvider, Langua.WebUI.Client.CustomAuthenticationStateProvider>();
builder.Services.AddLogging(loggerbuilder =>
{
//    //this gonna remove any default provider (like add console log maded by ms)
    loggerbuilder.ClearProviders();
//    //
    loggerbuilder.AddProvider(new LanguaLoggerProvider(builder.Configuration,(cate)=>true));
//    //
    loggerbuilder.AddConsole();
//    //

});
builder.Services.AddScoped<LanguaService>();
var connectionString = builder.Configuration.GetConnectionString("sqlConnection") ?? throw new InvalidOperationException("Connection string 'sqlConnection' not found.");
builder.Services.AddDbContext<Langua.DataContext.Data.LanguaContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging(true);
}, ServiceLifetime.Singleton);

builder.Services.AddSignalR();


builder.Services.AddScoped<LangClientService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<Langua.DataContext.Data.LanguaContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    //app.U
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseAntiforgery();
app.UseStaticFiles();
app.UseSwagger();
//app.UseCookiePolicy();
app.UseRequestLocalization(option =>
{
    option.SetDefaultCulture("fr");
    option.AddSupportedCultures(new[] { "fr", "en", "ar" });
    option.AddSupportedUICultures(new[] { "fr", "en","ar" });
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});

app.MapRazorComponents<App>()  
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Langua.WebUI.Client._Imports).Assembly);
app.MapControllers();
app.MapHub<ChatHub>(ChatHub.ChatGroupEndPoint);
app.Use(async (context, next) =>
{
    var lang = "fr";
    if (!context.Request.Cookies.ContainsKey("LanguaLangue"))
    {
        context.Response.Cookies.Append("LanguaLangue", "fr", new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            HttpOnly = true,
            Secure = context.Request.IsHttps
        });
    }
    else
    {
        lang = context.Request.Cookies["LanguaLangue"];
    }
    try
    {
        if (!string.IsNullOrEmpty(lang))
        {
            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        else
        {
            throw new ArgumentException("Language cookie value is invalid.");
        }
    }
    catch (CultureNotFoundException ex)
    {
        Console.WriteLine($"Invalid culture found in cookie: {lang}. Error: {ex.Message}");
        var defaultCulture = new CultureInfo("fr");
        Thread.CurrentThread.CurrentCulture = defaultCulture;
        Thread.CurrentThread.CurrentUICulture = defaultCulture;
    }

    await next.Invoke();
});

await Seeding.Initialize(app.Services.CreateScope().ServiceProvider);
//

//app.MapIdentityApi<ApplicationUser>();
app.MapingOutEndpoint();
app.Run();
