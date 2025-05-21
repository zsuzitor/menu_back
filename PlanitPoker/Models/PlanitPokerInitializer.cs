using BL.Models.Services.Interfaces;
using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanitPoker.Models.Helpers;
using PlanitPoker.Models.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Services;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlanitPoker.Models
{
    public class PlanitPokerInitializer: IStartUpInitializer
    {
        public async Task ErrorContainerInitialize(IServiceProvider services)
        {

            var configurationService = services.GetRequiredService<IConfigurationService>();
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomNameIsEmpty, "Название комнаты не указано", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.PlanitUserNotFound, "Пользователь не найден", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomAlreadyExist, "Комната уже существует", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.SomeErrorWithRoomCreating, "Неизвестная ошибка при создании комнаты", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.BadRoomNameWithRoomCreating, "Нельзя использовать данное название, разрешены англ буквы+цифры до 30 знаков", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomNotFound, "Комната не найдена", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.DontHaveAccess, "Нет доступа", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.CantVote, "Нельзя проголосовать", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.StoryNotFound, "История не найдена", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.StoryBadStatus, "Неверный статус истории", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomBadPassword, "Пароль не подходит", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomBadVoteMark, "Оценка не входит в допустимый диапазон", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomBadVoteMarks, "Передан неверный список оценок", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomBadCountCards, "Передано неверное количество карточек, доспустимо от 2 до 100", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.RoomBadLengthCard, "Допустимая длина карточки 5 символов", "PlaningPoker", "Error");
            await configurationService.AddIfNotExistAsync(Constants.PlanitPokerErrorConsts.UsernameBad, "Имя пользователя - кириллица/латиница/цифры до 50 символов", "PlaningPoker", "Error");


        }

        public void RepositoriesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPlaningUserRepository, PlaningUserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
        }

        public void ServicesInitialize(IServiceCollection services)
        {
            services.AddScoped<IPlanitPokerService, PlanitPokerService>();

            services.AddScoped<IPlanitApiHelper, PlanitApiHelper>();
            
        }

        public async Task ConfigurationInitialize(IServiceProvider services)
        {
            var configurationService = services.GetRequiredService<IConfigurationService>();
        }

        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<IPlanitPokerService>> actAlert = prSrv => prSrv.HandleInRoomsMemoryAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            var conf = serviceProvider.GetRequiredService<IConfiguration>();
            worker.Recurring("planing_poker_clean", conf["PlaningPokerApp:NotificationJobCron"], actAlert);
        }
    }
}
