using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;

namespace BonitaRestApi
{
    public class BonitaApi
    {
        [Test]
        public async void Test()
        {
            const string url = "http://enlt027.clinicient.local:8081/bonita/";

            var cookies = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            using (var client = new HttpClient(handler)) 
            {
                var uri = new Uri(url);
                client.BaseAddress = uri;
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("username", ""), 
                    new KeyValuePair<string, string>("password", ""), 
                    new KeyValuePair<string, string>("redirect", "true"), 
                    new KeyValuePair<string, string>("redirectUrl", ""), 
                });

                HttpResponseMessage response = await client.PostAsync("loginservice", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseCookies = cookies.GetCookies(uri);

                    var sessionID = responseCookies["JSESSIONID"];

                    // Once we have a successful session ID, we should attempt to logout to relieve the server
                    try
                    {

                        Trace.WriteLine(string.Format("Retrieved session ID {0}", sessionID));


                        // Do useful work 
                        //client.PostAsync("humanTask", new HttpContent() { new { }}); 


                    }
                    finally
                    {
                        var result = client.GetAsync("logoutservice").Result;
                        if (result.IsSuccessStatusCode)
                        {
                            Trace.WriteLine("Successfully Logged out.");
                        }
                    }

                }
            }
        }
    }
}
