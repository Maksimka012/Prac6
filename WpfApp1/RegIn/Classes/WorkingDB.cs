using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RegIn.Classes
{
    public class WorkingDB 
    {
        readonly static string conncection = "server=127.0.0.1;uid=root;pwd=;database=regin";
        public static MySqlConnection OpenConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(conncection);
                conn.Open();
                return conn;
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        } 

        public static MySqlDataReader Query(string Sql, MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand(Sql, conn);
            return cmd.ExecuteReader();
        }

        public static void CloseConnecion(MySqlConnection conn)
        {
            conn.Clone();
            MySqlConnection.ClearPool(conn);
        }

        public static bool OpenConnection(MySqlConnection conn)
        {
            return conn != null && conn.State == System.Data.ConnectionState.Open;
        }
    }
}
