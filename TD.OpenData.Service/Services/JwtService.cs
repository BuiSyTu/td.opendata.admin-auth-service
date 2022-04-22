using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.Service.ViewModels;

namespace TD.OpenData.Service.Services
{
    public class JwtService
    {
        private string secretJWT = "TanDan123!@#456789";
        private string keyAES;

        public string CreateJWT(PayLoadJWT payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secretJWT);
            return token;
        }

        public string ValidateJWT(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(secretJWT))
                    secretJWT = ConfigurationManager.AppSettings["secretJWT"] != null ? ConfigurationManager.AppSettings["secretJWT"] : "TanDan123!@#456789";
                var json = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                  .WithSecret(secretJWT)
                  .MustVerifySignature()
                  .Decode(token);
                return json;
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token hết hạn!");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Khóa bí mật sai!");
            }
        }

        private static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        private static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
              .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        private static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
              .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        public string ExcuteEncryptAES(string plainText)
        {
            if (string.IsNullOrEmpty(keyAES))
            {
                keyAES = ConfigurationManager.AppSettings["keyAES"] ?? "TanDan123!@#456789";
            }

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(keyAES)));
        }

        public string ExcuteDecryptAES(string encryptedText)
        {
            if (string.IsNullOrEmpty(keyAES))
            {
                keyAES = ConfigurationManager.AppSettings["keyAES"] ?? "TanDan123!@#456789";
            }

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(keyAES)));
        }
    }
}
