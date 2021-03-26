using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace HttpClientSample
{
    /// <summary>
    /// キーと値を持つJSON全体に対応したPOCOなクラスの例。
    /// フィールド(メンバ変数名)がJSONのキーと対応しています。このクラスはデータのみで処理を行いません。
    /// </summary>
    public class SampleKvPoco
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
