using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HttpClientSample
{
    /// <summary>
    /// ユーザー情報を表すJSON全体に対応したPOCOなクラスの例。
    /// フィールド(メンバ変数名)がJSONのキーと対応しています。このクラスはデータのみで処理を行いません。
    /// </summary>
    public class SampleUserPoco
    {
        // 文字列型
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        // 真偽値型
        [JsonPropertyName("isExcellent")]
        public bool IsExcellent { get; set; }
        // 数値型
        [JsonPropertyName("someIntValue")]
        public int SomeIntValue { get; set; }
        [JsonPropertyName("someDoubleValue")]
        public double? SomeDoubleValue { get; set; }
        // リスト型
        [JsonPropertyName("kvs")]
        public IList<SampleKvPoco> Kvs { get; set; }
    }
}
