using Menu.Models.Error.services.Interfaces;
using System.Collections.Generic;

namespace Menu.Models.Error.services
{
    public class ErrorService : IErrorService
    {
        private readonly Dictionary<string, OneError> _errors;


        public ErrorService()
        {
            _errors = new Dictionary<string, OneError>();
        }


        public void AddError(OneError error)
        {
            if (error != null && !string.IsNullOrWhiteSpace(error.Key))
            {
                _errors.TryAdd(error.Key, error);
            }

        }

        public void AddError(string key, string body)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }
            var error = new OneError(key, body);

            _errors.TryAdd(error.Key, error);
        }

    }
}
