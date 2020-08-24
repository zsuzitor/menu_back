

namespace Menu.Models.Error.services.Interfaces
{
    public interface IErrorService
    {
        void AddError(OneError error);
        void AddError(string key,string body);
    }
}
