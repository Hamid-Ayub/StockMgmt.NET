using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace PTIStockMgmt.Controllers
{
  public class Crypto
  {
    public static byte[] GenSalt()
    {
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      byte[] buff = new byte[16];
      rng.GetBytes(buff);
      return buff;
    }

    public static byte[] Hash(byte[] salt, string password)
    {
      byte[] pass = System.Text.Encoding.UTF8.GetBytes(password);
      return new SHA256Managed().ComputeHash(salt.Concat(pass).ToArray());
    }

    public static bool ComparePassword(string password, byte[] salt, byte[] stored_hash)
    {
      byte[] attempt = Hash(salt, password);

      return stored_hash.SequenceEqual(attempt);
    }
  }
}