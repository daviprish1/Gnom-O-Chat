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

        List<NewMessage> GetNewMessages(int lastMsgId, string chatName);
    }
}
