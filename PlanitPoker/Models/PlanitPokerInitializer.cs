using Common.Models;
using Common.Models.Error;
using Microsoft.Extensions.DependencyInjection;
using PlanitPoker.Models.Helpers;
using PlanitPoker.Models.Repositories;
using PlanitPoker.Models.Repositories.Interfaces;
using PlanitPoker.Models.Services;
using System;
using System.Linq.Expressions;

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
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.BadRoomNameWithRoomCreating, "Нельзя использовать данное название, разрешены англ буквы+цифры до 30 знаков");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomNotFound, "Комната не найдена");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.DontHaveAccess, "Нет доступа");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.CantVote, "Нельзя проголосовать");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.StoryNotFound, "История не найдена");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.StoryBadStatus, "Неверный статус истории");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomBadPassword, "Пароль не подходит");

            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomBadVoteMark, "Оценка не входит в допустимый диапазон");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomBadVoteMarks, "Передан неверный список оценок");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomBadCountCards, "Передано неверное количество карточек, доспустимо от 2 до 100");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.RoomBadLengthCard, "Допустимая длина карточки 5 символов");
            errorContainer.InitError(Consts.PlanitPokerErrorConsts.UsernameBad, "Имя пользователя - кириллица/латиница/цифры до 50 символов");
            





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


        public void WorkersInitialize(IServiceProvider serviceProvider)
        {
            Expression<Action<IPlanitPokerService>> actAlert = prSrv => prSrv.HandleInRoomsMemoryAsync();//.Wait();
            var worker = serviceProvider.GetRequiredService<IWorker>();
            worker.Recurring("planit_poker_clean", "0 * * * *", actAlert);//каждый час
        }
    }
}
