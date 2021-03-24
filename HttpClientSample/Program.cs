using System;

namespace HttpClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HttpClientのサンプルを呼び出すよ!");
            var client = new SampleServiceHttpClient("https://sample-service.com/api/");
            client.GetSample("someId-1");
            client.DeleteSample("someId-1");
            client.PostWithStringBodySample("someKey-123");
            client.PostWithPdfFileBodySample(@"C:\temp\履歴書だワン.pdf");
        }
    }
}
