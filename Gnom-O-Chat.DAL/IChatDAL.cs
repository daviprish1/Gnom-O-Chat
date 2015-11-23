using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gnom_O_Chat.EntityFr;
using Gnom_O_Chat.Repository;

namespace Gnom_O_Chat.DAL
{
    public interface IChatDAL
    {
        ChatUser GetUserByAccPass(string name, string pass);
        void AddChatUser(string name, string pass);
        void AddConnection(int idUser, bool isLogin);
        void AddUserToChat(ChatUser user, int idChat);
        void AddUserByNameToChat(string username, int idChat);
        int GetMainChatIdx();

        void SetUserOnlineOffline(ChatUser user, bool itLogin);
        void AddMessageToHistory(ChatUser user, string chatTitle, string msg);

        Chat GetChatFromTitle(string title);

        int GetLastMessageId(string chatTitle);

        List<MessageInfo> GetNewMessages(int lastMsgId, string chatName);

        List<string> GetListOfChats();
        List<string> GetListOfUsers();

        List<ConnectionInfo> GetConnectionInfoForAllUsers();

        List<ConnectionInfo> GetConnectionInfoForAllUsersBetweenDates(DateTime first, DateTime second);

        List<ConnectionInfo> GetConnectionInfoForSomeUser(string username);

        List<ConnectionInfo> GetConnectionInfoForSomeUserBetweenDates(string username, DateTime first, DateTime second);

        List<MessageInfo> GetMessageHistoryForAllUsers();

        List<MessageInfo> GetMessageHistoryForAllUsersBetweenDates(DateTime first, DateTime second);

        List<MessageInfo> GetMessageHistoryForSpecifiedChat(string chatname);

        List<MessageInfo> GetMessageHistoryForSpecifiedChatBetweenDates(string chatname, DateTime first, DateTime second);

        List<MessageInfo> GetMessageHistoryForSpecifiedChatAndUser(string chatname, string username);

        List<MessageInfo> GetMessageHistoryForSpecifiedChatAndUserBetweenDates(string chatname, string username, DateTime first, DateTime second);

        void AddNewChat(string name);

        List<string> GetListOfUserChats(ChatUser user);

        List<string> GetListOfUsersWhatCanBeAddedToChat(string chattitle, ChatUser user);

        ChatUser GetUserByAcc(string username);

        void LeaveFromMembership(string chatname, ChatUser user);
    }
}
