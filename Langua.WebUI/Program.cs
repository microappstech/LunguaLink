using Langua.WebUI.Pages;
//using Langua.WebUI.Pages.Account;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Langua.Api.Shared.ApiHelper;
using Langua.WebUI.Client.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Langua.ApiControllers.LanguaHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddCascadingAuthenticationState();
//builder.Services.AddScoped<IdentityUserAccessor>();
//builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>(serviceProvider =>
{
    return new SqlDataAccess(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddLocalization();
builder.Services.AddTransient(typeof(IRepositoryCrudBase<>), typeof(BaseRepositoryCrud<>));

builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 900 * 1024 * 1024;
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
    .AddApplicationPart(Assembly.Load(new AssemblyName("Langua.Account")));

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
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    }).AddJwtBearer(opts =>
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
    .AddIdentityCookies();
builder.Services.AddAuthorization();
builder.Services.AddScoped<LanguaService>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Langua.DataContext.Data.LanguaContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging(true);
});

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

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
//builder.Services.AddSingleton<Uri>();
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
//app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.UseStaticFiles();
app.UseSwagger();
app.UseRequestLocalization(option =>
{
    option.SetDefaultCulture("fr-FR");
    option.AddSupportedCultures(new[] { "fr-FR", "en-US" });
    option.AddSupportedUICultures(new[] { "fr-FR", "en-US" });
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
await Seeding.Initialize(app.Services.CreateScope().ServiceProvider);
//
//app.MapIdentityApi<ApplicationUser>();

//app.MapAdditionalIdentityEndpoints();
app.Run();
