
using BO.Models.Auth;
using BO.Models.WordsCardsApp.DAL.Domain;
using Common.Models.Error;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WordsCardsApp.BL.Services.Interfaces;
using WordsCardsApp.BO.Input;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.BL.Services
{
    public class WordsCardsService : IWordsCardsService
    {
        private readonly IWordsCardsRepository _wordCardRepository;
        private readonly IWordsListRepository _wordCardListsRepository;
        private readonly IImageService _imageService;

        public WordsCardsService(IWordsCardsRepository repository, IImageService imageService, IWordsListRepository wordCardListsRepository)
        {
            _wordCardRepository = repository;
            _imageService = imageService;
            _wordCardListsRepository = wordCardListsRepository;
        }

        public async Task<WordCard> GetByIdIfAccess(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _wordCardRepository.GetByIdIfAccessNoTrackAsync(id, userInfo.UserId);
        }


        public async Task<WordCard> Create(WordCardInputModel input, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var wordCardNew = WordCardFromInputModelNew(input);
            
            wordCardNew.UserId = userInfo.UserId;

            //long? listId = null;
            if (input.ListId != null)
            {
                var lst = await _wordCardListsRepository.GetByIdIfAccess(input.ListId.Value, userInfo.UserId);
                if (lst != null)
                {
                    //listId = lst.Id;
                    wordCardNew.WordCardWordList.Add(new WordCardWordList() { WordsListId = lst.Id });
                }
            }

            try
            {
                wordCardNew.ImagePath = await _imageService.CreateUploadFileWithOutDbRecord(input.MainImageNew);
                var resWordCard = await _wordCardRepository.AddAsync(wordCardNew);
                return resWordCard;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<WordCard>> Create(List<WordCardInputModel> input, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (input == null || input.Count == 0)
            {
                return new List<WordCard>();
            }

            try
            {
                List<WordCard> forAdd = new List<WordCard>();
                //var listsFromInput = new List<long>();
                var listsFromInput = input.Where(x => x.ListId != null).Select(x => x.ListId.Value).Distinct().ToList();
                var lst = await _wordCardListsRepository.GetByIdIfAccessNoTrack(listsFromInput, userInfo.UserId);
                foreach (var i in input)
                {
                    var wordCardNew = WordCardFromInputModelNew(i);
                    wordCardNew.UserId = userInfo.UserId;
                    wordCardNew.ImagePath = await _imageService.CreateUploadFileWithOutDbRecord(i.MainImageNew);
                    if (i.ListId != null)
                    {
                        var curList = lst.FirstOrDefault(x => x.Id == i.ListId);
                        if (curList != null)
                        {
                            wordCardNew.WordCardWordList.Add(new WordCardWordList() { WordsListId = curList.Id });
                        }
                    }
                    forAdd.Add(wordCardNew);
                }

                var resWordCards = await _wordCardRepository.AddAsync(forAdd);
                return resWordCards;
            }
            catch
            {
                throw;
            }
        }

        public async Task<WordCard> Update(WordCardInputModel input, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            if (input?.Id == null)
            {
                throw new SomeCustomException("id_is_required");//TODO
            }

            var oldObj = await GetByIdIfAccess((long)input.Id, userInfo);


            if (oldObj == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }


            var changed = FillWordCardFromInputModelEdit(oldObj, input);

          

            if (input.MainImageNew != null)
            {
                await _imageService.DeleteFileWithOutDbRecord(oldObj.ImagePath);
                oldObj.ImagePath = await _imageService.CreateUploadFileWithOutDbRecord(input.MainImageNew);
                changed = true;
            }
            else if (input.DeleteMainImage ?? false && !string.IsNullOrWhiteSpace(oldObj.ImagePath))
            {
                await _imageService.DeleteFileWithOutDbRecord(oldObj.ImagePath);
                oldObj.ImagePath = null;
                changed = true;

            }

            if (changed)//?
            {
                await _wordCardRepository.UpdateAsync(oldObj);
            }

            return oldObj;
        }

        /// <summary>
        /// true - hide now
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<bool> ChangeHideStatus(long id, UserInfo userInfo)
        {
            var hided = await _wordCardRepository.ChangeHideStatusAsync(id, userInfo.UserId);
            if (hided == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            return hided.Value;
        }

        public async Task<List<WordCard>> CreateFromFile(IFormFile file, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            //string strFile = null;
            List<WordCard> dataForAdd = new List<WordCard>();
            using var reader = new StreamReader(file.OpenReadStream());
            while (!reader.EndOfStream)
            {
                var tmpStr = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(tmpStr))
                {
                    continue;
                }

                var strData = tmpStr.Split(';');
                if (strData.Length < 5)
                {
                    continue;
                }

                dataForAdd.Add(new WordCard()
                {
                    Word = strData[0].Trim(),
                    WordAnswer = strData[1].Trim(),
                    Description = strData[2].Trim(),
                    Hided = bool.Parse(strData[3].Trim()),
                    ImagePath = strData[4].Trim(),
                    UserId = userInfo.UserId,
                });
            }

            //strFile = await reader.ReadLineAsync();
            return await _wordCardRepository.AddAsync(dataForAdd);
        }

        public async Task<WordCard> Delete(long id, UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            var deletedRecord = await _wordCardRepository.DeleteAsync(id, userInfo.UserId);
            if (deletedRecord == null)
            {
                throw new SomeCustomException(ErrorConsts.NotFound);
            }

            await _imageService.DeleteFileWithOutDbRecord(deletedRecord.ImagePath);

            return deletedRecord;
        }

        public async Task<List<WordCard>> GetAllForUser(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                throw new NotAuthException();
            }

            return await _wordCardRepository.GetAllUsersWordCardsAsync(userInfo.UserId);
        }


        //очень спорное название тк завязался на view
        public async Task<List<WordCard>> GetAllForUserForView(UserInfo userInfo)
        {
            var records = await GetAllForUser(userInfo);
            return await _wordCardRepository.LoadWordListsIdAsync(records);
        }

        private WordCard WordCardFromInputModelNew(WordCardInputModel input)
        {
            return new WordCard()
            {
                Word = input.Word,
                Description = input.Description,
                WordAnswer = input.WordAnswer,
            };
        }


        private bool FillWordCardFromInputModelEdit(WordCard baseObj, WordCardInputModel newObj)
        {
            bool changed = false;
            if (baseObj.Word != newObj.Word)
            {
                baseObj.Word = newObj.Word;
                changed = true;
            }

            if (baseObj.Description != newObj.Description)
            {
                baseObj.Description = newObj.Description;
                changed = true;
            }

            if (baseObj.WordAnswer != newObj.WordAnswer)
            {
                baseObj.WordAnswer = newObj.WordAnswer;
                changed = true;
            }

            return changed;
        }

        public string ToStringForSave(WordCard obj)
        {
            return $"{obj.Word};{obj.WordAnswer};{obj.Description};{obj.Hided};{obj.ImagePath};";
        }

        public async Task<List<string>> GetAllRecordsStringForSave(UserInfo userInfo)
        {
            var records = await GetAllForUser(userInfo);
            List<string> res = new List<string>();
            records.ForEach(x => res.Add(ToStringForSave(x)));
            return res;
        }

       
    }
}
