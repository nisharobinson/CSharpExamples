using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

using JsonSerialization.Methods;
using JsonSerialization.DataContracts;

namespace JsonSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            ResponseBodyIssNow responseBodyIssNow;
            string responseBodyIssNowFormattedString;

            if (Json.IssNow(out responseBodyIssNow))
                if (Formatters.IssNow(responseBodyIssNow, out responseBodyIssNowFormattedString))
                    Console.WriteLine(responseBodyIssNowFormattedString);

            Console.WriteLine("Press enter to continue");
            Console.ReadKey();
            Console.WriteLine();

            ResponseBodyXkcdComic responseBodyXkcdComic;
            string responseBodyXkcdComicFormattedString;

            if (Json.XkcdComic(out responseBodyXkcdComic))
                if (Formatters.XkcdComic(responseBodyXkcdComic, out responseBodyXkcdComicFormattedString))
                    Console.WriteLine(responseBodyXkcdComicFormattedString);

            Console.WriteLine("Press enter to continue");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}

namespace JsonSerialization.Methods
{
    public class WebRequests
    {
        public static Stream HttpWebRequest(string uri, string method)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new UriBuilder(uri).Uri);

            webRequest.Method = "GET";

            // HttpWebRequest's Timeout and ReadWriteTimeout -
            // What do these mean for the underlying TCP connection? https://stackoverflow.com/a/9410188
            webRequest.Timeout = (60 * 1000);
            webRequest.ReadWriteTimeout = (60 * 1000);

            // HttpWebRequest times out https://stackoverflow.com/a/10659400
            webRequest.ServicePoint.MaxIdleTime = (10 * 1000);
            // Adjusting HttpWebRequest Connection Timeout in C# https://stackoverflow.com/a/1501642
            webRequest.ServicePoint.ConnectionLimit = 100;

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();

