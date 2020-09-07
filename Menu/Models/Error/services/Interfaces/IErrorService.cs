

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Menu.Models.Error.services.Interfaces
{
    public interface IErrorService
    {
        void AddError(OneError error);
        void AddError(string key, string body);
        bool HasError();
        List<OneError> GetErrors();
        ErrorObject GetErrorsObject();
        bool ErrorsFromModelState(ModelStateDictionary modelState);
    }
}
