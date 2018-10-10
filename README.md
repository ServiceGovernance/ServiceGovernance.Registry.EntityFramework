# ServiceGovernance.Registry.EntityFramework

[![Build status](https://ci.appveyor.com/api/projects/status/ke7dooxke4nas6jq?svg=true)](https://ci.appveyor.com/project/twenzel/servicegovernance-registry-entityframework)
[![NuGet Version](http://img.shields.io/nuget/v/ServiceGovernance.Registry.EntityFramework.svg?style=flat)](https://www.nuget.org/packages/ServiceGovernance.Registry.EntityFramework/)
[![License](https://img.shields.io/badge/license-Apache-blue.svg)](LICENSE)

Persistance library for [ServiceRegistry](https://github.com/ServiceGovernance/ServiceGovernance.Registry) using EntityFramework.

## Usage

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

    services.AddServiceRegistry()
        // this adds the persistence to EF
        .AddRegistryStore(options =>
        {
            options.ConfigureDbContext = builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("default"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
        });
}
```
