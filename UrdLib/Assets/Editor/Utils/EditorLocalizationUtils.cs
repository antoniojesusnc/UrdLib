using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Urd.Editor.Utils
{
    public class EditorLocalizationUtils
    {
        public static LocalizedString GetLocalizedString(string table, string key, params string[] variables)
        {
            var localizedString = new LocalizedString(table, key);
            for (int i = 0; i < variables.Length; i++)
            {
                var variable = new StringVariable();
                variable.Value = i.ToString();
                localizedString.Add(variables[i], variable);
            }

            return localizedString;
        }
    }
}