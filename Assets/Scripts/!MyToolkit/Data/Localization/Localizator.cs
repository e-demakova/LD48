using System;
using System.Collections.Generic;

using UnityEngine;

using Deblue.Data;
using Deblue.ObservingSystem;

namespace Deblue.Localization
{
    public class Language
    {
        public readonly SystemLanguage LanguageType;
        public readonly string         FileName;

        public static Language Eng => new Language(SystemLanguage.English);
        public static Language Ru => new Language(SystemLanguage.Russian);

        public Language(SystemLanguage language)
        {
            LanguageType = language;
            FileName = Application.dataPath + "/Configs/Localization/" + Enum.GetName(typeof(SystemLanguage), language) + ".json";
        }
    }

    public readonly struct Language_Change
    {
        public readonly Language NewLanguage;

        public Language_Change(Language language)
        {
            NewLanguage = language;
        }
    }

    public static class Localizator
    {
        public static Handler<Language_Change> LanguageChange;

        [HideInInspector] public static SystemLanguage DefaultLanguage = SystemLanguage.Russian;
        [HideInInspector] public static Language Language;

        private static Dictionary<string, string> _locale = new Dictionary<string, string>(50);

        public static void Init()
        {
            LoadLanguage();
            LoadTextData();
        }

        public static void ChangeLanguage(SystemLanguage language)
        {
            Language = new Language(language);
            LoadTextData();
            SaveSelectedLanguage();
            RaiseLanguageChanged();
        }

        private static void LoadLanguage()
        {
            var lang = PlayerPrefs.GetString("Language");
            var result = DefaultLanguage;

            if (!string.IsNullOrEmpty(lang))
            {
                Enum.TryParse(lang, out result);
            }
            Language = new Language(result);
        }

        private static void SaveSelectedLanguage()
        {
            PlayerPrefs.SetString("Language", Enum.GetName(typeof(SystemLanguage), Language.LanguageType));
            PlayerPrefs.Save();
        }

        private static void LoadTextData()
        {
            var fileName = Language.FileName;
            _locale.Clear();

            var data = SavingManager.LoadJSON<TextItems>(fileName).Items;
            
            for (int i = 0; i < data.Length; i++)
            {
                _locale.Add(data[i].Id, data[i].Text);
            }
        }

        public static string GetText(string id)
        {
            _locale.TryGetValue(id, out var text);
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception(string.Format("Text id {0} didn't register.", id));
            }
            return text;
        }

        private static void RaiseLanguageChanged()
        {
            LoadTextData();
            LanguageChange.Raise(new Language_Change(Language));
        }
    }
}