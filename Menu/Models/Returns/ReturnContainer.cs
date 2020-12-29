using jwtLib.JWTAuth.Models.Poco;
using Menu.Models.DAL.Domain;
using Menu.Models.Error;
using Menu.Models.Poco;
using Menu.Models.Returns.Interfaces;
using Menu.Models.Returns.Types;
using System;
using System.Collections.Generic;

namespace Menu.Models.Returns
{
    public class ReturnContainer : IReturnContainer
    {
        private static Dictionary<Type, IReturnObjectFactory> _dictReturn = new Dictionary<Type, IReturnObjectFactory>()//TODO сделать статик?
        {
            //можно в отдельный контейнер как с ошибками и его подключать и так для каждого "модуля", если в апе допустим menu+что то еще
            //можно сделать словарем делегатов у Func<object,object>
            {typeof(AllTokens),new TokensReturnFactory() },
            {typeof(ErrorObject),new ErrorObjectReturnFactory () },
            {typeof(ArticleShort),new ArticleShortReturnFactory() },
            {typeof(Article),new ArticleFactory() },
            {typeof(List<ArticleShort>),new ArticleShortReturnFactory() },
            {typeof(List<Article>),new ArticleFactory() },
             {typeof(List<BoolResult>),new BoolResultFactory() },
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

        public void Init(string nameSpace)
        {
            //пока смысла нет, задел на будущее, если понадобится
        }
    }
}
