using System.Text;

namespace SideApp.Model
{
    public static class EnToJapTrans
    {
        private static readonly Dictionary<string, string> Map = new(StringComparer.OrdinalIgnoreCase)
        {
        { "a","あ" },{ "i","い" },{ "u","う" },{ "e","え" },{ "o","お" },

        { "ka","か" },{ "ki","き" },{ "ku","く" },{ "ke","け" },{ "ko","こ" },
        { "kya","きゃ" },{ "kyu","きゅ" },{ "kyo","きょ" },

        { "sa","さ" },{ "shi","し" },{ "su","す" },{ "se","せ" },{ "so","そ" },
        { "sha","しゃ" },{ "shu","しゅ" },{ "sho","しょ" },

        { "ta","た" },{ "chi","ち" },{ "tsu","つ" },{ "te","て" },{ "to","と" },
        { "cha","ちゃ" },{ "chu","ちゅ" },{ "cho","ちょ" },

        { "na","な" },{ "ni","に" },{ "nu","ぬ" },{ "ne","ね" },{ "no","の" },
        { "nya","にゃ" },{ "nyu","にゅ" },{ "nyo","にょ" },
        { "n","ん" },{ "nn","ん" },

        { "ha","は" },{ "hi","ひ" },{ "fu","ふ" },{ "he","へ" },{ "ho","ほ" },
        { "hya","ひゃ" },{ "hyu","ひゅ" },{ "hyo","ひょ" },

        { "ma","ま" },{ "mi","み" },{ "mu","む" },{ "me","め" },{ "mo","も" },
        { "mya","みゃ" },{ "myu","みゅ" },{ "myo","みょ" },

        { "ya","や" },{ "yu","ゆ" },{ "yo","よ" },

        { "ra","ら" },{ "ri","り" },{ "ru","る" },{ "re","れ" },{ "ro","ろ" },
        { "rya","りゃ" },{ "ryu","りゅ" },{ "ryo","りょ" },

        { "wa","わ" },{ "wo","を" },

        { "ga","が" },{ "gi","ぎ" },{ "gu","ぐ" },{ "ge","げ" },{ "go","ご" },
        { "gya","ぎゃ" },{ "gyu","ぎゅ" },{ "gyo","ぎょ" },

        { "za","ざ" },{ "ji","じ" },{ "zu","ず" },{ "ze","ぜ" },{ "zo","ぞ" },
        { "ja","じゃ" },{ "ju","じゅ" },{ "jo","じょ" },

        { "da","だ" },{ "di","ぢ" },{ "du","づ" },{ "de","で" },{ "do","ど" },

        { "ba","ば" },{ "bi","び" },{ "bu","ぶ" },{ "be","べ" },{ "bo","ぼ" },
        { "bya","びゃ" },{ "byu","びゅ" },{ "byo","びょ" },

        { "pa","ぱ" },{ "pi","ぴ" },{ "pu","ぷ" },{ "pe","ぺ" },{ "po","ぽ" },
        { "pya","ぴゃ" },{ "pyu","ぴゅ" },{ "pyo","ぴょ" },
        };

        private static readonly string[] KeysByLength = Map.Keys.OrderByDescending(k => k.Length).ToArray();
        public static string Convert(string romaji)
        {
            if (string.IsNullOrEmpty(romaji)) return string.Empty;
            romaji = romaji.ToLowerInvariant();
            var sb = new StringBuilder(romaji.Length);
            int i = 0;
            while (i < romaji.Length)
            {
                char c = romaji[i];

                if (c == 'n' && i == romaji.Length - 1) 
                {
                    sb.Append(c);
                    i++;
                    continue;
                }
                
                if (i + 1 < romaji.Length
                    && c == romaji[i + 1]
                    && IsLatinConsonant(c)
                    && c != 'n')
                {
                    sb.Append('っ');
                    i++;
                    continue;
                }
                
                bool matched = false;
                foreach (var key in KeysByLength) 
                {
                    if (i + key.Length < romaji.Length) continue;
                    if(string.Compare(romaji, i, key, 0, key.Length,
                        StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        sb.Append(Map[key]);
                        i += key.Length;
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }

        private static bool IsLatinConsonant(char c)
        {
            char lc = char.ToLowerInvariant(c);
            return lc >= 'a' && lc <= 'z' && !"aeiou".Contains(lc);
        }
    }
}
