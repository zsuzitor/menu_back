

namespace Common.Models.Error.Interfaces
{
    public interface IErrorContainer
    {
        /// <summary>
        /// get error object from static container errors
        /// return null if dont searched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        OneError TryGetError(string key);
    }
}
