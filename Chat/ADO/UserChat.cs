//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chat.ADO
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserChat
    {
        public int Id_userChat { get; set; }
        public int Id_user { get; set; }
        public int Id_chat { get; set; }
    
        public virtual Chat Chat { get; set; }
        public virtual User User { get; set; }
    }
}