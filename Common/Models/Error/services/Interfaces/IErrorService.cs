

using System.Collections.Generic;

namespace Common.Models.Error.services.Interfaces
{
    public interface IErrorService
    {
        void AddError(OneError error);
        void AddError(string key, string body);
        bool HasError();
        bool HasError(string key);
        List<OneError> GetErrors();
        ErrorObject GetErrorsObject();
       
    }
}
