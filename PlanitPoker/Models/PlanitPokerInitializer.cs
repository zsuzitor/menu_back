using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using PlanitPoker.Models.Helpers;
using PlanitPoker.Models.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Services;

namespace PlanitPoker.Models
{
    public class PlanitPokerInitializer: IStartUpInitializer
    {
        public void ErrorContainerInitialize(ErrorContainer errorContainer)
        {//todo может это лучше через наследование? под каждое приложение свой контейнер
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomNameIsEmpty, "Название комнаты не указано");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.PlanitUserNotFound, "Пользователь не найден");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomAlreadyExist, "Комната уже существует");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.SomeErrorWithRoomCreating, "Неизвестная ошибка при создании комнаты");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomNotFound, "Комната не найдена");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.DontHaveAccess, "Нет доступа");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.CantVote, "Нельзя проголосовать");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.StoryNotFound, "История не найдена");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.StoryBadStatus, "Неверный статус истории");


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

        public void WorkersInitialize(IWorker worker)
        {

        }
    }
}
