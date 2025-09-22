using Common.Models.Error.services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Common.Models.Error.services
{
    public sealed class ErrorService : IErrorService
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
                if (_errors.ContainsKey(error.Key))
                {
                    _errors[error.Key].Errors.AddRange(error.Errors);
                }
                else
                {
                    _errors.TryAdd(error.Key, error);
                }
            }
        }

        public void AddError(string key, string body)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            if (_errors.ContainsKey(key))
            {
                _errors[key].Errors.Add(body);
            }
            else
            {
                var error = new OneError(key, body);
                _errors.TryAdd(error.Key, error);
            }

        }

        public List<OneError> GetErrors()
        {
            return _errors.Values.ToList();
        }

        public ErrorObject GetErrorsObject()
        {
            var res = new ErrorObject();
            res.Errors = GetErrors();
            return res;
        }

        public bool HasError()
        {
            return _errors.Count > 0;
        }

        public bool HasError(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return _errors.ContainsKey(key);
        }
    }
}
