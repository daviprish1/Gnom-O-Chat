using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnom_O_Chat.Repository
{
    public class MessageInfo
    {
        public string userName
        { get; set; }

        public string message
        { get; set; }

        public DateTime messageDate
        { get; set; }

        public int messageId
        { get; set; }

        public string chatTitle
        { get; set; }
    }
}
