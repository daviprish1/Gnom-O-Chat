using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;

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
            int count = (from h in this._context.History
                         join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                         where c.ChatTitle == chatTitle
                         select h.IdHistory).Count();
            if (count < 1)
                return 0;

            int id = (from h in this._context.History
                      join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                      where c.ChatTitle == chatTitle
                      select h.IdHistory).Max();

            return id;
        }

        public List<MessageInfo> GetNewMessages(int lastMsgId, string chatName)
        {
            List<MessageInfo> newMessages = new List<MessageInfo>();

            var newmsgs = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where h.IdHistory > lastMsgId && c.ChatTitle == chatName
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in newmsgs)
            {
                MessageInfo nm = new MessageInfo();
                nm.userName = msg.Username;
                nm.message = msg.Message;
                nm.messageDate = msg.MsgDate;
                nm.messageId = msg.MsgId;
                nm.chatTitle = msg.ChatName;

                newMessages.Add(nm);
            }

            return newMessages;
        }


        public List<string> GetListOfChats()
        {
            List<string> ListOfchats = new List<string>();
            var chats = from c in this._context.Chat
                        select c.ChatTitle;
            ListOfchats.AddRange(chats);

            return ListOfchats;
        }

        public List<ConnectionInfo> GetConnectionInfoForAllUsers()
        {
            List<ConnectionInfo> connectInfo = new List<ConnectionInfo>();

            var history = from cc in this._context.ChatConnections
                          join u in this._context.ChatUser on cc.IdUser_ChatUser equals u.IdUser
                          select new { user = u.UserName, connectiondate = cc.ConnectionDate, islogin = cc.IsLogin };

            foreach (var h in history)
            {
                ConnectionInfo info = new ConnectionInfo();
                info.activitydate = h.connectiondate;
                info.username = h.user;
                info.isLogin = h.islogin;

                connectInfo.Add(info);
            }
            return connectInfo;
        }

        public List<ConnectionInfo> GetConnectionInfoForAllUsersBetweenDates(DateTime first, DateTime second)
        {
            if (first > second)
            {
                DateTime tmpdt = first;
                first = second;
                second = tmpdt;
            }

            List<ConnectionInfo> connectInfo = new List<ConnectionInfo>();

            var history = from cc in this._context.ChatConnections
                          join u in this._context.ChatUser on cc.IdUser_ChatUser equals u.IdUser
                          where cc.ConnectionDate >= first && cc.ConnectionDate <= second
                          select new { user = u.UserName, connectiondate = cc.ConnectionDate, islogin = cc.IsLogin };

            foreach (var h in history)
            {
                ConnectionInfo info = new ConnectionInfo();
                info.activitydate = h.connectiondate;
                info.username = h.user;
                info.isLogin = h.islogin;

                connectInfo.Add(info);
            }
            return connectInfo;
        }

        public List<ConnectionInfo> GetConnectionInfoForSomeUser(string username)
        {
            List<ConnectionInfo> connectInfo = new List<ConnectionInfo>();

            var history = from cc in this._context.ChatConnections
                          join u in this._context.ChatUser on cc.IdUser_ChatUser equals u.IdUser
                          where u.UserName == username
                          select new { user = u.UserName, connectiondate = cc.ConnectionDate, islogin = cc.IsLogin };

            foreach (var h in history)
            {
                ConnectionInfo info = new ConnectionInfo();
                info.activitydate = h.connectiondate;
                info.username = h.user;
                info.isLogin = h.islogin;

                connectInfo.Add(info);
            }
            return connectInfo;
        }

        public List<ConnectionInfo> GetConnectionInfoForSomeUserBetweenDates(string username, DateTime first, DateTime second)
        {
            if (first > second)
            {
                DateTime tmpdt = first;
                first = second;
                second = tmpdt;
            }

            List<ConnectionInfo> connectInfo = new List<ConnectionInfo>();

            var history = from cc in this._context.ChatConnections
                          join u in this._context.ChatUser on cc.IdUser_ChatUser equals u.IdUser
                          where u.UserName == username && cc.ConnectionDate >= first && cc.ConnectionDate <= second
                          select new { user = u.UserName, connectiondate = cc.ConnectionDate, islogin = cc.IsLogin };

            foreach (var h in history)
            {
                ConnectionInfo info = new ConnectionInfo();
                info.activitydate = h.connectiondate;
                info.username = h.user;
                info.isLogin = h.islogin;

                connectInfo.Add(info);
            }
            return connectInfo;
        }

        public List<MessageInfo> GetMessageHistoryForAllUsers()
        {
            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }


        public List<MessageInfo> GetMessageHistoryForAllUsersBetweenDates(DateTime first, DateTime second)
        {
            if (first > second)
            {
                DateTime tmpdt = first;
                first = second;
                second = tmpdt;
            }

            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where h.MessageDate >= first && h.MessageDate <= second
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }




        public List<MessageInfo> GetMessageHistoryForSpecifiedChat(string chatname)
        {
            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where c.ChatTitle == chatname
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }

        public List<MessageInfo> GetMessageHistoryForSpecifiedChatBetweenDates(string chatname, DateTime first, DateTime second)
        {
            if (first > second)
            {
                DateTime tmpdt = first;
                first = second;
                second = tmpdt;
            }

            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where c.ChatTitle == chatname && h.MessageDate >= first && h.MessageDate <= second
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }

        public List<MessageInfo> GetMessageHistoryForSpecifiedChatAndUser(string chatname, string username)
        {
            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where c.ChatTitle == chatname && u.UserName == username
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }

        public List<MessageInfo> GetMessageHistoryForSpecifiedChatAndUserBetweenDates(string chatname, string username, DateTime first, DateTime second)
        {
            if (first > second)
            {
                DateTime tmpdt = first;
                first = second;
                second = tmpdt;
            }

            List<MessageInfo> messageshistory = new List<MessageInfo>();

            var history = from h in this._context.History
                          join c in this._context.Chat on h.IdChat_Chat equals c.IdChat
                          join u in this._context.ChatUser on h.IdUser_ChatUser equals u.IdUser
                          where c.ChatTitle == chatname && h.MessageDate >= first && h.MessageDate <= second && u.UserName == username
                          select new
                          {
                              Username = u.UserName,
                              Message = h.Message,
                              MsgDate = h.MessageDate,
                              MsgId = h.IdHistory,
                              ChatName = c.ChatTitle
                          };

            foreach (var msg in history)
            {
                MessageInfo mi = new MessageInfo();
                mi.userName = msg.Username;
                mi.message = msg.Message;
                mi.messageDate = msg.MsgDate;
                mi.messageId = msg.MsgId;
                mi.chatTitle = msg.ChatName;

                messageshistory.Add(mi);
            }

            return messageshistory;
        }

        public List<string> GetListOfUsers()
        {
            List<string> ListOfUsers = new List<string>();

            var users = (from u in this._context.ChatUser
                         select u.UserName);
            ListOfUsers.AddRange(users);

            return ListOfUsers;
        }

        public void AddNewChat(string name)
        {
            Chat chat = new Chat();
            chat.ChatTitle = name;
            chat.IsWhisper = false;

            this._context.Chat.Add(chat);
            this._context.SaveChanges();
        }

        public List<string> GetListOfUserChats(ChatUser user)
        {
            List<string> userChats = new List<string>();

            var chats = from c in this._context.Chat
                        join cm in this._context.ChatMembership on c.IdChat equals cm.IdChat_Chat
                        where cm.IdUser_ChatUser == user.IdUser
                        select c.ChatTitle;
            userChats.AddRange(chats);

            return userChats;
        }

        public List<string> GetListOfUsersWhatCanBeAddedToChat(string chattitle, ChatUser user)
        {
            List<string> userlist = new List<string>();
            Chat chat = this.GetChatFromTitle(chattitle);

            //var users = (from u in this._context.ChatUser
            //            join cm in this._context.ChatMembership on u.IdUser equals cm.IdUser_ChatUser
            //            join c in this._context.Chat on cm.IdChat_Chat equals c.IdChat
            //            where c.ChatTitle != chattitle
            //            select u.UserName).Distinct();

            var users = (from u in this._context.ChatUser
                        where
                        !(from cm in this._context.ChatMembership
                             where cm.IdChat_Chat == chat.IdChat
                             select cm.IdUser_ChatUser).Contains(u.IdUser)
                        select u.UserName).Distinct();
            userlist.AddRange(users);

            return userlist;
        }

        public ChatUser GetUserByAcc(string username)
        {
            ChatUser user;
            var usr = (from u in this._context.ChatUser
                       where u.UserName == username
                       select u).FirstOrDefault();
            user = usr;
            return user;
        }

        public void LeaveFromMembership(string chatname, ChatUser user)
        {
            Chat chat = this.GetChatFromTitle(chatname);

            var membership = from cm in this._context.ChatMembership
                             where cm.IdChat_Chat == chat.IdChat && cm.IdUser_ChatUser == user.IdUser
                             select cm;

            this._context.ChatMembership.RemoveRange(membership);
            this._context.SaveChanges();
        }


    }
}
