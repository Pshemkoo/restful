using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTful
{
    public interface IMessageService
    {
        IEnumerable<Message> GetAll();
        Message Get(long id);
        Message CreateMessage(Message weatherForecast);
        Message UpdateMessage(long id, Message weatherForecast);
        void DeleteMessage(long id);
    }

    public class MessageService : IMessageService
    {
        private readonly List<Message> messages;

        public MessageService()
        {
            messages = new List<Message>
            {
                new Message
                {
                    Id = 1L,
                    Content = "Pierwsza wiadomość",
                    Author = "Jacek",
                    Created = DateTime.Now
                },
                new Message
                {
                    Id = 2L,
                    Content = "Druga wiadomość",
                    Author = "Marek",
                    Created = DateTime.Now
                },
                new Message
                {
                    Id = 3L,
                    Content = "Trzecia wiadomość",
                    Author = "Ewa",
                    Created = DateTime.Now
                }
            };
        }

        public Message CreateMessage(Message weatherForecast)
        {
            weatherForecast.Id = messages.Count + 1L;
            messages.Add(weatherForecast);
            return weatherForecast;
        }

        public void DeleteMessage(long id)
        {
            var message = messages.FirstOrDefault(m => m.Id == id);
            messages.Remove(message);
        }

        public Message Get(long id)
        {
            return messages.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Message> GetAll()
        {
            return messages.OrderBy(m => m.Id);
        }

        public Message UpdateMessage(long id, Message weatherForecast)
        {
            var message = messages.FirstOrDefault(m => m.Id == id);
            weatherForecast.Id = message.Id;
            messages.Remove(message);
            messages.Add(weatherForecast);

            return weatherForecast;
        }
    }
}