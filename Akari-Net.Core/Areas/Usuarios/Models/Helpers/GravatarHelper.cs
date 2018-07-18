using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Akari_Net.Core.Areas.Usuarios.Models.Helpers
{
    public static class GravatarHelper
    {
        public static string GetImage(string email)
        {
            const string urlBase = "https://www.gravatar.com/avatar.php?gravatar_id={0}&s=48&d=identicon";
            if (email == null || !email.Contains("@"))
            {
                return string.Format(urlBase, "0");
            }
            byte[] hash = MD5.Create().ComputeHash(new UTF8Encoding().GetBytes(email.ToLower()));
            var str = new StringBuilder(hash.Length * 2);
            foreach (byte t in hash)
                str.Append(t.ToString("X2"));
            return string.Format(urlBase, str.ToString().ToLower());
        }
    }
}
