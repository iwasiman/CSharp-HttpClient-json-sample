using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HttpClientSample
{
    /// <summary>
    /// JSON周りの処理を行うユーティリティのサンプルクラス。
    /// </summary>
    public class JsonUtilSample
    {
        /// <summary>
        /// 入力をJSON文字列に変換します。
        /// </summary>
        /// <param name="dict">Dictionary<string, string>型の入力</param>
        /// <returns>JSON文字列</returns>
        public static string ToJson(Dictionary<string, string> dict)
        {
            var json = JsonSerializer.Serialize(dict, JsonUtilSample.GetOption());
            return json;
        }

        /// <summary>
        /// 入力をJSON文字列に変換します。
        /// </summary>
        /// <param name="dict">Dictionary<string, int>型の入力</param>
        /// <returns>JSON文字列</returns>
        public static string ToJson(Dictionary<string, int> dict)
        {
            var json = JsonSerializer.Serialize(dict, JsonUtilSample.GetOption());
            return json;
        }

        /// <summary>
        /// オプションを設定します。内部メソッドです。
        /// </summary>
        /// <returns>JsonSerializerOptions型のオプション</returns>
        private static JsonSerializerOptions GetOption()
        {
            // ユニコードのレンジ指定で日本語も正しく表示、インデントされるように指定
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            };
            return options;
        }

        /// <summary>
        /// 入力のJSON文字列をDictionary型に変換します。
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>Dictionary<string, string>型の出力(入力が異常の場合は空のオブジェクト)</returns>
        public static Dictionary<string, string> JsonToDict(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new Dictionary<string, string>();
            }
            try
            {
                Dictionary<string, string> dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json, JsonUtilSample.GetOption());
                return dict;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 入力をJSON文字列に変換します。
        /// </summary>
        /// <param name="poco">定義済みのクラスオブジェクト</param>
        /// <returns>JSON文字列 (入力が異常な場合はnull)</returns>
        public static string ToJson(Object poco)
        {
            try
            {
                var json = JsonSerializer.Serialize(poco, JsonUtilSample.GetOption());
                return json;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 入力のJSON文字列をクラスに変換します。
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>SampleUserPoco型の出力</returns>
        public static SampleUserPoco JsonToSampleUserPoco(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }
            try
            {
                SampleUserPoco poco = JsonSerializer.Deserialize<SampleUserPoco>(json, JsonUtilSample.GetOption());
                return poco;
            }
            catch (JsonException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
