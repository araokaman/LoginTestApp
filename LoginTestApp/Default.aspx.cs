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
        //http://192.168.3.4:8080/Default

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //セッション変数代入用
                int snsCode = 0;
                string state = string.Empty;
                
                if(Session["sns"] != null && Session["state"] != null)
                {
                    snsCode = (int)Session["sns"];
                    state = (string)Session["state"];

                    if (state.Equals(Request.QueryString["state"]))
                    {
                        try
                        {
                            //アクセストークンを取得
                            SocialLogincs socialLogincs = new SocialLogincs();
                            Dictionary<string, string> responsePair = socialLogincs.GetAccessToken(snsCode, Request.QueryString["code"]);

                            if (responsePair.ContainsKey("access_token") && responsePair.ContainsKey("token_type"))
                            {
                                //SNS別のユーザーIDを取得
                                Dictionary<string, string> responseUserProfile = socialLogincs.GetUserProfile(snsCode, responsePair["access_token"], responsePair["token_type"]);

                                //会員IDの検証
                                int csfUserId = 0;      //SNSユーザーIDからCSF会員IDを抽出

                                //SNSがLINEモバイルの場合、アクセストークンを無効化する
                                if (snsCode == 1)
                                {
                                    socialLogincs.InvalidationAccessToken(responsePair["access_token"]);
                                }

                                //会員IDが紐づいていればログイン（トップページへリダイレクト）
                                if (csfUserId > 0)
                                {
                                    Session["KaiinId"] = csfUserId;
                                    Response.Redirect("http://192.168.3.4:8080/LoginSuccessful");
                                }                                
                            }
                        }
                        catch
                        {
                            //SNSログインエラーの例外処理
                        }
                    }
                    //Session["sns"] = null;
                    Session["state"] = null;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SocialLogincs socialLogincs = new SocialLogincs();
            Dictionary<string, string> keyValues = socialLogincs.SocialLoginBtnPush(1);
            Session["sns"] = 1;
            Session["state"] = keyValues["state"];
            Response.Redirect(keyValues["authorizationUri"]);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SocialLogincs socialLogincs = new SocialLogincs();
            Dictionary<string, string> keyValues = socialLogincs.SocialLoginBtnPush(2);
            Session["sns"] = 2;
            Session["state"] = keyValues["state"];
            Response.Redirect(keyValues["authorizationUri"]);
        }
    }
}