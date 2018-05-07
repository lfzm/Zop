using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Exceptions;

namespace Orleans
{
    public static class OrlenasExceptionsSiloHostBuilderExtensions
    {
        public static ISiloHostBuilder AddExceptionsFilter(this ISiloHostBuilder builder)
        {
            builder.AddIncomingGrainCallFilter<OrleansExceptionsFiltered>();
            return builder;
        }

        public static ISiloHostBuilder AddExceptionsFilter<TGrainCallFilter>(this ISiloHostBuilder builder)
            where TGrainCallFilter : class, IIncomingGrainCallFilter
        {
            builder.AddIncomingGrainCallFilter<TGrainCallFilter>();
            return builder;
        }
    }
}
