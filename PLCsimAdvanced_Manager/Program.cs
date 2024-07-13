using System.Reflection;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Win32;
using MudBlazor;
using MudBlazor.Services;
using PLCsimAdvanced_Manager.Services;
using PLCsimAdvanced_Manager.Services.Logger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBootstrapBlazor();
builder.Services.AddMatBlazor();
builder.Services.AddMudServices(config =>
    {
        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
        config.SnackbarConfiguration.NewestOnTop = false;
        config.SnackbarConfiguration.ShowCloseIcon = true;
        config.SnackbarConfiguration.VisibleStateDuration = 10000;
        config.SnackbarConfiguration.HideTransitionDuration = 500;
        config.SnackbarConfiguration.ShowTransitionDuration = 500;
        config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    }
);
var useUrls = builder.Configuration.GetSection("UseUrls").Get<string[]>();
if (useUrls != null && useUrls.Length > 0)
{
    builder.WebHost.UseUrls(useUrls);
}

builder.Services.AddSingleton<PsaGeneralLogger>();
builder.Services.AddSingleton<InstanceLogger>();
builder.Services.AddSingleton<InstanceHandler>();
builder.Services.AddSingleton<EventDispatchService>();
builder.Services.AddSingleton<ManagerFacade>();

var app = builder.Build();
app.Services.GetService<EventDispatchService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    if (useUrls != null && useUrls.Length > 0)
    {
        builder.WebHost.UseUrls(useUrls);
    }

    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
