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
    
    public partial class Chat
    {
        public Chat()
        {
            this.ChatMembership = new HashSet<ChatMembership>();
            this.History = new HashSet<History>();
        }
    
        public int IdChat { get; set; }
        public string ChatTitle { get; set; }
        public bool IsWhisper { get; set; }
    
        public virtual ICollection<ChatMembership> ChatMembership { get; set; }
        public virtual ICollection<History> History { get; set; }
    }
}
