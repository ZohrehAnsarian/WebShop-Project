using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class MessageRepository : EFBaseRepository<Message>
    {
        public IEnumerable<Message> GetAllMessages()
        {
            var MessageList = from s in Context.Messages
                              select s;

            return MessageList.OrderByDescending(A => A.MessageDate).ToArray();
        }
        public Message GetMessageById(int id)
        {
            var message = Context.Messages.Find(id);

            return message;
        }
        public void CreateMessage(Message Message)
        {
            Add(Message);
        }

        public void UpdateMessage(Message Message)
        {
            var oldMessage = (from s in Context.Messages where s.Id == Message.Id select s).FirstOrDefault();
            oldMessage.MessageDate = Message.MessageDate;
            oldMessage.MessageText = Message.MessageText;
            oldMessage.OrderNumber = Message.OrderNumber;
            oldMessage.PublicUserEmail = Message.PublicUserEmail;
            oldMessage.Receiver = Message.Receiver;
            oldMessage.Sender = Message.Sender;
            oldMessage.Title = Message.Title;
            oldMessage.Type = Message.Type;
            oldMessage.Visited = Message.Visited;

            Update(oldMessage);

        }

        public bool DeleteMessage(int id)
        {
            var result = (from j in Context.Messages where j.Id == id select j).Count();
            if (result == 0)
            {
                var deleteable = Context.Messages.Find(id);
                Delete(deleteable);
                return true;
            }

            return false;

        }

        public int GetMessagesCount(string receiver, bool visited)
        {
            return Context.Messages.Count(m => m.Receiver == receiver && m.Visited == visited);

        }

        public void UpdateMessageVisit(int id, bool visited)
        {
            var oldMessage = (from s in Context.Messages where s.Id == id select s).FirstOrDefault();

            oldMessage.Visited = visited;

            Update(oldMessage);
        }
    }
}
