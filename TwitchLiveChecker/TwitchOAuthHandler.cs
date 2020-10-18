using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitchLiveChecker
{
    class TwitchOAuthHandler
    {

        public static TwitchOAuthToken NewOAuthToken()
        {
            string auth_url = "https://id.twitch.tv/oauth2/authorize";
            string challenge_url = "https://id.twitch.tv/oauth2/token";
            string clientid = "bbloyjh0wzg9fdeplksluv1gs27865";
            string clientsecret = "su3io0qf1bhant8tdxuyxaes5qlxfe";
            string redirect_uri = "http://localhost:58214";
            string scope = "user:read:email";

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"{redirect_uri}/");
            listener.Start();

            var httpclient = new RestClient($"{auth_url}?client_id={clientid}&redirect_uri={redirect_uri}&response_type=code&scope={scope}");
            httpclient.FollowRedirects = false;

            var request = new RestRequest(Method.GET);
            IRestResponse response = httpclient.Execute(request);

            string location = response.Headers.ToList().Find(x => x.Name == "Location").Value.ToString();

            Process.Start(new ProcessStartInfo { FileName = location, UseShellExecute = true });

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest callback = context.Request;

            string oauth_code = callback.QueryString.Get("code");


            HttpListenerResponse callback_answer = context.Response;
            string responseString = "<HTML><BODY><h1>Success!</h1>You can close this page now!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            callback_answer.ContentLength64 = buffer.Length;
            System.IO.Stream output = callback_answer.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            listener.Stop();

            // https://id.twitch.tv/oauth2/token?client_id=bbloyjh0wzg9fdeplksluv1gs27865&client_secret=su3io0qf1bhant8tdxuyxaes5qlxfeazujnyqcn6nbc6mfe9jtw0sxfi12xg&redirect_uri=58214
            httpclient = new RestClient($"{challenge_url}?client_id={clientid}&client_secret={clientsecret}&code={oauth_code}&grant_type=authorization_code&redirect_uri={redirect_uri}");
            httpclient.FollowRedirects = false;

            request = new RestRequest(Method.POST);
            response = httpclient.Execute(request);

            return JsonConvert.DeserializeObject<TwitchOAuthToken>(response.Content);
        }

        public static TwitchOAuthToken RefreshOAuthToken()
        {
            return new TwitchOAuthToken();
        }
    }
}