            return stream;
        }
    }

    public class Json
    {
        public static bool IssNow(out ResponseBodyIssNow responseBody)
        {
            using (Stream stream = WebRequests.HttpWebRequest("http://api.open-notify.org/iss-now.json", "GET"))
            {
                responseBody = (ResponseBodyIssNow)new DataContractJsonSerializer(typeof(ResponseBodyIssNow)).ReadObject(stream);
            }
            return true;
        }

        public static bool XkcdComic(out ResponseBodyXkcdComic responseBody)
        {
            using (Stream stream = WebRequests.HttpWebRequest("https://xkcd.com/info.0.json", "GET"))
            {
                responseBody = (ResponseBodyXkcdComic)new DataContractJsonSerializer(typeof(ResponseBodyXkcdComic)).ReadObject(stream);
            }
            return true;
        }
    }

    public class Formatters
    {
        public static bool IssNow(ResponseBodyIssNow responseBodyIssNow, out string formattedString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("responseBodyIssNow").AppendLine();
            sb.Append("Timestamp: ").Append(responseBodyIssNow.Timestamp).AppendLine();
            sb.Append("Message: ").Append(responseBodyIssNow.Message).AppendLine();
            sb.Append("ResponseBodyIssNow_IssPosition.Latitude: ").Append(responseBodyIssNow.IssPosition.Latitude).AppendLine();
            sb.Append("ResponseBodyIssNow_IssPosition.Longitude: ").Append(responseBodyIssNow.IssPosition.Longitude).AppendLine();
            formattedString = sb.ToString();
            return true;
        }

        public static bool XkcdComic(ResponseBodyXkcdComic responseBodyXkcdComic, out string formattedString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("responseBodyXkcdComic").AppendLine();
            sb.Append("Month: ").Append(responseBodyXkcdComic.Month).AppendLine();
            sb.Append("Number: ").Append(responseBodyXkcdComic.Number).AppendLine();
            sb.Append("Year: ").Append(responseBodyXkcdComic.Year).AppendLine();
            sb.Append("News: ").Append(responseBodyXkcdComic.News).AppendLine();
            sb.Append("SafeTitle: ").Append(responseBodyXkcdComic.SafeTitle).AppendLine();
            sb.Append("Transcript: ").Append(responseBodyXkcdComic.Transcript).AppendLine();
            sb.Append("Alt: ").Append(responseBodyXkcdComic.Alt).AppendLine();
            sb.Append("Img: ").Append(responseBodyXkcdComic.Img).AppendLine();
            sb.Append("Title: ").Append(responseBodyXkcdComic.Title).AppendLine();
            sb.Append("Day: ").Append(responseBodyXkcdComic.Month).AppendLine();
            formattedString = sb.ToString();
            return true;
        }

        public static string HttpWebRequestProperties(HttpWebRequest webRequest)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("webRequest.Accept: ").Append(webRequest.Accept).AppendLine();
            sb.Append("webRequest.Address: ").Append(webRequest.Address).AppendLine();
            sb.Append("webRequest.AllowAutoRedirect: ").Append(webRequest.AllowAutoRedirect).AppendLine();
            sb.Append("webRequest.AllowWriteStreamBuffering: ").Append(webRequest.AllowWriteStreamBuffering).AppendLine();
            sb.Append("webRequest.AuthenticationLevel: ").Append(webRequest.AuthenticationLevel).AppendLine();
            sb.Append("webRequest.AutomaticDecompression: ").Append(webRequest.AutomaticDecompression).AppendLine();
            sb.Append("webRequest.CachePolicy: ").Append(webRequest.CachePolicy).AppendLine();
            sb.Append("webRequest.ClientCertificates: ").Append(webRequest.ClientCertificates).AppendLine();
            sb.Append("webRequest.Connection: ").Append(webRequest.Connection).AppendLine();
            sb.Append("webRequest.ConnectionGroupName: ").Append(webRequest.ConnectionGroupName).AppendLine();
            sb.Append("webRequest.ContentLength: ").Append(webRequest.ContentLength).AppendLine();
            sb.Append("webRequest.ContentType: ").Append(webRequest.ContentType).AppendLine();
            sb.Append("webRequest.ContinueDelegate: ").Append(webRequest.ContinueDelegate).AppendLine();
            sb.Append("webRequest.CookieContainer: ").Append(webRequest.CookieContainer).AppendLine();
            sb.Append("webRequest.Credentials: ").Append(webRequest.Credentials).AppendLine();
            sb.Append("webRequest.Expect: ").Append(webRequest.Expect).AppendLine();
            sb.Append("webRequest.HaveResponse: ").Append(webRequest.HaveResponse).AppendLine();
            sb.Append("webRequest.Headers: ").Append(webRequest.Headers).AppendLine();
            sb.Append("webRequest.IfModifiedSince: ").Append(webRequest.IfModifiedSince).AppendLine();
            sb.Append("webRequest.ImpersonationLevel: ").Append(webRequest.ImpersonationLevel).AppendLine();
            sb.Append("webRequest.KeepAlive: ").Append(webRequest.KeepAlive).AppendLine();
            sb.Append("webRequest.MaximumAutomaticRedirections: ").Append(webRequest.MaximumAutomaticRedirections).AppendLine();
            sb.Append("webRequest.MaximumResponseHeadersLength: ").Append(webRequest.MaximumResponseHeadersLength).AppendLine();
            sb.Append("webRequest.MediaType: ").Append(webRequest.MediaType).AppendLine();
            sb.Append("webRequest.Method: ").Append(webRequest.Method).AppendLine();
            sb.Append("webRequest.Pipelined: ").Append(webRequest.Pipelined).AppendLine();
            sb.Append("webRequest.PreAuthenticate: ").Append(webRequest.PreAuthenticate).AppendLine();
            sb.Append("webRequest.ProtocolVersion: ").Append(webRequest.ProtocolVersion).AppendLine();
            sb.Append("webRequest.Proxy: ").Append(webRequest.Proxy).AppendLine();
            sb.Append("webRequest.ReadWriteTimeout: ").Append(webRequest.ReadWriteTimeout).AppendLine();
            sb.Append("webRequest.Referer: ").Append(webRequest.Referer).AppendLine();
            sb.Append("webRequest.RequestUri: ").Append(webRequest.RequestUri).AppendLine();
            sb.Append("webRequest.SendChunked: ").Append(webRequest.SendChunked).AppendLine();
            sb.Append("webRequest.ServicePoint: ").Append(webRequest.ServicePoint).AppendLine();
            sb.Append("webRequest.ServicePoint.Address: ").Append(webRequest.ServicePoint.Address).AppendLine();
            sb.Append("webRequest.ServicePoint.BindIPEndPointDelegate: ").Append(webRequest.ServicePoint.BindIPEndPointDelegate).AppendLine();
            sb.Append("webRequest.ServicePoint.Certificate: ").Append(webRequest.ServicePoint.Certificate).AppendLine();
            sb.Append("webRequest.ServicePoint.ClientCertificate: ").Append(webRequest.ServicePoint.ClientCertificate).AppendLine();
            sb.Append("webRequest.ServicePoint.ConnectionLeaseTimeout: ").Append(webRequest.ServicePoint.ConnectionLeaseTimeout).AppendLine();
            sb.Append("webRequest.ServicePoint.ConnectionLimit: ").Append(webRequest.ServicePoint.ConnectionLimit).AppendLine();
            sb.Append("webRequest.ServicePoint.ConnectionName: ").Append(webRequest.ServicePoint.ConnectionName).AppendLine();
            sb.Append("webRequest.ServicePoint.CurrentConnections: ").Append(webRequest.ServicePoint.CurrentConnections).AppendLine();
            sb.Append("webRequest.ServicePoint.Expect100Continue: ").Append(webRequest.ServicePoint.Expect100Continue).AppendLine();
            sb.Append("webRequest.ServicePoint.IdleSince: ").Append(webRequest.ServicePoint.IdleSince).AppendLine();
            sb.Append("webRequest.ServicePoint.MaxIdleTime: ").Append(webRequest.ServicePoint.MaxIdleTime).AppendLine();
            sb.Append("webRequest.ServicePoint.ProtocolVersion: ").Append(webRequest.ServicePoint.ProtocolVersion).AppendLine();
            sb.Append("webRequest.ServicePoint.ReceiveBufferSize: ").Append(webRequest.ServicePoint.ReceiveBufferSize).AppendLine();
            sb.Append("webRequest.ServicePoint.SupportsPipelining: ").Append(webRequest.ServicePoint.SupportsPipelining).AppendLine();
            sb.Append("webRequest.ServicePoint.UseNagleAlgorithm: ").Append(webRequest.ServicePoint.UseNagleAlgorithm).AppendLine();
            sb.Append("webRequest.Timeout: ").Append(webRequest.Timeout).AppendLine();
            sb.Append("webRequest.TransferEncoding: ").Append(webRequest.TransferEncoding).AppendLine();
            sb.Append("webRequest.UnsafeAuthenticatedConnectionSharing: ").Append(webRequest.UnsafeAuthenticatedConnectionSharing).AppendLine();
            sb.Append("webRequest.UseDefaultCredentials: ").Append(webRequest.UseDefaultCredentials).AppendLine();
            sb.Append("webRequest.UseDefaultCredentials: ").Append(webRequest.UseDefaultCredentials).AppendLine();
            sb.Append("webRequest.UserAgent: ").Append(webRequest.UserAgent).AppendLine();
            return sb.ToString();
        }
    }
}

namespace JsonSerialization.DataContracts
{
    [DataContract]
    public class ResponseBodyIssNow_IssPosition
    {
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }
    }

    [DataContract]
    public class ResponseBodyIssNow
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }

        [DataMember(Name = "iss_position")]
        public ResponseBodyIssNow_IssPosition IssPosition { get; set; }
    }

    [DataContract]
    public class ResponseBodyXkcdComic
    {
        [DataMember(Name = "month")]
        public string Month { get; set; }

        [DataMember(Name = "num")]
        public int Number { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }

        [DataMember(Name = "news")]
        public string News { get; set; }

        [DataMember(Name = "safe_title")]
        public string SafeTitle { get; set; }

        [DataMember(Name = "transcript")]
        public string Transcript { get; set; }

        [DataMember(Name = "alt")]
        public string Alt { get; set; }

        [DataMember(Name = "img")]
        public string Img { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "day")]
        public string Day { get; set; }
    }
}
