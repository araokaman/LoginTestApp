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
        int snsCode;
        string userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Session["sns"] != null && Session["KaiinId"] != null)
                {
                    snsCode = (int)Session["sns"];
                    userId = (string)Session["KaiinI"];

                    if (snsCode == 1)
                    {
                        Button1.Visible = true;
                    }
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SocialLogincs socialLogincs = new SocialLogincs();
            socialLogincs.SendLineMessage(userId);
        }
    }
}