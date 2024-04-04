namespace MyApi.Tests;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using MyApi.Domain.Customer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<ApplicationFactory<MyApi.Program>, ApplicationFactory<MyApi.Program>>();
        services.AddTransient<ApplicationFactory, ApplicationFactory>();
        services.AddTransient<TestDatabase, TestDatabase>();
        //services.AddTransient<ClientAuth, ClientAuth>();
    }
}