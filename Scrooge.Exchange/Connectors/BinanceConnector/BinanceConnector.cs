using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scrooge.Exchange.Connectors.BinanceConnector
{
    /// <summary>
    /// Connector for Binance exchange. Taken from https://github.com/ilkerulutas/BinanceSdk
    /// </summary>
    public partial class BinanceConnector : IConnector
    {
        //FIXME: make initialization from config
        private const string BaseUrl = "https://api.binance.com/api";  

        private readonly string _apiKey;
        private readonly string _apiSecret;

        public enum ApiMethodType
        {
            None = 0,
            ApiKey = 1,
            Signed = 2
        }
        
        protected delegate T ResponseParseHandler<T>(string input);

        protected async Task<T> SendRequest<T>(string methodName, string version, ApiMethodType apiMethod, HttpMethod httpMethod,
            Dictionary<string, string> parameters = null, ResponseParseHandler<T> customHandler = null)
        {

            if ((apiMethod == ApiMethodType.ApiKey && string.IsNullOrEmpty(_apiKey)) ||
                (apiMethod == ApiMethodType.Signed &&
                 (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_apiSecret))))
            {
                throw new Exception("You have to instantiate client with proper keys in order to make ApiKey or Signed API requests!");
            }

            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            if (apiMethod == ApiMethodType.Signed)
            {
                var timestamp = Util.GetCurrentMilliseconds();
                parameters.Add("timestamp", timestamp.ToString(CultureInfo.InvariantCulture));

                var parameterTextForSignature = GetParameterText(parameters);
                var signedBytes = Util.Sign(_apiSecret, parameterTextForSignature);
                parameters.Add("signature", Util.GetHexString(signedBytes));
            }

            var parameterTextPrefix = parameters.Count > 0 ? "?" : string.Empty;
            var parameterText = GetParameterText(parameters);

            string response;

            using (var client = new WebClient())
            {
                if (apiMethod == ApiMethodType.Signed || apiMethod == ApiMethodType.ApiKey)
                {
                    client.Headers.Add("X-MBX-APIKEY", _apiKey);
                }

                try
                {
                    var getRequestUrl = $"{BaseUrl}/{version}/{methodName}{parameterTextPrefix}{parameterText}";
                    var postRequestUrl = $"{BaseUrl}/{version}/{methodName}";

                    response = httpMethod == HttpMethod.Get ?
                        await client.DownloadStringTaskAsync(getRequestUrl) :
                        await client.UploadStringTaskAsync(postRequestUrl, httpMethod.Method, parameterText);
                }
                catch (WebException webException)
                {
                    using (var reader = new StreamReader(webException.Response.GetResponseStream(), Encoding.UTF8))
                    {
                        var errorObject = JObject.Parse(reader.ReadToEnd());
                        var errorCode = (int)errorObject["code"];
                        var errorMessage = (string)errorObject["msg"];
                        throw new Exception($"{errorCode}{errorMessage}");
                    }
                }
            }
            return customHandler != null ? customHandler(response) : JsonConvert.DeserializeObject<T>(response);
        }

        private static string GetParameterText(Dictionary<string, string> parameters)
        {
            if (parameters.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var item in parameters)
            {
                builder.Append($"&{item.Key}={item.Value}");
            }
            return builder.Remove(0, 1).ToString();
        }
    }
}
