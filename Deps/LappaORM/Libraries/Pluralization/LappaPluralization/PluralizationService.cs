// Copyright (C) Arctium Software.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LappaPluralization
{
    // Basic pluralization class. Needs to be extended with more exceptions.
    // English nouns supported only.
    public class PluralizationService
    {
        PluralizationSettings settings;

        public PluralizationService()
        {
            settings = new PluralizationSettings();
        }

        public string Pluralize(string word)
        {
            if (settings.Exceptions.Contains(word))
                return word;

            var irregularNoun = settings.IrregularWords.SingleOrDefault(s => word.EndsWith(s.Key, StringComparison.InvariantCultureIgnoreCase));

            if (irregularNoun.Key != null)
                return word.Remove(word.Length - irregularNoun.Key.Length, irregularNoun.Key.Length) + word[word.Length - irregularNoun.Key.Length] + irregularNoun.Value.Substring(1);

            if (settings.NonChangingWords.Any(s => word.EndsWith(s, StringComparison.InvariantCultureIgnoreCase)))
                return word;

            if (Regex.IsMatch(word, @"\d$"))
                return word;

            if (word.Length >= 2)
            {
                var wordPreEndChar = word[word.Length - 2];

                if (word.EndsWith("y", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (wordPreEndChar == 'a' || wordPreEndChar == 'e' || wordPreEndChar == 'i' || wordPreEndChar == 'o' || wordPreEndChar == 'u')
                        return word + "ies";

                    return word.Remove(word.Length - 1, 1) + "ies";
                }

                if (word.EndsWith("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (wordPreEndChar == 'a' || wordPreEndChar == 'e' || wordPreEndChar == 'i' || wordPreEndChar == 'o' || wordPreEndChar == 'u')
                        return word + "oes";

                    return word + "s";
                }
            }

            if (word.EndsWith("ies", StringComparison.InvariantCultureIgnoreCase))
                return word;

            if (word.EndsWith("s", StringComparison.InvariantCultureIgnoreCase) || word.EndsWith("x", StringComparison.InvariantCultureIgnoreCase) || 
                word.EndsWith("ch", StringComparison.InvariantCultureIgnoreCase) || word.EndsWith("sh", StringComparison.InvariantCultureIgnoreCase))
                return word + "es";

            return word + "s";
        }

        public string Pluralize<T>() => Pluralize(typeof(T));
        public string Pluralize(Type t) => Pluralize(t.Name);

        public void AddException(string word) => settings.Exceptions.Add(word);
        public void AddIrregularWord(string singular, string irregularPlural) => settings.IrregularWords.Add(singular, irregularPlural);
        public void AddNonChangingWord(string word) => settings.NonChangingWords.Add(word);
    }
}
