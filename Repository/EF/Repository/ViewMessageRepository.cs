using Model;
using Repository.EF.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class ViewMessageRepository : EFBaseRepository<ViewMessage>
    {
        public IEnumerable<ViewMessage> GetAllViewMessages()
        {
            var ViewMessageList = from s in Context.ViewMessages
                                  orderby s.MessageDate descending
                                  select s;

            return ViewMessageList.OrderByDescending(A => A.MessageDate).ToArray();
        }

        public List<ViewMessage> GetReceiverViewMessages(string receiver)
        {
            var ViewMessageList = from s in Context.ViewMessages
                                  where s.Receiver == receiver
                                  select s;

            return ViewMessageList.OrderByDescending(A => A.MessageDate).ToList();
        }

        public IEnumerable<ViewMessage> Select(ViewMessage filterItem, int index, int count, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var messageList = from message in Context.ViewMessages
                              //where
                              //  message.Sender != "70d732a9-3fb4-47b7-a5df-b737a3980158" &&
                              //  message.Receiver != "00000000-0000-0000-0000-000000000000" //Admin to Public User
                              select message;

            if (filterItem.MessageText != null)
            {
                messageList = messageList.Where(j => j.MessageText.Contains(filterItem.MessageText));
            }

            if (filterItem.SenderName != null)
            {
                messageList = messageList.Where(j => j.SenderName.Contains(filterItem.SenderName));
            }

            if (filterItem.Receiver != null)
            {
                messageList = messageList.Where(j => j.Receiver == filterItem.Receiver);
            }

            if (fromDate != null)
            {
                messageList = messageList.Where(j => j.MessageDate >= fromDate && j.MessageDate <= toDate);
            }

            return messageList.OrderByDescending(j => j.MessageDate).Skip(index).Take(count).ToArray();
        }
    }
}
