using BLL.Base;

using Model;
using Model.ViewModels.FlashCard;

using Repository.EF.Repository;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLFlashCard : BLBase
    {
        FlashCardRepository flashCardRepository;
        public BLFlashCard(int currentLanguageId) : base(currentLanguageId)
        {
            flashCardRepository = UnitOfWork.GetRepository<FlashCardRepository>();

        }
        public IEnumerable<VmFlashCard> GetAllFlashCard()
        {
            var flashCards = flashCardRepository.GetAllFlashCard();
            var vmFlashCards = (from f in flashCards
                                select new VmFlashCard
                                {
                                    Id = f.Id,
                                    Front = f.Front,
                                    Back = f.Back,
                                }).ToList();

            return vmFlashCards;
        }
        public VmFlashCard GetFlashCardById(int id)
        {
            var flashCards = flashCardRepository.GetFlashCardById(id);
            var vmFlashCard = new VmFlashCard
            {
                Id = flashCards.Id,
                Front = flashCards.Front,
                Back = flashCards.Back,
            };

            return vmFlashCard;
        }
        public IEnumerable<VmFlashCard> GetFlashCardHasAllByFront(string frontText)
        {
            var words = frontText.Split(' ');
            var predicateBuilder = PredicateBuilder.True<FlashCard>();

            foreach (var item in words)
            {
                predicateBuilder = predicateBuilder.And(p => p.Front.Contains(item));
            }

            var flashCards = flashCardRepository.GetFlashCard(predicateBuilder);
            var vmFlashCards = (from f in flashCards
                                select new VmFlashCard
                                {
                                    Id = f.Id,
                                    Front = f.Front,
                                    Back = f.Back,
                                }).ToList();

            return vmFlashCards;
        }
        public IEnumerable<VmFlashCard> GetFlashCardHasAllByBack(string backText)
        {
            var words = backText.Split(' ');
            var predicateBuilder = PredicateBuilder.True<FlashCard>();

            foreach (var item in words)
            {
                predicateBuilder = predicateBuilder.And(p => p.Back.Contains(item));
            }

            var flashCards = flashCardRepository.GetFlashCard(predicateBuilder);
            var vmFlashCards = (from f in flashCards
                                select new VmFlashCard
                                {
                                    Id = f.Id,
                                    Front = f.Front,
                                    Back = f.Back,
                                }).ToList();

            return vmFlashCards;
        }
        public IEnumerable<VmFlashCard> GetFlashCardHasAnyByFront(string frontText)
        {
            var words = frontText.Split(' ');
            var predicateBuilder = PredicateBuilder.False<FlashCard>();

            foreach (var item in words)
            {
                predicateBuilder = predicateBuilder.Or(p => p.Front.Contains(item));
            }

            var flashCards = flashCardRepository.GetFlashCard(predicateBuilder);
            var vmFlashCards = (from f in flashCards
                                select new VmFlashCard
                                {
                                    Id = f.Id,
                                    Front = f.Front,
                                    Back = f.Back,
                                }).ToList();

            return vmFlashCards;
        }
        public IEnumerable<VmFlashCard> GetFlashCardHasAnyByBack(string backText)
        {
            var words = backText.Split(' ');
            var predicateBuilder = PredicateBuilder.False<FlashCard>();

            foreach (var item in words)
            {
                predicateBuilder = predicateBuilder.Or(p => p.Back.Contains(item));
            }

            var flashCards = flashCardRepository.GetFlashCard(predicateBuilder);
            var vmFlashCards = (from f in flashCards
                                select new VmFlashCard
                                {
                                    Id = f.Id,
                                    Front = f.Front,
                                    Back = f.Back,
                                }).ToList();

            return vmFlashCards;
        }
        public bool CreateFlashCard(VmFlashCard vmFlashCard)
        {
            try
            {

                flashCardRepository.CreateFlashCard(new FlashCard
                {
                    Front = vmFlashCard.Front,
                    Back = vmFlashCard.Back,
                });

                UnitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateFlashCard(VmFlashCard vmFlashCard)
        {
            try
            {
                flashCardRepository.UpdateFlashCard(
                    new FlashCard
                    {
                        Id = vmFlashCard.Id,
                        Front = vmFlashCard.Front,
                        Back = vmFlashCard.Back,
                    });

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}