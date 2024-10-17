using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
namespace SysShopDAO.Implementation
{
    
    public class BaseImpl
    {
    
        string connectionString = "Server=localhost;Database=bdincos2023;Uid=root;Pwd=admin";
        public string query;
        public MySqlCommand CreateBasicCommand()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand();
            command.Connection = connection;
            return command;
        }

        public MySqlCommand CreateBasicCommand(string query)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query,connection);
            return command;
        }



        /// <summary>
        /// Insert , Update , Delete
        /// </summary>
        /// <param name="command">Comando asociado a una conexion y con su consulta SQL</param>
        /// <returns></returns>
        public int BaseExecuteNonQuery(MySqlCommand command)
        {
            try
            {
                command.Connection.Open();
                return  command.ExecuteNonQuery();
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


        /// <summary>
        /// Select
        /// </summary>
        /// <param name="command">comando asociado  a la conexion con su consulta SQL y sus parametros</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(MySqlCommand command)
        {
            DataTable table = new DataTable();
            try
            {
                command.Connection.Open();

                MySqlDataAdapter adaptador = new MySqlDataAdapter(command);
                adaptador.Fill(table);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                command.Connection.Close();
            }

            return table;
        }

    }
}
