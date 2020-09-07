

namespace Menu.Models.Error.Interfaces
{
    public interface IErrorContainer
    {
        OneError TryGetError(string key);
    }
}
