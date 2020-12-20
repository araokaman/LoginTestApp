using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace LoginTestApp
{
    public class SocialLogincs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SocialLogincs()
        {

        }

        #region SNSログインボタン押下処理
        /// <summary>
        /// SNSログインボタン押下処理
        /// </summary>
        /// <param name="snsCode">SNSコード</param>
        /// <returns>認可URI、state</returns>
        public Dictionary<string, string> SocialLoginBtnPush(int snsCode)
        {
            string baseUri = string.Empty;
            string clientId = string.Empty;
            string scopeType = string.Empty;

            //SNS別で値を設定
            switch (snsCode)
            {
                //LINE
                case 1:
                    baseUri = ConfigurationManager.AppSettings["lineApiUri1"];
                    clientId = ConfigurationManager.AppSettings["lineApiId"];
                    scopeType = "profile";
                    break;
                //YAHOO
                case 2:
                    baseUri = ConfigurationManager.AppSettings["yahooApiUri1"];
                    clientId = ConfigurationManager.AppSettings["yahooApiId"];
                    scopeType = "openid";
                    break;
            }

            string state = Guid.NewGuid().ToString("N").Substring(0, 30);
            StringBuilder authorizationUri = new StringBuilder();
            authorizationUri.AppendFormat("{0}response_type={1}&client_id={2}&redirect_uri={3}&state={4}&scope={5}", baseUri, "code", clientId, ConfigurationManager.AppSettings["callBackUri"], state, scopeType);

            Dictionary<string, string> pairs = new Dictionary<string, string>();
            pairs.Add("authorizationUri", authorizationUri.ToString());
            pairs.Add("state", state);
            return pairs;
        }
        #endregion

        #region アクセストークン取得処理
        /// <summary>
        /// アクセストークン取得処理
        /// </summary>
        /// <param name="sessionSnsCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAccessToken(int sessionSnsCode, string code)
        {
            RestClient client = new RestClient();
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", ConfigurationManager.AppSettings["callBackUri"]);

            //SNS別で値を設定
            switch (sessionSnsCode)
            {
                //LINE
                case 1:
                    client.BaseUrl = new Uri(ConfigurationManager.AppSettings["lineApiUri2"]);
                    request.AddParameter("client_id", ConfigurationManager.AppSettings["lineApiId"]);
                    request.AddParameter("client_secret", ConfigurationManager.AppSettings["lineApiSecret"]);
                    break;
                //YAHOO
                case 2:
                    client.BaseUrl = new Uri(ConfigurationManager.AppSettings["yahooApiUri2"]);
                    request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["yahooApiId"] + ":" + ConfigurationManager.AppSettings["yahooApiSecret"])));
                    break;
            }

            IRestResponse response = client.Execute(request);
            //Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
        }
        #endregion

        #region SNS側のユーザープロフィール（ユーザーID）を取得する
        /// <summary>
        /// SNS側のユーザープロフィール（ユーザーID）を取得する
        /// </summary>
        /// <param name="sessionSnsCode"></param>
        /// <param name="accessToken"></param>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetUserProfile(int sessionSnsCode, string accessToken, string tokenType)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(Method.GET);
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            //SNS別で値を設定
            switch (sessionSnsCode)
            {
                //LINE
                case 1:
                    client.BaseUrl = new Uri(ConfigurationManager.AppSettings["lineApiUri3"]);
                    break;
                //YAHOO
                case 2:
                    client.BaseUrl = new Uri(ConfigurationManager.AppSettings["yahooApiUri3"]);                    
                    break;
            }
            request.AddHeader("Authorization", tokenType + " " + accessToken);
            IRestResponse response = client.Execute(request);
            //Dictionary<string, string> jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
        }
        #endregion

        #region 指定のアクセストークンを無効化
        public bool InvalidationAccessToken(string accessToken)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("access_token", accessToken);
            request.AddParameter("client_id", ConfigurationManager.AppSettings["lineApiId"]);
            request.AddParameter("client_secret", ConfigurationManager.AppSettings["lineApiSecret"]);

            //LINEのみ必要な処理となるため、LINE用のURIを設定
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["lineApiUri4"]);            

            IRestResponse response = client.Execute(request);
            return response != null ? true : false;
        }
        #endregion

        /// <summary>
        /// 特定のユーザーにLINEメッセージを送信する
        /// </summary>
        /// <param name="lineUserId"></param>
        /// <returns>LINEメッセージ送信の成否</returns>
        public bool SendLineMessage(string lineUserId)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", "Bearer " + ConfigurationManager.AppSettings["lineMessageToken"]);

            LineMessage lineMessage = new LineMessage("text", "Hello, world");
            SendingParty sendingParty = new SendingParty("", lineMessage);      //toUserIdを入力
            var json = JsonConvert.SerializeObject(sendingParty);
            request.AddParameter();

            //LINEのみ必要な処理となるため、LINE用のURIを設定
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["lineApiUri5"]);

            IRestResponse response = client.Execute(request);
        }
    }

    public class SendingParty
    {
        [JsonProperty("to")]
        public string UserId { get; set; }

        [JsonProperty("message")]
        public LineMessage lineMessage { get; set; }

        public SendingParty(string userId, LineMessage message)
        {
            this.UserId = userId;
            this.lineMessage = message;
        }
    }

    public class LineMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        public LineMessage(string type, string text)
        {
            this.Type = type;
            this.Text = text;
        }
    }
}