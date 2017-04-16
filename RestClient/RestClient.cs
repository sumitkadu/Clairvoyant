using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestClient
{
    public class RestClient
    {
        public static LogInformation GetData(string payload, string url)
        {
            return MakeRequest(url, payload);            
        }

        public static LogInformation MakeRequest(string requestUrl, string payload)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            try
            {
                string data = string.Empty;
                request.Method = "POST";
                request.ContentType = request.Accept = "application/json";
                request.ContentLength = payload.Length;
                using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter.Write(payload);
                }

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    data = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<LogInformation>(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetLog(string requestUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            try
            {
                string data = string.Empty;     
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    data = reader.ReadToEnd();
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}