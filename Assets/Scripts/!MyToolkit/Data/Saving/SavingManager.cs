using System.IO;
using System.Text;

using UnityEngine;


namespace Deblue.Data
{
    public sealed class SavingManager
    {
        public static Encoding DefoultEncoding => Encoding.GetEncoding(1251);

        public static string LoadStreamingAsset(string fileName)
        {
            CheckIsEmpty(fileName);
            string filePath;

            filePath = Application.dataPath + "/StreamingAssets/" + fileName;

            CheckIsExist(filePath);

            return File.ReadAllText(filePath);
        }

        public static T LoadResource<T>(string filePath) where T : Object
        {
            CheckIsEmpty(filePath);

            T result = Resources.Load<T>(filePath);
            if(result == null)
            {
                throw new System.Exception(string.Format("Loading resource {0} by given path {1} failed.", typeof(T).Name, filePath));
            }
            return result;
        }

        public static void Save<T>(string fileName, T obj)
        {
            var filePath = GetPersistentFilePath(fileName, false);
            using (Stream stream = File.Create(filePath))
            {
                using (StreamWriter writer = new StreamWriter(stream, DefoultEncoding))
                {
                    writer.Write(JsonUtility.ToJson(obj));
                }
            }
        }

        public static T LoadJSON<T>(string filePath, T defoultValue = default(T))
        {
            CheckIsEmpty(filePath);
            CheckIsExist(filePath);

            T result;
            using (Stream stream = File.OpenRead(filePath))
            {
                using (StreamReader reader = new StreamReader(stream, DefoultEncoding))
                {
                    result = JsonUtility.FromJson<T>(reader.ReadToEnd());
                }
                if (result == null)
                {
                    result = defoultValue;
                }
            }
            return result;
        }

        public static T LoadPersistentJSON<T>(string fileName, T defoultValue = default(T))
        {
            var filePath = GetPersistentFilePath(fileName, true);
            return LoadJSON(filePath, defoultValue);
        }

        private static void CheckIsEmpty(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new System.ArgumentNullException("File name can't be null.");
            }
        }

        private static void CheckIsExist(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new System.ArgumentException($"The specified file ({filePath}) does not exists.");
            }
        }

        private static string GetPersistentFilePath(string fileName, bool checkExisting)
        {
            CheckIsEmpty(fileName);
            var filePath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);
            if (checkExisting)
            {
                CheckIsExist(filePath);
            }
            return filePath;
        }
    }
}
