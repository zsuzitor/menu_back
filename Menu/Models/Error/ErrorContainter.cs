using Menu.Models.Error.Interfaces;
using System;
using System.Collections.Generic;

namespace Menu.Models.Error
{
    public class ErrorContainer : IErrorContainer
    {
        //TODO надо сделать перечислением Dictionary<enum, OneError>
        public readonly Dictionary<string, OneError> StaticErrors;

        public OneError TryGetError(string key)
        {
            if (StaticErrors.ContainsKey(key))
            {
                return StaticErrors[key];
            }

            return null;
        }
        public ErrorContainer()
        {
            StaticErrors = new Dictionary<string, OneError>()
            {
                { "user_not_found", new OneError("user_not_found","Пользователь не найден") },
                { ErrorConsts.NotAuthorized, new OneError(ErrorConsts.NotAuthorized,"Не авторизован") },
                { ErrorConsts.SomeError, new OneError(ErrorConsts.SomeError,"Произошла неизвестная ошибка") },
                { ErrorConsts.NotFound, new OneError(ErrorConsts.NotFound,"Не найдено") },
                { "has_no_access", new OneError("has_no_access","Нет доступа") },
                { "user_already_exist", new OneError("user_already_exist","Пользователь уже существует") },
            };
        }
    }
}
