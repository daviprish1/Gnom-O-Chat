//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gnom_O_Chat.EntityFr
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChatUser
    {
        public ChatUser()
        {
            this.ChatConnections = new HashSet<ChatConnections>();
            this.ChatMembership = new HashSet<ChatMembership>();
            this.History = new HashSet<History>();
        }
    
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsOnline { get; set; }
        public bool IsMod { get; set; }
    
        public virtual ICollection<ChatConnections> ChatConnections { get; set; }
        public virtual ICollection<ChatMembership> ChatMembership { get; set; }
        public virtual ICollection<History> History { get; set; }
    }
}
