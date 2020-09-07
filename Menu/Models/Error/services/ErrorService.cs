using Menu.Models.Error.services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

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
            if (_errors.ContainsKey(key))
            {
                _errors[key].Errors.Add(body);
            }
            else
            {
                _errors.TryAdd(error.Key, error);
            }

        }

        public List<OneError> GetErrors()
        {
            return _errors.Values.ToList();
        }

        public ErrorObject GetErrorsObject()
        {
            var res= new ErrorObject();
            res.Errors = GetErrors();
            return res;
        }

        public bool HasError()
        {
            return _errors.Count > 0;
        }

        //return true если есть ошибки
        public bool ErrorsFromModelState(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return false;
            }

            //var errors = modelState.ToList();
            //if (errors.Count == 0)
            //{
            //    return false;
            //}

            
            foreach (var keyInput in modelState.Keys)
            {
                var input = modelState[keyInput];

                if (input.Errors.Count == 0)
                {
                    continue;
                }

                var errObj = new OneError(keyInput, input.Errors.Select(x => x.ErrorMessage).ToList());
                AddError(errObj);
            }

            return HasError();
        }
    }
}
