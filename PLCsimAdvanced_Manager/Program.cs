using System.Reflection;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Win32;
using MudBlazor.Services;

const string VERSION = "5.0";
const string BaseDLLName = "Siemens.Simatic.Simulation.Runtime.Api";
string architectureFolder = Environment.Is64BitOperatingSystem ? "x64" : "x86";
string DLLName = $"{BaseDLLName}.{architectureFolder}";

string registryKey = @"SOFTWARE\Wow6432Node\Siemens\Shared Tools\PLCSIMADV_SimRT";
string registryValueName = "Path";
using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
{
    if (key != null)
    {
        object value = key.GetValue(registryValueName);
        if (value != null)
        {
            string installationPath = value.ToString();

            // Append the API subdirectory to the installation path
            string apiPath = System.IO.Path.Combine(installationPath, "API", VERSION);
            string dllPath = Path.Combine(apiPath, DLLName + ".dll");
            Assembly dll = Assembly.LoadFile(dllPath);
        }
        else
        {
            Console.WriteLine($"{registryValueName} not found in registry.");
        }
    }
    else
    {
        Console.WriteLine("PLCsim Advanced key not found in registry. Is PLCsim Advanced installed?.");
    }
}



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBootstrapBlazor();
builder.Services.AddMatBlazor();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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