using AEAssist.Helper;
using Dalamud.Interface.Textures.TextureWraps;

namespace Cindy_Master.Util
{
    public class ImageProvider
    {
        private readonly ImageRequestHandler imageRequestHandler;
        private readonly AuthenticationManager authManager;
        private bool isLoading = false;
        private string Gateway = "http://154.9.252.182:28390";
        public ImageProvider(int MaxWidth = 600, int MaxHeight = 600)
        {
            authManager = new AuthenticationManager(Gateway);
            imageRequestHandler = new ImageRequestHandler(authManager, Gateway, MaxWidth, MaxHeight);
        }

        public bool IsLoading => isLoading;

        public async Task<bool> AuthenticateAsync()
        {
            await authManager.EnsureAuthenticatedAsync();
            return !string.IsNullOrEmpty(authManager.GetAccessToken());
        }

        public async Task<bool> LoadImageQueueAsync()
        {
            if (isLoading)
                return false;

            isLoading = true;
            try
            {
                string selectedPrefix = imageRequestHandler.GetSelectedPrefix();

                return await imageRequestHandler.FetchImageUrlsAsync(selectedPrefix);
            }
            finally
            {
                isLoading = false;
            }
        }

        public async void LoadNextImageAsync(Action<IDalamudTextureWrap> onLoadComplete)
        {
            try
            {
                bool isAuthenticated = await authManager.EnsureAuthenticatedAsync();
                if (!isAuthenticated)
                {
                    return;
                }
                string selectedPrefix = imageRequestHandler.GetSelectedPrefix();
                string? imageUrl = imageRequestHandler.LoadNextImage(selectedPrefix);
                if (string.IsNullOrEmpty(imageUrl))
                {
                    LogHelper.Print("没有更多图像可以显示。");
                    return;
                }

                string token = authManager.GetAccessToken();

                ThreadLoadImageHandlerExtensions.TryGetOrLoadTextureWrap(
                    imageUrl, token, out var textureWrap, onLoadComplete
                );

                if (textureWrap == null)
                {
                    onLoadComplete(null);
                }
            }
            catch (Exception ex)
            {
                onLoadComplete(null);
            }
        }

        public async Task<List<string>> FetchPrefixesAsync()
        {
            return await imageRequestHandler.FetchPrefixesAsync();
        }

        public void SetSelectedPrefix(string prefix)
        {
            imageRequestHandler.SetSelectedPrefix(prefix);
        }

        public string GetSelectedPrefix()
        {
            return imageRequestHandler.GetSelectedPrefix();
        }
    }
}
