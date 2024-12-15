using Bit.Besql.Sample.Client.Data;
using Bit.Besql.Sample.Client.Data.CompiledModel;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddBesqlDbContextFactory<OfflineDbContext>(optionsBuilder =>
        {
            optionsBuilder
                .UseModel(OfflineDbContextModel.Instance) // use generated compiled model in order to make db context optimized
                .UseSqlite("Data Source=Offline-ClientDb.db");
        });

        return services;
    }
}
