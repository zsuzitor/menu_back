using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Common.Models
{
    class _IStartUpInitializer
    {
    }

    public interface IStartUpInitializer
    {
        Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services);
        Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services);
        IStartUpInitializer ServicesInitialize(IServiceCollection services);
        IStartUpInitializer RepositoriesInitialize(IServiceCollection services);
        IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider);
    }
}
