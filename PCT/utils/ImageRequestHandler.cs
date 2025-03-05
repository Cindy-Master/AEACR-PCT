using AEAssist.Helper;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Cindy_Master.Util
{
    public class ImageRequestHandler
    {
        private readonly string gateway;
        private readonly int seTuNum;
        private readonly AuthenticationManager authManager;
        private static readonly HttpClient httpClient = new();
        private bool isAuthenticated = false;
        private ConcurrentQueue<string> imageUrlsQueue = new ConcurrentQueue<string>();
        private string selectedPrefix = "imageflowpublic";
        private int MaxWidth;
        private int MaxHeight;
        public ImageRequestHandler(AuthenticationManager authManager, string gateway, int MaxWidth, int MaxHeight)
        {
            this.gateway = gateway;
            this.seTuNum = 5;
            this.authManager = authManager;
            this.MaxWidth = MaxWidth;
            this.MaxHeight = MaxHeight;

        }

        public async Task<bool> FetchImageUrlsAsync(string selectedPrefix)
        {
            if (!isAuthenticated)
            {
                isAuthenticated = await authManager.EnsureAuthenticatedAsync();
            }

            if (!isAuthenticated)
            {
                return false;
            }

            if (MaxWidth > 1000)
                MaxWidth = 1000;
            if (MaxHeight > 1000)
                MaxHeight = 1000;

            try
            {
                string responseBody = await SendImageRequestAsync(selectedPrefix); // 使用选定的前缀
                if (string.IsNullOrEmpty(responseBody))
                {
                    LogHelper.PrintError("获取图像列表为空");
                    return false;
                }

                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("success", out JsonElement successElement) && successElement.GetBoolean())
                {
                    if (root.TryGetProperty("code", out JsonElement codeElement) && codeElement.TryGetInt32(out int code))
                    {
                        if (code == 0)
                        {
                            if (root.TryGetProperty("data", out JsonElement data) && data.TryGetProperty("files", out JsonElement files)
                                && data.TryGetProperty("prefix", out JsonElement prefix))
                            {
                                foreach (var file in files.EnumerateArray())
                                {
                                    string? fileName = file.GetString();
                                    if (!string.IsNullOrEmpty(fileName))
                                    {
                                        string imageUrl = $"{gateway}/{prefix}/{fileName}?quality=80&maxwidth={MaxWidth}&maxheight={MaxHeight}&mode=carve";
                                        imageUrlsQueue.Enqueue(imageUrl);
                                    }
                                }
                            }
                        }
                        else if (code == 201)
                        {
                            if (root.TryGetProperty("message", out JsonElement messageElement))
                            {
                                string message = messageElement.GetString();
                                LogHelper.PrintError($"{message}");
                            }
                        }
                    }
                }
                else
                {
                    LogHelper.PrintError("请求失败");
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return !imageUrlsQueue.IsEmpty;
        }


        public string? LoadNextImage(string selectedPrefix)
        {
            if (imageUrlsQueue.Count < 2)
            {
                _ = FetchImageUrlsAsync(selectedPrefix); // 根据选定的前缀加载图像 URL
            }
            if (imageUrlsQueue.TryDequeue(out var imageUrl))
            {
                return imageUrl;
            }
            return null;
        }


        private async Task<string> SendImageRequestAsync(string prefix)
        {
            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{gateway}/api/v1/metadata/list?prefix={prefix}&random={seTuNum}");
                requestMessage.Headers.Add("Authorization", $"Bearer {authManager.GetAccessToken()}");

                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<List<string>> FetchPrefixesAsync()
        {
            if (!isAuthenticated)
            {
                isAuthenticated = await authManager.EnsureAuthenticatedAsync();
            }

            List<string> prefixes = new List<string>();

            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{gateway}/api/v1/metadata/prefix");
                requestMessage.Headers.Add("Authorization", $"Bearer {authManager.GetAccessToken()}");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseBody);
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty("success", out JsonElement successElement) && successElement.GetBoolean())
                {
                    if (root.TryGetProperty("data", out JsonElement data) && data.TryGetProperty("prefixes", out JsonElement prefixArray))
                    {
                        foreach (var prefix in prefixArray.EnumerateArray())
                        {
                            string prefixString = prefix.GetString();
                            if (!string.IsNullOrEmpty(prefixString))
                            {
                                prefixes.Add(prefixString);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return prefixes;
        }

        public void SetSelectedPrefix(string prefix)
        {
            selectedPrefix = prefix;
        }

        public string GetSelectedPrefix() => selectedPrefix;
    }
}
