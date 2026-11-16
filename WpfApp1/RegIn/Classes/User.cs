using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public void GetUserLogin(string Login)
        {
            using (var db = new WorkingDB())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == Login);
                if (user != null)
                {
                    HandlerCorrectLogin.Invoke();
                }
                else
                {
                    HandlerInCorrectLogin.Invoke();
                }
            }
        }

    }
}
