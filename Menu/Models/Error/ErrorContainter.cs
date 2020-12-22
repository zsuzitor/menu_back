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
                { "not_authorized", new OneError("not_authorized","Не авторизован") },
                { "some_error", new OneError("some_error","Произошла неизвестная ошибка") },
                { "not_found", new OneError("not_found","Не найдено") },
                { "has_no_access", new OneError("has_no_access","Нет доступа") },
                { "user_already_exist", new OneError("user_already_exist","Пользователь уже существует") },
            };
        }
    }
}
