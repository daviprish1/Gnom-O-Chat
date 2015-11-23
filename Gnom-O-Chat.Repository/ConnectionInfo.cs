using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnom_O_Chat.Repository
{
    public class ConnectionInfo
    {
        public string username { get; set; }
        public DateTime activitydate { get; set; }
        public bool? isLogin { get; set; }
    }
}
