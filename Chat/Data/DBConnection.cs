using Chat.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data
{
    internal class DBConnection
    {
        public static ChatEntities connection = new ChatEntities();
    }
}
