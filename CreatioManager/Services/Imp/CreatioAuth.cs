using MupiModel.Dtos.AppSettings;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CreatioManager.Services.Imp
{
    public class CreatioAuth
    {

        private readonly CreatioSetting _appSettings;


        public CookieContainer AuthCookie { get; set; }
        public string? CsrfToken { get; set; }


        public CreatioAuth(CreatioSetting settings)
        {
            _appSettings = settings;
            AuthCookie = new CookieContainer();
            LoginCreatio();
        }

        #region Manage Session at Creatio

        private void LoginCreatio()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var handler = new HttpClientHandler();
            handler.CookieContainer = AuthCookie;

            var client = new HttpClient(handler);

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_appSettings.UrlCreatio}/ServiceModel/AuthService.svc/Login");

            request.Content = new StringContent(
                JsonConvert.SerializeObject(new
                {
                    _appSettings.UserName,
                    UserPassword = _appSettings.Password
                }),
                Encoding.UTF8, "application/json");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            CookieCollection cookieCollection = AuthCookie.GetCookies(new Uri(request.RequestUri?.ToString() ?? ""));
            CsrfToken = cookieCollection["BPMCSRF"]?.Value ?? "";

        }

        #endregion

    }
}
