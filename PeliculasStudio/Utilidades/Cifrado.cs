using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasStudio.Utilidades
{
    public static class Cifrado
    {
        public static string HashPassword(string pswd)
        {
            using( var sha256= SHA256.Create())
            {

                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pswd));

                StringBuilder builder = new StringBuilder();

                for(int i=0;i< bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
