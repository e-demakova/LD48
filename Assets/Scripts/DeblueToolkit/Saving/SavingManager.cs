using System.IO;
using System.Text;

using UnityEngine;


namespace Deblue.Data
{
    public sealed class SavingManager
    {
        public static Encoding DefoultEncoding => Encoding.UTF8;

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
            var filePath = GetFilePath(fileName, false);
            using (Stream stream = File.Create(filePath))
            {
                using (StreamWriter writer = new StreamWriter(stream, DefoultEncoding))
                {
                    writer.Write(JsonUtility.ToJson(obj));
                }
            }
        }

        public static T Load<T>(string fileName)
        {
            return Load<T>(fileName, default(T));
        }

        public static T Load<T>(string fileName, T defoultValue)
        {
            T result;

            var filePath = GetFilePath(fileName, true);
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

        private static string GetFilePath(string fileName, bool checkExisting)
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
