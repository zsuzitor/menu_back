
using BO.Models.Auth;
using BO.Models.WordsCardsApp.DAL.Domain;
using Common.Models.Error;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsCardsApp.BL.Services.Interfaces;
using WordsCardsApp.BO.Input;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.BL.Services
{
    public class WordsListService : IWordsListService
    {
        private readonly IWordsListRepository _wordsListRepository;
        private readonly IWordsCardsService _wordsCardsService;

        public WordsListService(IWordsListRepository wordsListRepository, IWordsCardsService wordsCardsService)
        {
            _wordsListRepository = wordsListRepository;
            _wordsCardsService = wordsCardsService;
        }

        public async Task<WordCardWordList> AddToList(long cardId, long listId, UserInfo userInfo)
        {
            var list = await _wordsListRepository.GetByIdIfAccess(listId, userInfo.UserId);
            if (list == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var card = await _wordsCardsService.GetByIdIfAccess(cardId, userInfo);
            if (card == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var relation = await _wordsListRepository.Get(cardId, listId);
            if (relation != null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);//TODO тут бы указать ошибку точнее, типо уже добавлено в список
            }

            return await _wordsListRepository.AddToList( new WordCardWordList() { WordCardId = cardId, WordsListId = listId });


        }

        public async Task<WordsList> Create(WordCardListInputModel input, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (input == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);//TODO
            }

            var forAdd = new WordsList()
            {
                Title = input.Title,
                UserId = userInfo.UserId,
            };
            return await _wordsListRepository.Add(forAdd);
            //throw new System.NotImplementedException();
        }

        public async Task<WordsList> Delete(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }


            var rec = await _wordsListRepository.GetByIdIfAccess(id, userInfo.UserId);
            if (rec == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return await _wordsListRepository.Delete(rec);
        }

        public async Task<List<WordsList>> GetAllForUser(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _wordsListRepository.GetAllForUser(userInfo.UserId);
        }

        public async Task<WordsList> GetByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _wordsListRepository.GetByIdIfAccess(id, userInfo.UserId);
        }

        public async Task<WordCardWordList> RemoveFromList(long cardId, long listId, UserInfo userInfo)
        {
            var list = await _wordsListRepository.GetByIdIfAccess(listId, userInfo.UserId);
            if (list == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var card = await _wordsCardsService.GetByIdIfAccess(cardId, userInfo);//вохможно лишнее, тк уже проверили лист
            if (card == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var relation = await _wordsListRepository.Get(cardId, listId);
            if (relation == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);//TODO тут бы указать ошибку точнее, типо уже добавлено в список
            }

            return await _wordsListRepository.RemoveFromList(relation);
        }

        public async Task<WordsList> Update(WordCardListInputModel input, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (input?.Id == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            var fromDb = await _wordsListRepository.GetByIdIfAccess(input.Id.Value, userInfo.UserId);
            if (fromDb == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            fromDb.Title = input.Title;
            return await _wordsListRepository.Update(fromDb);

        }
    }
}
