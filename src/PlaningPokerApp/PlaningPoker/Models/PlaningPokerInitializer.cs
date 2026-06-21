using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaningPoker.Models.Helpers;
using PlaningPoker.Models.Repositories;
using PlaningPoker.Models.Repositories.Interfaces;
using PlaningPoker.Models.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlaningPoker.Models
{
    public class PlaningPokerInitializer: IStartUpInitializer
    {
        public async Task<IStartUpInitializer> ErrorContainerInitialize(IServiceProvider services)
        {

            var serviceScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {

                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomNameIsEmpty, "Название комнаты не указано", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.PlaningUserNotFound, "Пользователь не найден", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomAlreadyExist, "Комната уже существует", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.SomeErrorWithRoomCreating, "Неизвестная ошибка при создании комнаты", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.BadRoomNameWithRoomCreating, "Нельзя использовать данное название, разрешены англ буквы+цифры до 30 знаков", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomNotFound, "Комната не найдена", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.DontHaveAccess, "Нет доступа", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.CantVote, "Нельзя проголосовать", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.StoryNotFound, "История не найдена", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.StoryBadStatus, "Неверный статус истории", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomBadPassword, "Пароль не подходит", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomBadVoteMark, "Оценка не входит в допустимый диапазон", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomBadVoteMarks, "Передан неверный список оценок", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomBadCountCards, "Передано неверное количество карточек, доспустимо от 2 до 100", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.RoomBadLengthCard, "Допустимая длина карточки 5 символов", "PlaningPoker", "Error");
                await configurationService.AddIfNotExistAsync(Constants.PlaningPokerErrorConsts.UsernameBad, "Имя пользователя - кириллица/латиница/цифры до 50 символов", "PlaningPoker", "Error");

            }

            return this;
        }

        public IStartUpInitializer RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPlaningUserRepository, PlaningUserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            return this;
        }

        public IStartUpInitializer ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPlaningPokerService, PlaningPokerService>();

            services.AddScoped<IPlaningApiHelper, PlaningApiHelper>();
            return this;

        }

        public async Task<IStartUpInitializer> ConfigurationInitialize(IServiceProvider services)
        {
            //var configurationService = services.GetRequiredService<IConfigurationService>();
            return this;
        }

        public IStartUpInitializer WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<IPlaningPokerService>> actAlert = prSrv => prSrv.HandleInRoomsMemoryAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("planing_poker_clean", conf["PlaningPokerApp:CleanRoomsJobCron"], actAlert);
            return this;
        }
    }
}
