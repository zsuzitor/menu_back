

namespace Menu.Models.Returns.Interfaces
{
    public interface IReturnContainer
    {
        /// <summary>
        /// если нужна какая то особая коллекция типов
        /// </summary>
        /// <param name="nameSpace"></param>
        void Init(string nameSpace);
        object GetReturnType<TIn>(TIn obj);
    }
}
