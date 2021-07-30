using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoginTestApp
{
    public class Ajax
    {
        public async Task<IActionResult> OnPostAsync(xxx.Models.testuser testuser)//** ユーザー認証のモデル **//
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            var client = ClientFactory.CreateClient();
            var reCAPTCHA = configuration.GetSection("reCAPTCHA").Get<xxx.Models.reCAPTCHA>();

            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"secret", reCAPTCHA.SecretKey},
                    {"response", recaptchaResponse},
                    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
                };
                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    this.ModelState.AddModelError(string.Empty, "ログインエラーが発生しました ");
                }

                if (ModelState.IsValid)
                //**　ユーザー認証を行います　**//
                {
                    if (testuser.IsValid(testuser.UserName, testuser.Password, ((int)testuser.ID).ToString()))
                    {
                        Response.Redirect("/xxx/zzzz/testIndex?id=" + HttpUtility.UrlEncode(((int)testuser.ID).ToString()));
                    }
                }
                Message = "ログインエラーが発生しました";
            }
            catch (HttpRequestException ex)
            {
                this.Logger.LogError(ex, "Unexpected error calling reCAPTCHA api.");
            }
            return Page();
        }
    }
}