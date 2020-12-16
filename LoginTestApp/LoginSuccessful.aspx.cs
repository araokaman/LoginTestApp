using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestSharp;
using Newtonsoft.Json;
using System.Text;

namespace LoginTestApp
{
    public partial class LoginSuccessful : System.Web.UI.Page
    {
        string redirectUrl = "https://localhost:44383/LoginSuccessful";
        string redirectUrl2 = "https://localhost:44383/Contact";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["state"] != null)
            {
                string state = (string)Session["state"];
            }
            string uri = Request.Url.AbsoluteUri;
            string code = Request.QueryString["code"];
            string responseState = Request.QueryString["state"];
            string clientId = "";
            string clientSecret = "";
            RestClient client = new RestClient();

            if (Session["sns"] != null)
            {
                switch (Session["sns"])
                {
                    case "line":
                        clientId = "1655374860";
                        clientSecret = "7fffdeae9d1aa3c9f990fc5c41c94cff";
                        client.BaseUrl = new Uri(@"https://api.line.me/oauth2/v2.1/token");
                        break;
                    case "yahoo":
                        clientId = "dj0zaiZpPXU1NjFDc3gxVmtrbCZzPWNvbnN1bWVyc2VjcmV0Jng9NDA-";
                        clientSecret = "85270e1b513a322d420ab726ae319238c9b24bfb";
                        client.BaseUrl = new Uri(@"https://auth.login.yahoo.co.jp/yconnect/v1/token");
                        break;
                }                
            }

            #region
            //client = new RestClient(@"https://api.line.me/oauth2/v2.1/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUrl);
            switch (Session["sns"])
            {
                case "line":
                    request.AddParameter("client_id", clientId);
                    request.AddParameter("client_secret", clientSecret);
                    break;
                case "yahoo":
                    request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));
                    break;
            }
            //request.AddParameter("client_id", clientId);
            //request.AddParameter("client_secret", clientSecret);

            var response = client.Execute(request);
            //var json = JsonConvert.DeserializeObject(response.Content);
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);            

            client = null;
            request = null;
            response = null;

            switch (Session["sns"])
            {
                case "line":
                    client = new RestClient(@"https://api.line.me/v2/profile");
                    break;
                case "yahoo":
                    client = new RestClient(@"https://userinfo.yahooapis.jp/yconnect/v1/attribute?schema=openid");
                    break;
            }
            
            request = new RestRequest(Method.GET);

            switch (Session["sns"])
            {
                case "line":
                    request.AddHeader("Authorization", jsonDictionary["token_type"] + " " + jsonDictionary["access_token"]);
                    break;
                case "yahoo":
                    request.AddHeader("Authorization", jsonDictionary["token_type"] + " " + jsonDictionary["access_token"]);
                    break;
            }
            string token = jsonDictionary["access_token"];
            jsonDictionary.Clear();

            response = client.Execute(request);
            jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            #endregion        
        }
    }
}