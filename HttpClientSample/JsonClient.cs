using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HttpClientSample
{
    // JSONの変換クラスを呼びだす側のコードのサンプルです。
    public class JsonClient
    {
        static void Main(string[] args)
        {
            JsonClient.processSimpleDictToJson();
            JsonClient.processSimpleJsonToDict();
            JsonClient.procesPocoToJson();
            JsonClient.processJsonToSampleUserPoco();
        }

        private static void processSimpleDictToJson()
        {
            var dict1 = new Dictionary<string, string>();
            dict1.Add("fruits-1", "いちご");
            dict1.Add("fruits-2", "りんご");
            Console.WriteLine(JsonUtilSample.ToJson(dict1));

            var dict2 = new Dictionary<string, int>();
            dict2.Add("国語", 80);
            dict2.Add("英語", 90);
            Console.WriteLine(JsonUtilSample.ToJson(dict2));
        }

        private static void processSimpleJsonToDict()
        {
            string jsonStr = @"
{
  ""fruits-3"": ""バナナ"",
  ""fruits-4"": ""ぶどう""
}
";
            var dict = JsonUtilSample.JsonToDict(jsonStr);
            foreach (string key in dict.Keys)
            {
                Console.WriteLine("キー: " + key + " 値: " + dict[key]);
            }
        }

        private static void procesPocoToJson()
        {
            // C# 3.0から使えるオブジェクト初期化子を使ってオブジェクトを生成してみる
            var poco = new SampleUserPoco {Token = "token_qwertyuiop@[", UserName = "Alice", IsExcellent = true,
                SomeIntValue = 10, SomeDoubleValue = 12.345678901};
            var kvList = new List<SampleKvPoco>();
            kvList.Add(new SampleKvPoco { Key = "key-1", Value = "value-1" });
            kvList.Add(new SampleKvPoco { Key = "key-2", Value = "value-2" });
            poco.Kvs = kvList;
            // ポコッと作ったオブジェクトをJSONに変換！
            Console.WriteLine(JsonUtilSample.ToJson(poco));
        }

        private static void processJsonToSampleUserPoco()
        {
            string jsonStr = @"
{
  ""token"": ""token_qwertyuiop@["",
  ""userName"": ""Alice"",
  ""isExcellent"": true,
  ""someIntValue"": 10,
  ""someDoubleValue"": null,
  ""kvs"": [
    {
      ""key"": ""key-1"",
      ""value"": ""value-1""
    },
    {
      ""key"": ""key-2"",
      ""value"": ""value-2""
    }
  ]
}
";
            var poco = JsonUtilSample.JsonToSampleUserPoco(jsonStr);
            Console.WriteLine("---クラスオブジェクトに変換した結果"
                + " token:"+ poco.Token + " userName:" + poco.UserName
                + " isExcellent:" + poco.IsExcellent + " someIntValue:" + poco.SomeIntValue
                + " someDoubleValue:" + poco.SomeDoubleValue + " kvs count:" + poco.Kvs.Count
                );
        }
    }
}
