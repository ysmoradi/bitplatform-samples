﻿using System.Reflection;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    public static void AddClientConfigurations(this IConfigurationBuilder builder)
    {
        var assembly = Assembly.Load("Bit.Tutorial10.Client.Core");
        builder.AddJsonStream(assembly.GetManifestResourceStream("Bit.Tutorial10.Client.Core.appsettings.json")!);

        if (BuildConfiguration.IsDebug())
        {
            builder.AddJsonStream(assembly.GetManifestResourceStream("Bit.Tutorial10.Client.Core.appsettings.Development.json")!);
        }
    }
}
