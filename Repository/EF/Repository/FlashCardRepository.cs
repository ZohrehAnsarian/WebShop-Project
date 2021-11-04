using Repository.EF.Base;
using System.Linq;
using Model;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using LinqKit;

namespace Repository.EF.Repository
{
    public class FlashCardRepository : EFBaseRepository<FlashCard>
    {
        public void CreateFlashCard(FlashCard flashCard)
        {
            Add(flashCard);
        }

        public IEnumerable<FlashCard> GetAllFlashCard()
        {

            var flashCards = Context.FlashCards.OrderByDescending(f => f.Id).ToArray();

            return flashCards;
        }

        public IEnumerable<FlashCard> GetFlashCard(Expression<Func<FlashCard, bool>> predicate)
        {

            var flashCards = Context.FlashCards.AsExpandable().Where(predicate).ToArray();

            return flashCards;
        }


        public FlashCard GetFlashCardById(int id)
        {
            var flashCard = Context.FlashCards.SingleOrDefault(a => a.Id == id);

            return flashCard;
        }

        public void UpdateFlashCard(FlashCard flashCard)
        {
            var oldFlashCard = Context.FlashCards.Find(flashCard.Id);

            oldFlashCard.Front = flashCard.Front;
            oldFlashCard.Back = flashCard.Back;

            Update(oldFlashCard);
        }

    }
}
