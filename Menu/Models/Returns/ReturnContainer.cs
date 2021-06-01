using jwtLib.JWTAuth.Models.Poco;
using BO.Models.MenuApp.DAL.Domain;
using Common.Models.Error;
using Common.Models.Poco;
using System;
using System.Collections.Generic;
using Menu.Models.Returns.Interfaces;
using MenuApp.Models.BO;
using Menu.Models.Returns.Types;
using Menu.Models.Returns.Types.MenuApp;
using BO.Models.DAL.Domain;
using Menu.Models.Returns.Types.WordsCardsApp;
using BO.Models.WordsCardsApp.DAL.Domain;
using PlanitPoker.Models;
using Menu.Models.Returns.Types.PlanitPoker;

namespace Menu.Models.Returns
{
    public class ReturnContainer : IReturnContainer
    {
        private static Dictionary<Type, IReturnObjectFactory> _dictReturn = new Dictionary<Type, IReturnObjectFactory>()//TODO сделать статик?
        {
            //можно в отдельный контейнер как с ошибками и его подключать и так для каждого "модуля", если в апе допустим menu+что то еще
            //можно сделать словарем делегатов у Func<object,object>
            {typeof(AllTokens), new TokensReturnFactory() },
            {typeof(ErrorObject), new ErrorObjectReturnFactory () },
            {typeof(ArticleShort), new ArticleShortReturnFactory() },
            {typeof(Article), new ArticleReturnFactory() },
            {typeof(List<ArticleShort>), new ArticleShortReturnFactory() },
            {typeof(List<Article>), new ArticleReturnFactory() },
            {typeof(BoolResult), new BoolResultFactory() },
            {typeof(User), new ShortUserReturnFactory() },
            {typeof(WordCard), new WordCardReturnFactory() },
            {typeof(List<WordCard>), new WordCardReturnFactory() },
            {typeof(WordsList), new WordListReturnFactory() },
            {typeof(List<WordsList>), new WordListReturnFactory() },
            {typeof(WordCardWordList), new WordCardWordListReturnFactory() },
            {typeof(List<WordCardWordList>), new WordCardWordListReturnFactory() },
            {typeof(PlanitUser), new PlanitUserReturnFactory() },
            {typeof(List<PlanitUser>), new PlanitUserReturnFactory() },
            {typeof(StoredRoom), new StoredRoomReturnFactory() },
            {typeof(List<StoredRoom>), new StoredRoomReturnFactory() },
            
               
              
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
