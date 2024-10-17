using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysShopDAO.Interfaces;
using SysShopDAO.Model;


namespace SysShopDAO.Implementation
{
    public class SessionImpl : ISession
    {
        public void CerrarSesion()
        {
            SessionClass.SessionID = 0;
            SessionClass.SessionRol = " ";
            SessionClass.SessionFullName = " ";
            SessionClass.SessionUserName = " ";

            
        }
    }
}
