using System;
using System.Text;

namespace Security.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Users u = new Users();
            u.username = "liufei";

            string salt = Guid.NewGuid().ToString();
            string passowrd = @"F";

            byte[] passwordAndSaltBytes = Encoding.UTF8.GetBytes(passowrd+salt);
            byte[] hasBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);

            string hashString = Convert.ToBase64String(hasBytes);

            u.password = hashString;
            u.salt = salt;

            Console.WriteLine(hashString.Length);

            Console.WriteLine();
        }
    }

    public class Users
    {
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
    }
}
