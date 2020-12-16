using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using RestSharp;
using System.Text;

namespace LoginTestApp
{
    public partial class _Default : Page
    {
        string client_id;
        string client_secret;
        string response_type = "code";
        //string redirectUrl = "https://r25.jp/";
        string redirectUrl = "https://localhost:44383/LoginSuccessful";

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            int count = Request.Cookies.Count;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string lineBaseUrl = @"https://access.line.me/oauth2/v2.1/authorize?";
            string lineClientId = "1655374860";
            string random = Guid.NewGuid().ToString("N").Substring(0, 30);
            string lineLoginUrl = lineBaseUrl + string.Format("response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope={4}"
                                                                , response_type, lineClientId, redirectUrl, random, "profile");
            Session["state"] = random;
            Session["sns"] = "line";
            Response.Redirect(lineLoginUrl);
            
            #region
            //var query = System.Web.HttpUtility.ParseQueryString("");
            //query.Add("response_type", response_type);
            //query.Add("client_id", lineClientId);
            //query.Add("redirect_url", redirectUrl);
            //query.Add("state", random);
            //query.Add("scope", "profile");

            //var uriBuilder = new System.UriBuilder("access.line.me/oauth2/v2.1/authorize")
            //{
            //    Query = query.ToString(), Scheme = Uri.UriSchemeHttps
            //};

            //Response.Redirect(uriBuilder.Uri.ToString());
            #endregion
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string yahooBaseUrl = @"https://auth.login.yahoo.co.jp/yconnect/v1/authorization?";
            string yahooClientId = "dj0zaiZpPXU1NjFDc3gxVmtrbCZzPWNvbnN1bWVyc2VjcmV0Jng9NDA-";
            string random = Guid.NewGuid().ToString("N").Substring(0, 30);
            string yahooLoginUrl = yahooBaseUrl + string.Format("response_type={0}&client_id={1}&redirect_uri={2}&scope={3}&state={4}"
                                                                , response_type, yahooClientId, redirectUrl, "openid", random);
            Session["state"] = random;
            Session["sns"] = "yahoo";
            Response.Redirect(yahooLoginUrl);
        }
    }
}