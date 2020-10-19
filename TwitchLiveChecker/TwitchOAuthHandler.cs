using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchConfig;

namespace TwitchLiveChecker
{
    class TwitchOAuthHandler
    {
        // https://dev.twitch.tv/docs/authentication/getting-tokens-oauth

        public static Config NewOAuthToken()
        {
            Config config = Config.GetConfig();
            string scope = "user:read:email";

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"{SystemConfig.Environment["redirect_uri"]}/");
            listener.Start();

            var httpclient = new RestClient(SystemConfig.Twitch["loginurl"]);
            httpclient.FollowRedirects = false;

            var request = new RestRequest(Method.GET);
            request.AddParameter("client_id", SystemConfig.Application["clientid"]);
            request.AddParameter("redirect_uri", SystemConfig.Environment["redirect_uri"]);
            request.AddParameter("response_type", "code");
            request.AddParameter("scope", scope);


            IRestResponse response = httpclient.Execute(request);

            string location = response.Headers.ToList().Find(x => x.Name == "Location").Value.ToString();

            Process.Start(new ProcessStartInfo { FileName = location, UseShellExecute = true });

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest callback = context.Request;

            string oauth_challenge = callback.QueryString.Get("code");

            HttpListenerResponse callback_answer = context.Response;
            string responseString = "<HTML><BODY><h1>Success!</h1>You can close this page now!</BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            callback_answer.ContentLength64 = buffer.Length;
            System.IO.Stream output = callback_answer.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            listener.Stop();

            httpclient = new RestClient(SystemConfig.Twitch["tokenurl"]);
            httpclient.FollowRedirects = false;

            request = new RestRequest(Method.POST);
            request.AddParameter("client_id", SystemConfig.Application["clientid"]);
            request.AddParameter("client_secret", SystemConfig.Application["clientsecret"]);
            request.AddParameter("code", oauth_challenge);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", SystemConfig.Environment["redirect_uri"]);

            response = httpclient.Execute(request);

            TwitchOAuthToken oauth_token = JsonConvert.DeserializeObject<TwitchOAuthToken>(response.Content);

            config.oauth["authtoken"] = oauth_token.access_token;
            config.oauth["refreshtoken"] = oauth_token.refresh_token;

            config.Save();

            return config;


        }

        public static Config RefreshOAuthToken()
        {
            Config config = Config.GetConfig();
            RestClient client = new RestClient(SystemConfig.Twitch["tokenurl"]);

            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("client_id", SystemConfig.Application["clientid"]);
            request.AddParameter("client_secret", SystemConfig.Application["clientsecret"]);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", config.oauth["refreshtoken"]);

            IRestResponse response = client.Execute(request);
            TwitchOAuthToken token = JsonConvert.DeserializeObject<TwitchOAuthToken>(response.Content);

            config.oauth["authtoken"] = token.access_token;
            config.oauth["refreshtoken"] = token.refresh_token;

            config.Save();

            return config;
        }

        public static void RevokeOAuthToken()
        {
            Config config = Config.GetConfig();
            RestClient client = new RestClient(SystemConfig.Twitch["revocationurl"]);


            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("client_id", SystemConfig.Application["clientid"]);
            request.AddParameter("token", config.oauth["authtoken"]);

            IRestResponse response = client.Execute(request);

            config.oauth["authtoken"] = "";
            config.oauth["refreshtoken"] = "";

            config.Save();
        }
    }
}
