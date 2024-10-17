using SysShopDAO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysShopDAO.Interfaces
{
    public interface IUsuario:IBase<Usuario>
    {
        DataTable Login(string nombreUsuario, string contrasenia);
        bool ExisteNombreUsuario(string nombreUsuario);
        string generarUsuario(string nombres, string primerApellido, string segundoApellido);
        string generarContra();
        DataTable VerificarContraseñaActual(string nombreUsuario, string contraseniaActual);

        // Cambia la contraseña del usuario
        int CambiarContrasenia(byte idUsuario, string nuevaContrasenia);
        bool validarContraseña(string contraseña);



    }
}
