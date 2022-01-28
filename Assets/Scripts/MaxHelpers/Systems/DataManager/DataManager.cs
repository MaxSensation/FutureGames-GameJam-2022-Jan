using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace MaxHelpers
{
    public class DataManager : StaticInstance<DataManager>
    {
        private readonly Dictionary<string, ISave> _savedData = new();
        private string _saveFile;
        private FileStream _dataStream;
        // Encryption key
        private readonly byte[] _savedKey = { 0x12, 0x15, 0x16, 0x15, 0x12, 0x15, 0x13, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x11 };
        
        public T GetData<T>(string saveKey) => _savedData.ContainsKey(saveKey) ? (T) _savedData[saveKey] : default;
        public void SaveData(string saveKey, ISave data)
        {
            _saveFile = Application.persistentDataPath + $"/{saveKey}_save.json";
            var iAes = Aes.Create();
            _dataStream = new FileStream(_saveFile, FileMode.Create);
            var inputIv = iAes.IV;
            _dataStream.Write(inputIv, 0, inputIv.Length);
            var iStream = new CryptoStream(_dataStream, iAes.CreateEncryptor(_savedKey, iAes.IV), CryptoStreamMode.Write);
            var sWriter = new StreamWriter(iStream);
            var jsonString = JsonUtility.ToJson(data);
            sWriter.Write(jsonString);
            sWriter.Close();
            iStream.Close();
            _dataStream.Close();
        }

        public bool Load<T>(string saveKey)
        {
            _saveFile = Application.persistentDataPath + $"/{saveKey}_save.json";
            if (!File.Exists(_saveFile)) return false;
            _dataStream = new FileStream(_saveFile, FileMode.Open);
            var oAes = Aes.Create();
            var outputIv = new byte[oAes.IV.Length];
            _dataStream.Read(outputIv, 0, outputIv.Length);
            var oStream = new CryptoStream(_dataStream, oAes.CreateDecryptor(_savedKey, outputIv), CryptoStreamMode.Read);
            var reader = new StreamReader(oStream);
            var text = reader.ReadToEnd();
            _savedData.Add(saveKey, (ISave)JsonUtility.FromJson<T>(text));
            reader.Close();
            oStream.Close();
            _dataStream.Close();
            return true;
        }
    }
}