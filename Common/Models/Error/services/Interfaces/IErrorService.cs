﻿

using System.Collections.Generic;

namespace Common.Models.Error.services.Interfaces
{
    public interface IErrorService
    {
        void AddError(OneError error);
        void AddError(string key, string body);
        bool HasError();
        List<OneError> GetErrors();
        ErrorObject GetErrorsObject();
       
    }
}
