using System;
using System.IO;
using System.Security.Cryptography;

namespace grpcArachne.Services
{
    public interface ICryptoService
    {
        string encrypt(string rawData);
        string decrypt(string chippedData);
    }

    public class CryptoService : ICryptoService
    {
        private readonly Aes _aes;

        public CryptoService()
        {
            _aes = Aes.Create();
        }

        public string encrypt(string rawDataString)
        {
            using ICryptoTransform encryptor = _aes.CreateEncryptor();
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(rawDataString);
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string decrypt(string chippedDataString)
        {
            byte[] chippedData = Convert.FromBase64String(chippedDataString);
            using ICryptoTransform decryptor = _aes.CreateDecryptor();
            using MemoryStream msEncrypt = new(chippedData);
            using CryptoStream csDecrypt = new(msEncrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
