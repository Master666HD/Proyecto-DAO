using MySql.Data.MySqlClient;
using SysShopDAO.Interfaces;
using SysShopDAO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SysShopDAO.Implementation
{
    public class UsuarioImpl :BaseImpl, IUsuario
    {
        public int Delete(Usuario t)
        {
            query = @"UPDATE usuario SET estado=0,
	                fechaActualizacion=CURRENT_TIMESTAMP
                    WHERE idUsuario=@idUsuario";
            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@idUsuario", t.Id);
            return BaseExecuteNonQuery(command);
        }

        public int Insert(Usuario t)
        {
            // 1. Insertar el registro en la tabla "usuario"
            query = @"INSERT INTO usuario(ci, nombres, primerApellido, segundoApellido, fechaNacimiento, sexo, rol, nombreUsuario, contrasenia,idUsuario)
              VALUES(@ci, @nombres, @primerApellido, @segundoApellido, @fechaNacimiento, @sexo, @rol, @nombreUsuario, MD5(@contrasenia),@idUsuario)";

            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@ci", t.Ci);
            command.Parameters.AddWithValue("@nombres", t.Nombres);
            command.Parameters.AddWithValue("@primerApellido", t.PrimerApellido);
            command.Parameters.AddWithValue("@segundoApellido", t.SegundoApellido);
            command.Parameters.AddWithValue("@fechaNacimiento", t.FechaNacimiento);
            command.Parameters.AddWithValue("@sexo", t.Sexo);
            command.Parameters.AddWithValue("@rol", t.Rol);
            command.Parameters.AddWithValue("@nombreUsuario", t.NombreUsuario);
            command.Parameters.AddWithValue("@contrasenia", t.Contrasenia);
            command.Parameters.AddWithValue("@idUsuario", t.idUsuario);
            
            try
            {
                return BaseExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public Usuario Get(byte id)
        {
            Usuario t = null;
            query = @"SELECT id, ci, nombres, primerApellido, segundoApellido, fechaNacimiento, sexo, rol, nombreUsuario, contrasenia, estado, fechaRegistro, IFNULL(fechaActualizacion, CURRENT_TIMESTAMP),idUsuario
                  FROM usuario
                  WHERE id = @id";
            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@id", id);

            DataTable table = ExecuteDataTable(command);

            if (table.Rows.Count > 0)
            {
                t = new Usuario
                    (
                    byte.Parse(table.Rows[0][0].ToString()),
                    table.Rows[0][1].ToString(),
                    table.Rows[0][2].ToString(),
                    table.Rows[0][3].ToString(),
                    table.Rows[0][4].ToString(),
                    DateTime.Parse(table.Rows[0][5].ToString()),
                    char.Parse(table.Rows[0][6].ToString()),
                    table.Rows[0][7].ToString(),
                    table.Rows[0][8].ToString(),
                    table.Rows[0][9].ToString(),
                    byte.Parse(table.Rows[0][10].ToString()),
                    DateTime.Parse(table.Rows[0][11].ToString()),
                    DateTime.Parse(table.Rows[0][12].ToString()),
                    byte.Parse(table.Rows[0][13].ToString())
                );
            }
            return t;
        }
        public DataTable Select()
        {
           
                query = @"SELECT id AS ID , nombres, primerApellido, segundoApellido, sexo, ci,fechaNacimiento,  rol, nombreUsuario, contrasenia, estado, fechaRegistro, IFNULL(fechaActualizacion, CURRENT_TIMESTAMP)
                  FROM usuario
                  WHERE estado = 1
                  ORDER BY 2;";
                MySqlCommand command = CreateBasicCommand(this.query);
                return ExecuteDataTable(command);
            

        }

        public int Update(Usuario t)
        {
            query = @"UPDATE usuario SET ci=@ci, nombres=@nombres, primerApellido=@primerApellido, segundoApellido=@segundoApellido, 
                     fechaNacimiento=@fechaNacimiento, sexo=@sexo, rol=@rol, nombreUsuario=@nombreUsuario, contrasenia=@contrasenia, 
                     fechaActualizacion=CURRENT_TIMESTAMP
                  WHERE id=@id";

            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@ci", t.Ci);
            command.Parameters.AddWithValue("@nombres", t.Nombres);
            command.Parameters.AddWithValue("@primerApellido", t.PrimerApellido);
            command.Parameters.AddWithValue("@segundoApellido", t.SegundoApellido);
            command.Parameters.AddWithValue("@fechaNacimiento", t.FechaNacimiento);
            command.Parameters.AddWithValue("@sexo", t.Sexo);
            command.Parameters.AddWithValue("@rol", t.Rol);
            command.Parameters.AddWithValue("@nombreUsuario", t.NombreUsuario);
            command.Parameters.AddWithValue("@contrasenia", t.Contrasenia);
            command.Parameters.AddWithValue("@id", t.Id);

            return BaseExecuteNonQuery(command);
        }

     
        public DataTable Login(string nombreUsuario, string contrasenia)
        {
            query = @"SELECT id , CONCAT(nombres,' ',primerApellido), rol , nombreUsuario
                        FROM usuario
                        WHERE estado = 1 AND nombreUsuario = @nombreUsuario AND contrasenia = MD5(@contrasenia)";

            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
            command.Parameters.AddWithValue("@contrasenia", contrasenia);

            try
            {
                return ExecuteDataTable(command);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {

            query = @"SELECT COUNT(*) FROM usuario WHERE nombreUsuario = @nombreUsuario AND estado = 1;";
            MySqlCommand command = CreateBasicCommand(this.query);
            command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
           int n = BaseExecuteNonQuery(command);
            
            try
            {
                if (n>0) // Si el conteo es mayor que 0, el nombre de usuario ya existe
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                throw ex; // Lanza la excepción si ocurre algún error
            }
         
        }

        // Método para generar un nombre de usuario único
        public string generarUsuario(string nombres, string primerApellido, string segundoApellido)
        {
            string nombreUsuarioBase = $"{nombres[0]}{primerApellido}".ToLower();
            string nombreUsuario = nombreUsuarioBase;
            int counter = 1;

            // Verificar si el nombre de usuario ya existe, si es así, agregar un número al final
            while (ExisteNombreUsuario(nombreUsuario))
            {
                nombreUsuario = $"{nombreUsuarioBase}{counter}";
                counter++;
            }

            return nombreUsuario;
        }

        // Método para generar una contraseña aleatoria
        public string generarContra()
        {
            // Usamos una cadena de caracteres para generar la contraseña
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8) // La longitud de la contraseña será de 8 caracteres
                                  .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public DataTable VerificarContraseñaActual(string nombreUsuario, string contraseniaActual)
        {
            query = @"SELECT id, nombreUsuario, contrasenia 
              FROM usuario 
              WHERE nombreUsuario = @nombreUsuario AND contrasenia = MD5(@contraseniaActual)";

            MySqlCommand command = CreateBasicCommand(query);
            command.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
            command.Parameters.AddWithValue("@contraseniaActual", contraseniaActual);

            return ExecuteDataTable(command);
        }

        public int CambiarContrasenia(byte id, string nuevaContrasenia)
        {
            query = @"UPDATE usuario SET contrasenia = MD5(@nuevaContrasenia)
              WHERE id = @id";

            MySqlCommand command = CreateBasicCommand(query);
            command.Parameters.AddWithValue("@nuevaContrasenia", nuevaContrasenia);
            command.Parameters.AddWithValue("@id", id);

           return BaseExecuteNonQuery(command);
        }

        public bool validarContraseña(string contraseña)
        {

            // Comprobamos si la contraseña tiene al menos 8 caracteres
            if (contraseña.Length < 8)
            {
                return false;
            }

            bool tieneMayuscula = false;
            bool tieneMinuscula = false;
            bool tieneDigito = false;
            bool tieneSimbolo = false;

            // Recorremos la contraseña para verificar cada carácter
            for (int i = 0; i < contraseña.Length; i++)
            {
                if (char.IsUpper(contraseña[i]))
                {
                    tieneMayuscula = true;
                }
                else if (char.IsLower(contraseña[i]))
                {
                    tieneMinuscula = true;
                }
                else if (char.IsDigit(contraseña[i]))
                {
                    tieneDigito = true;
                }
                else if (!char.IsLetterOrDigit(contraseña[i]))
                {
                    tieneSimbolo = true;
                }

                // Si ya cumple todos los requisitos, devolvemos true
                if (tieneMayuscula && tieneMinuscula && tieneDigito && tieneSimbolo)
                {
                    return true;
                }
            }

            // Si no cumple con al menos uno de los requisitos, devolvemos false
            return false;

        }
    }
}
