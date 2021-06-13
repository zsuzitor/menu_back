
using System;
using System.Collections.Generic;
using WEB.Common.Models.Returns.Interfaces;

namespace WEB.Common.Models.Returns
{
    public class ReturnContainer : IReturnContainer
    {
        private static Dictionary<Type, IReturnObjectFactory> _dictReturn = new Dictionary<Type, IReturnObjectFactory>()//TODO сделать статик?
        {
            //можно в отдельный контейнер как с ошибками и его подключать и так для каждого "модуля", если в апе допустим menu+что то еще
            //можно сделать словарем делегатов у Func<object,object>
           
            
               
              
            //TODO списки

        };

        public object GetReturnType<TIn>(TIn obj)
        {
            Type objType = typeof(TIn);
            if (_dictReturn.ContainsKey(objType))
            {
                var factory = _dictReturn[objType];
                return factory.GetObjectReturn(obj);
            }

            return obj;
        }


        public void AddTypeToContainer(Type type, IReturnObjectFactory factory)
        {
            if (!_dictReturn.ContainsKey(type))
            {
                _dictReturn.Add(type, factory);
            }
        }

        public void Init(string nameSpace)
        {
            //пока смысла нет, задел на будущее, если понадобится
        }
    }
}
