using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	
 *  Manage TextMap And Query Of Text
 *
 *	by Xuanyi
 *
 */

namespace MoleMole
{
    public class Localization
    {
        /* Language Types */
        public const string CHINESE = "Localization/Chinese.json";
        public const string ENGLISH = "Localization/English.json";

        private string _language;
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
                string text = ResourceManager.readTextFromFile(Application.dataPath + "/Resources/" + _language);
                if (text != null)
                {
                    _languageNode = SimpleJSON.JSON.Parse(text);
                }
            }
        }

        private SimpleJSON.JSONNode _languageNode;

        private Localization()
        {
            Language = CHINESE;
        }

        public string GetText(string id)
        {
            return _languageNode[id];
        }
    }
}
