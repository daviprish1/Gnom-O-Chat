using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnom_O_Chat.EntityFr;
using Gnom_O_Chat.Repository;

namespace Gnom_O_Chat.DAL
{
    public class ChatDBmanager : IChatDAL
    {
        private EntitiesModel _context;

        public ChatDBmanager()
        {
            this._context = new EntitiesModel();
        }

        public ChatUser GetUserByAccPass(string name, string pass)
        {

            ChatUser curUser = this._context.ChatUser.Where(u => u.UserName == name && u.Password == pass).FirstOrDefault();

            return curUser;
        }


        public void AddChatUser(string name, string pass)
        {
            ChatUser newUser = new ChatUser();
            newUser.UserName = name;
            newUser.Password = pass;
            newUser.IsMod = false;
            newUser.IsOnline = false;

            this._context.ChatUser.Add(newUser);

            this._context.SaveChanges();
        }


        public void AddConnection(int idUser, bool isLogin)
        {
            ChatConnections connection = new ChatConnections();
            connection.ConnectionDate = DateTime.Now;
            connection.IdUser_ChatUser = idUser;
            connection.IsLogin = isLogin;

            this._context.ChatConnections.Add(connection);

            this._context.SaveChanges();
        }


        public void AddUserToChat(ChatUser user, int idChat)
        {
            ChatMembership membership = new ChatMembership();


            membership.IdUser_ChatUser = user.IdUser;
            membership.IdChat_Chat = idChat;

            this._context.ChatMembership.Add(membership);
            this._context.SaveChanges();

        }


        public void AddUserByNameToChat(string username, int idChat)
        {
            ChatMembership membership = new ChatMembership();

            var u = (from user in this._context.ChatUser
                     where user.UserName == username
                     select user).FirstOrDefault();

            membership.IdUser_ChatUser = u.IdUser;
            membership.IdChat_Chat = idChat;

            this._context.ChatMembership.Add(membership);
            this._context.SaveChanges();
        }


        public int GetMainChatIdx()
        {
            var idx = (from chat in this._context.Chat
                       where chat.ChatTitle == "MainChat"
                       select chat.IdChat).FirstOrDefault();

            return idx;
        }


        public void SetUserOnlineOffline(ChatUser user, bool itLogin)
        {
            user.IsOnline = itLogin;
            this._context.SaveChanges();
        }


        public void AddMessageToHistory(ChatUser user, string chatTitle, string msg)
        {
            History history = new History();

            var chat = (from c in this._context.Chat
                        where c.ChatTitle == chatTitle
                        select c).First();

            history.IdChat_Chat = chat.IdChat;
            history.IdUser_ChatUser = user.IdUser;
            history.Message = msg;
            history.MessageDate = DateTime.Now;

            this._context.History.Add(history);
            this._context.SaveChanges();
        }

        public Chat GetChatFromTitle(string title)
        {
            var curChat = (from c in this._context.Chat
                           where c.ChatTitle == title
                           select c).First();

            return curChat;
        }

        public int GetLastMessageId(string chatTitle)
        {
            int id = (from h in this._context.History
                      join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                      where c.ChatTitle == chatTitle
                      select h.IdHistory).Max();

            return id;
        }

        public List<NewMessage> GetNewMessages(int lastMsgId, string chatName)
        {
            List<NewMessage> newMessages = new List<NewMessage>();

            var newmsgs = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where h.IdHistory > lastMsgId && c.ChatTitle == chatName
                          select new { Username = u.UserName, Message = h.Message, MsgDate = h.MessageDate,
                              MsgId = h.IdHistory, ChatName = c.ChatTitle};

            foreach(var msg in newmsgs)
            {
                NewMessage nm = new NewMessage();
                nm.userName = msg.Username;
                nm.message = msg.Message;
                nm.messageDate = msg.MsgDate;
                nm.messageId = msg.MsgId;
                nm.chatTitle = msg.ChatName;

                newMessages.Add(nm);
            }

            return newMessages;
        }
    }
}
