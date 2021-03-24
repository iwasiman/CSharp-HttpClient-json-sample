using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace HttpClientSample
{
    /// <summary>
    /// SampleServiceとの通信を担当する、HTTPクライアントのサンプル。
    /// </summary>
    public class SampleServiceHttpClient
    {
        /// <summary>
        /// 通信先のベースURL
        /// </summary>
        private readonly string baseUrl;
        /// <summary>
        /// C#側のHttpクライアント
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// デフォルトコンストラクタ。外部からは呼び出せません。
        /// </summary>
        private SampleServiceHttpClient()
        {
        }

        /// <summary>
        /// 引数付きのコンストラクタ。こちらを使用します。
        /// 引数には正しいURLが入っていることが前提です。
        /// </summary>
        /// <param name="baseUrl">ベースのURL</param>
        public SampleServiceHttpClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
            // 通信するメソッドでその都度HttpClientをnewすると毎回ソケットを開いてリソースを消費するため、
            // メンバ変数で使い回す手法を取っています。
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// 情報がURLに載ったGETリクエストを送受信するサンプル。
        /// </summary>
        /// <param name="someId">何かのID</param>
        /// <returns>正常：レスポンスのボディ / 異常：null</returns>
        public string GetSample(string someId)
        {
            String requestEndPoint = this.baseUrl + "/some/search/?someId=" + someId;
            HttpRequestMessage request = this.CreateRequest(HttpMethod.Get, requestEndPoint);

            string resBodyStr;
            HttpStatusCode resStatusCoode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> response;
            // 通信実行。メンバ変数でhttpClientを持っているので、using(～)で囲いません。囲うと通信後にオブジェクトが破棄されます。
            // 引数にrequestを取る場合はGetAsyncやPostAsyncでなくSendAsyncメソッドになります。
            // 戻り値はTask<HttpResponseMessage>で、変数名.ResultとするとSystem.Net.Http.HttpResponseMessageクラスが取れます。
            try
            {
                response = httpClient.SendAsync(request);
                resBodyStr = response.Result.Content.ReadAsStringAsync().Result;
                resStatusCoode = response.Result.StatusCode;
            }
            catch (HttpRequestException e)
            {
                // UNDONE: 通信失敗のエラー処理
                return null;
            }

            if (!resStatusCoode.Equals(HttpStatusCode.OK))
            {
                // UNDONE: レスポンスが200 OK以外の場合のエラー処理
                return null;
            }
            if (String.IsNullOrEmpty(resBodyStr))
            {
                // UNDONE: レスポンスのボディが空の場合のエラー処理
                return null;
            }
            // 中身のチェックなどを経て終了。
            return resBodyStr;
        }

        /// <summary>
        /// URLに情報を持ってDELETEを送受信するサンプル。
        /// </summary>
        /// <param name="someId">何かのID</param>
        /// <returns>正常：固定文字列 / 異常：null</returns>
        public string DeleteSample(string someId)
        {
            String requestEndPoint = this.baseUrl + "some/" + someId;
            HttpRequestMessage request = this.CreateRequest(HttpMethod.Delete, requestEndPoint);

            HttpStatusCode resStatusCoode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> response;
            String resBodyStr;

            try
            {
                response = httpClient.SendAsync(request);
                resBodyStr = response.Result.Content.ReadAsStringAsync().Result;
                resStatusCoode = response.Result.StatusCode;
            }
            catch (HttpRequestException e)
            {
                // UNDONE: 通信失敗のエラー処理
                return null;
            }

            if (!resStatusCoode.Equals(HttpStatusCode.OK))
            {
                // UNDONE: レスポンスが200 OK以外の場合のエラー処理
                return null;
            }
            if (String.IsNullOrEmpty(resBodyStr))
            {
                // UNDONE: レスポンスのボディが空の場合のエラー処理
                return null;
            }
            return someId + "の削除に成功したんだが!";
        }

        /// <summary>
        /// ボディに文字列のキーをJSONで持ってPOSTを送受信するサンプルです。
        /// </summary>
        /// <param name="someKey">何かのキーとか</param>
        /// <returns>正常：レスポンスのボディ / 異常：null</returns>
        public string PostWithStringBodySample(string someKey)
        {
            String requestEndPoint = this.baseUrl + "some/post";
            var request = this.CreateRequest(HttpMethod.Post, requestEndPoint);
            var jsonDict = new Dictionary<string, string>()
                {
                    {"someKey", someKey},
                };
            string reqBodyJson = JsonSerializer.Serialize(jsonDict, this.GetJsonOption());
            var content = new StringContent(reqBodyJson, Encoding.UTF8, @"application/json");
            request.Content = content;

            string resBodyStr;
            HttpStatusCode resStatusCoode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> response;
            try
            {
                response = httpClient.SendAsync(request);
                resBodyStr = response.Result.Content.ReadAsStringAsync().Result;
                resStatusCoode = response.Result.StatusCode;
            }
            catch (HttpRequestException e)
            {
                // UNDONE: 通信失敗のエラー処理
                return null;
            }

            if (!resStatusCoode.Equals(HttpStatusCode.OK))
            {
                // UNDONE: レスポンスが200 OK以外の場合のエラー処理
                return null;
            }
            if (String.IsNullOrEmpty(resBodyStr))
            {
                // UNDONE: レスポンスのボディが空の場合のエラー処理
                return null;
            }
            // 中身を取り出したりして終了
            return resBodyStr;
        }

        /// <summary>
        /// オプションを設定します。内部メソッドです。
        /// </summary>
        /// <returns>JsonSerializerOptions型のオプション</returns>
        private JsonSerializerOptions GetJsonOption()
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
        /// バイナリファイルなどをボディに持ってPOSTを送受信するサンプルです。
        /// </summary>
        /// <param name="filePath">アップロードするファイルのファイルパス</param>
        /// <returns>正常：レスポンスのボディ / 異常：null</returns>
        public string PostWithPdfFileBodySample(string filePath)
        {
            String requestEndPoint = this.baseUrl + "resume/upload";
            HttpRequestMessage request = this.CreateRequest(HttpMethod.Post, requestEndPoint);
            // こうした場合、Accept: multipart/form-data を指定となっていることが多いです。
            request.Headers.Remove("Accept");
            request.Headers.Add("Accept", "multipart/form-data");

            var content = new MultipartFormDataContent();
            // ☆生成されるボディ部
            // Content-Type: multipart/form-data; boundary = "{MultipartFormDataContentクラスが自動で設定}"
            // Content-Length: {MultipartFormDataContentクラスが自動で設定}

            // ボディに--boundaryで区切られたマルチパートのデータを追加
            var multiDocumentsContent = new StringContent("hoge");
            content.Add(multiDocumentsContent, "hogePart");
            // ☆生成されるボディ部
            // --boundary
            // Content-Type: text/plain; charset=utf-8
            // Content-Disposition: form-data; name=hogePart
            //
            // hoge

            StreamContent streamContent = null;

            HttpStatusCode resStatusCoode = HttpStatusCode.NotFound;
            Task<HttpResponseMessage> response;
            String resBodyStr;

            using (var fileStream = File.OpenRead(filePath))
            {
                streamContent = new StreamContent(fileStream);
                // 以下のコードで、{Content-Disposition: form-data; name=file; filename="{ファイル名}"] が出来上がります。
                //streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                //{
                //    Name = "file",
                //    FileName = Path.GetFileName(filePath)
                //};
                // しかしファイル名が2バイト文字だと化けてしまうので、手動でエンコードしたfilenameを追加したヘッダーを別に作ります。
                var finfo = new FileInfo(filePath);
                string headerStr = string.Format("form-data; name=\"file\"; filename=\"{0}\"", finfo.Name);
                byte[] headerValueByteArray = Encoding.UTF8.GetBytes(headerStr);
                var encodedHeaderValue = new StringBuilder();
                foreach (byte b in headerValueByteArray)
                {
                    encodedHeaderValue.Append((char)b);
                }
                streamContent.Headers.ContentDisposition = null; // デフォルトで用意されているので一旦削除
                streamContent.Headers.Add("Content-Disposition", encodedHeaderValue.ToString());
                // この例ではPDFファイルを想定しています。
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                streamContent.Headers.Add("Content-Length", fileStream.Length.ToString());
                content.Add(streamContent, "file");
                // ☆生成されるボディ部
                // --boundary
                // Content-Disposition: form-data; name="file"; filename="{エンコードされたファイル名}"
                // Content-Type: application/pdf
                // Content-Length: {上で計算された値}
                //
                // {バイナリファイルの実体}
                // --boundary--

                // ２つの部分が加えられたボディ部をリクエストと一緒にして、送信
                request.Content = content;

                try
                {
                    response = httpClient.SendAsync(request);
                    resBodyStr = response.Result.Content.ReadAsStringAsync().Result;
                    resStatusCoode = response.Result.StatusCode;
                }
                catch (HttpRequestException e)
                {
                    // UNDONE: 通信失敗のエラー処理
                    return null;
                }
                fileStream.Close();
            }

            if (!resStatusCoode.Equals(HttpStatusCode.OK))
            {
                // UNDONE: レスポンスが200 OK以外の場合のエラー処理
                return null;
            }
            if (String.IsNullOrEmpty(resBodyStr))
            {
                // UNDONE: レスポンスのボディが空の場合のエラー処理
                return null;
            }
            // 中身のチェックなどを経て終了。
            return resBodyStr;
        }

        /// <summary>
        /// HTTPリクエストメッセージを生成する内部メソッドです。
        /// </summary>
        /// <param name="httpMethod">HTTPメソッドのオブジェクト</param>
        /// <param name="requestEndPoint">通信先のURL</param>
        /// <returns>HttpRequestMessage</returns>
        private HttpRequestMessage CreateRequest(HttpMethod httpMethod, string requestEndPoint)
        {
            var request = new HttpRequestMessage(httpMethod, requestEndPoint);
            return this.AddHeaders(request);
        }

        /// <summary>
        /// HTTPリクエストにヘッダーを追加する内部メソッドです。
        /// </summary>
        /// <param name="request">リクエスト</param>
        /// <returns>HttpRequestMessage</returns>
        private HttpRequestMessage AddHeaders(HttpRequestMessage request)
        {
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept-Charset", "utf-8");
            // 同じようにして、例えば認証通過後のトークンが "Authorization: Bearer {トークンの文字列}"
            // のように必要なら適宜追加していきます。
            return request;
        }
    }
}
