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
        void ErrorContainerInitialize(ErrorContainer errorContainer);
        Task ConfigurationInitialize(IServiceProvider services);
        void ServicesInitialize(IServiceCollection services);
        void RepositoriesInitialize(IServiceCollection services);
        void WorkersInitialize(IServiceProvider serviceProvider);
    }
}
