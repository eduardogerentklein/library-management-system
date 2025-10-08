using Application;
using ConsoleApp.Menus;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services
    .AddInfrastructure()
    .AddApplicationServices();

services.AddSingleton<MainMenu>();

var serviceProvider = services.BuildServiceProvider();

var mainMenu = serviceProvider.GetRequiredService<MainMenu>();
await mainMenu.ShowAsync();
