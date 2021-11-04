using BLL.Base;
using Model;
using Model.ViewModels.Message;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLMessage : BLBase
    {
        public VmMessage GetMessageById(int id)
        {
            var messageRepository = UnitOfWork.GetRepository<MessageRepository>();

            var message = messageRepository.GetMessageById(id);

            var vmMessage = new VmMessage
            {
                Id = message.Id,
                MessageDate = message.MessageDate,
                MessageText = message.MessageText,
                OrderNumber = message.OrderNumber,
                PublicUserEmail = message.PublicUserEmail,
                Receiver = message.Receiver,
                Sender = message.Sender,
                Title = message.Title,
                Type = message.Type,
                Visited = message.Visited,
                ShowAnswerButton = true,
            };

            return vmMessage;
        }
        public IEnumerable<VmMessage> GetRecevierMessages(string receiver)
        {
            var viewMessageRepository = UnitOfWork.GetRepository<ViewMessageRepository>();

            var messageList = viewMessageRepository.GetReceiverViewMessages(receiver);

            var VmMessageList = from m in messageList
                                select new VmMessage
                                {
                                    Id = m.Id,
                                    MessageDate = m.MessageDate,
                                    MessageText = m.MessageText,
                                    OrderNumber = m.OrderNumber,
                                    PublicUserEmail = m.PublicUserEmail,
                                    Receiver = m.Receiver,
                                    ReceiverName = m.ReceiverName,
                                    Sender = m.Sender,
                                    SenderName = m.SenderName,
                                    Title = m.Title,
                                    Type = m.Type,
                                    Visited = m.Visited,
                                };

            return VmMessageList;
        }

        public IEnumerable<VmMessage> GetAllMessages()
        {
            var viewMessageRepository = UnitOfWork.GetRepository<ViewMessageRepository>();

            var messageList = viewMessageRepository.GetAllViewMessages();

            var VmMessageList = from m in messageList
                                select new VmMessage
                                {
                                    Id = m.Id,
                                    MessageDate = m.MessageDate,
                                    MessageText = m.MessageText,
                                    OrderNumber = m.OrderNumber,
                                    PublicUserEmail = m.PublicUserEmail,
                                    Receiver = m.Receiver,
                                    ReceiverName = m.ReceiverName,
                                    Sender = m.Sender,
                                    SenderName = m.SenderName,
                                    Title = m.Title,
                                    Type = m.Type,
                                    Visited = m.Visited,
                                    ShowAnswerButton = true,
                                    FaMessageDate = AppClassLibrary.AppToolBox.GetJalaliDateText(m.MessageDate.Value.Date),
                                };

            return VmMessageList;
        }
        public IEnumerable<VmMessage> GetMessagesByFilter(VmMessage filterItem)
        {
            var viewMessageRepository = UnitOfWork.GetRepository<ViewMessageRepository>();

            var viewFilterItem = new ViewMessage
            {
                Receiver = filterItem.Receiver,
                MessageText = filterItem.MessageText,
                SenderName = filterItem.SenderName,
                MessageDate = filterItem.MessageDate,
            };

            var messageList = viewMessageRepository.Select(viewFilterItem, 0, int.MaxValue, filterItem.FromDate, filterItem.ToDate);

            var vmMessageList = (from message in messageList
                                 select new VmMessage
                                 {
                                     Id = message.Id,
                                     MessageDate = message.MessageDate,
                                     MessageText = message.MessageText,
                                     OrderNumber = message.OrderNumber,
                                     PublicUserEmail = message.PublicUserEmail,
                                     Receiver = message.Receiver,
                                     Sender = message.Sender,
                                     ReceiverName = message.ReceiverName,
                                     SenderName = message.SenderName,
                                     Title = message.Title,
                                     Type = message.Type,
                                     Visited = message.Visited,
                                     ShowAnswerButton = true,
                                     FaMessageDate = AppClassLibrary.AppToolBox.GetJalaliDateText(message.MessageDate.Value.Date),

                                 }).ToList();

            foreach (var item in vmMessageList)
            {
                if (item.Receiver == "70d732a9-3fb4-47b7-a5df-b737a3980158" && item.Sender == "00000000-0000-0000-0000-000000000000") //Admin
                {
                    item.SenderName = item.PublicUserEmail;
                    item.ShowAnswerButton = false;
                }




            }
            return vmMessageList;
        }

        public int CreateMessage(VmMessage vmMessage)
        {
            MessageRepository MessageRepository = UnitOfWork.GetRepository<MessageRepository>();

            var newMessage = new Message()
            {
                MessageDate = vmMessage.MessageDate,
                MessageText = vmMessage.MessageText,
                OrderNumber = vmMessage.OrderNumber,
                PublicUserEmail = vmMessage.PublicUserEmail,
                Receiver = vmMessage.Receiver,
                Sender = vmMessage.Sender,
                Title = vmMessage.Title,
                Type = vmMessage.Type,
                Visited = vmMessage.Visited,
            };

            MessageRepository.CreateMessage(newMessage);

            UnitOfWork.Commit();

            return newMessage.Id;
        }
        public bool UpdateMessage(VmMessage vmMessage)
        {
            try
            {
                MessageRepository MessageRepository = UnitOfWork.GetRepository<MessageRepository>();

                var updateableMessage = new Message()
                {
                    Id = vmMessage.Id,
                    MessageDate = vmMessage.MessageDate,
                    MessageText = vmMessage.MessageText,
                    OrderNumber = vmMessage.OrderNumber,
                    PublicUserEmail = vmMessage.PublicUserEmail,
                    Receiver = vmMessage.Receiver,
                    Sender = vmMessage.Sender,
                    Title = vmMessage.Title,
                    Type = vmMessage.Type,
                    Visited = vmMessage.Visited,
                };

                MessageRepository.UpdateMessage(updateableMessage);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool UpdateMessageVisit(int id, bool visited)
        {
            try
            {
                MessageRepository MessageRepository = UnitOfWork.GetRepository<MessageRepository>();


                MessageRepository.UpdateMessageVisit(id, visited);

                UnitOfWork.Commit();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool DeleteMessage(int id)
        {
            bool result = true;
            try
            {
                MessageRepository MessageRepository = UnitOfWork.GetRepository<MessageRepository>();

                if (MessageRepository.DeleteMessage(id) == true)
                {
                    UnitOfWork.Commit();
                }
            }
            catch
            {
                result = false;
            }

            return result;

        }
        public int GetMessagesCount(string receiver, bool visited = false)
        {
            MessageRepository MessageRepository = UnitOfWork.GetRepository<MessageRepository>();

            var messageCount = MessageRepository.GetMessagesCount(receiver, visited);
            return messageCount;
        }
    }
}