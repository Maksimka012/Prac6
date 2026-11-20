using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace RegIn.Classes
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte[] Image = new byte[0];
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public CorrectLogin HandlerCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;
        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();

        public void GetUserLogin(string login)
        {
            this.Id = -1;
            this.Login = String.Empty;
            this.Password = String.Empty;
            this.Name = String.Empty;
            this.Image = new byte[0];
            MySqlConnection conn = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(conn))
            {
                MySqlDataReader userQuery = WorkingDB.Query($"select * from `users` where `Login` = '{login}'", conn);
                if (userQuery.HasRows)
                {
                    userQuery.Read();
                    this.Id = userQuery.GetInt32(0);
                    this.Login = userQuery.GetString(1);
                    this.Password = userQuery.GetString(2);
                    this.Name = userQuery.GetString(3);
                    if (userQuery.IsDBNull(4))
                    {
                        this.Image = new byte[64 * 1024];
                        userQuery.GetBytes(4, 0, Image, 0, Image.Length);
                    }
                    this.DateUpdate = userQuery.GetDateTime(5);
                    this.DateCreate = userQuery.GetDateTime(6);
                    HandlerInCorrectLogin.Invoke();
                }
                else
                {
                    HandlerInCorrectLogin.Invoke();
                }
            }
            else
            {
                HandlerInCorrectLogin.Invoke();
            }
            WorkingDB.CloseConnecion(conn);
        }

        public void SetUser()
        {
            MySqlConnection conn = WorkingDB.OpenConnection();
            if (WorkingDB.OpenConnection(conn))
            {
                MySqlCommand cmd = new MySqlCommand("insert into `users` (`Login`, `Password`, `Name`, `Image`, `DateUpdate`, `DateCreate`) values (@Login, @Password, @Name, @Image, @DateUpdate, @DateCreate)", conn);
                cmd.Parameters.AddWithValue("@Login", this.Login);
                cmd.Parameters.AddWithValue("@Password", this.Password);
                cmd.Parameters.AddWithValue("@Name", this.Name);
                cmd.Parameters.AddWithValue("@Image", this.Image);
                cmd.Parameters.AddWithValue("@DateUpdate", this.DateUpdate);
                cmd.Parameters.AddWithValue("@DateCreate", this.DateCreate);
                cmd.ExecuteNonQuery();
            }
            WorkingDB.CloseConnecion(conn);
        }

        public void CreateNewPassword()
        {
            if(Login != String.Empty)
            {
                Password = GeneratePass();
                MySqlConnection conn = WorkingDB.OpenConnection();
                if (WorkingDB.OpenConnection(conn))
                {
                    WorkingDB.Query($"update `users` set `Password` = '{this.Password}' where `Login` = '{this.Login}'", conn);
                }
                WorkingDB.CloseConnecion(conn);
                SendMail.SenaMessage($"Your account password has been changed\nNew password: {this.Password}", this.Login );
            }
        }

        public string GeneratePass()
        {
            List<Char> NewPassword = new List<char>();
            Random rnd = new Random();
            char[] ArrNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '%', '&', '*', '=', '+'};
            char[] ArrUppercase = { 'q', 'w', 'e', 'r', 't', 's', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };
            for (int i = 0; i < 1; i++)
            {
                NewPassword.Add(ArrNumbers[rnd.Next(0, ArrNumbers.Length)]);
            }
            for (int i=0; i<1; i++)
            {
                NewPassword.Add(ArrSymbols[rnd.Next(0, ArrNumbers.Length)]);
            }
            for (int i = 0; i < 2; i++)
            {
                NewPassword.Add(char.ToUpper(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]));
            }
            for (int i = 0; i < 6; i++)
            {
                NewPassword.Add(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]);
            }
            for(int i = 0; i < NewPassword.Count; i++)
            {
                int RandomSymbol = rnd.Next(0, NewPassword.Count);
                char Symbol = NewPassword[RandomSymbol];
                NewPassword[RandomSymbol] = NewPassword[i];
                NewPassword[i] = Symbol;
            }
            string nPassword = "";
            for(int i = 0; i <NewPassword.Count; i++)
            {
                nPassword += NewPassword[i];
            }
            return nPassword;
        }
    }
}
