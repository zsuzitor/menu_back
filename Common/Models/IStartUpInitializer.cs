﻿using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Models
{
    class _IStartUpInitializer
    {
    }

    public interface IStartUpInitializer
    {
        void ErrorContainerInitialize(ErrorContainer errorContainer);
        void ServicesInitialize(IServiceCollection services);
        void RepositoriesInitialize(IServiceCollection services);
        void WorkersInitialize(IServiceProvider serviceProvider);
    }
}
