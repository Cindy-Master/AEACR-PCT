using AEAssist.Helper;
using System.Text.Json;

namespace Cindy_Master.Util
{
    public class AuthenticationManager
    {
        private readonly string biosId;
        private readonly string gateway;
        private string accessToken;
        private string refreshToken;
        private DateTime accessTokenExpiry;
        private static readonly HttpClient httpClient = new();
        private DateTime refreshTokenExpiry;

        public AuthenticationManager(string gateway)
        {
            this.biosId = biosId ?? MacIdHelper.BiosId();
            this.gateway = gateway;
        }

        public async Task<bool> EnsureAuthenticatedAsync()
        {
            if (string.IsNullOrEmpty(accessToken) || DateTime.Now >= accessTokenExpiry)
            {
                if (string.IsNullOrEmpty(refreshToken) || DateTime.Now >= refreshTokenExpiry)
                {
                    await AuthenticateAsync();
                }
                else
                {
                    await RefreshAccessTokenAsync();
                }

                if (string.IsNullOrEmpty(accessToken) || DateTime.Now >= accessTokenExpiry)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task AuthenticateAsync()
        {
            if (string.IsNullOrEmpty(biosId))
            {
                LogHelper.PrintError("机器码不能为空");
                return;
            }

            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, gateway + "/api/v1/auth");
                requestMessage.Content = new StringContent($"{{ \"clientKey\": \"{biosId}\" }}", System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("success", out JsonElement successElement) && successElement.GetBoolean())
                {
                    if (root.TryGetProperty("data", out JsonElement data))
                    {
                        accessToken = data.GetProperty("accessToken").GetString();
                        refreshToken = data.GetProperty("refreshToken").GetString();
                        accessTokenExpiry = DateTime.Now.AddMinutes(data.GetProperty("accessExpireMinutes").GetInt32());
                        refreshTokenExpiry = DateTime.Now.AddDays(data.GetProperty("refreshExpireDays").GetInt32());

                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    }
                }
                else
                {
                    LogHelper.PrintError("吃点素的");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task RefreshAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, gateway + "/api/v1/auth/refresh");
                requestMessage.Content = new StringContent($"{{ \"refreshToken\": \"{refreshToken}\" }}", System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("success", out JsonElement successElement) && successElement.GetBoolean())
                {
                    if (root.TryGetProperty("data", out JsonElement data))
                    {
                        accessToken = data.GetProperty("accessToken").GetString();
                        refreshToken = data.GetProperty("refreshToken").GetString();
                        accessTokenExpiry = DateTime.Now.AddMinutes(data.GetProperty("accessExpireMinutes").GetInt32());
                        refreshTokenExpiry = DateTime.Now.AddDays(data.GetProperty("refreshExpireDays").GetInt32());

                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    }
                }
                else
                {
                    LogHelper.PrintError("刷新失败");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public string GetAccessToken() => accessToken;
    }
}
