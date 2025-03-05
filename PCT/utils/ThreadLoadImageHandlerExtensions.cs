using Cindy_Master.Util;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons;
using ECommons.DalamudServices;
using System.Collections.Concurrent;

public class ThreadLoadImageHandlerExtensions
{
    internal static ConcurrentDictionary<string, ImageLoadingResult> CachedTextures = new ConcurrentDictionary<string, ImageLoadingResult>();

    internal static ConcurrentDictionary<(uint ID, bool HQ), ImageLoadingResult> CachedIcons = new ConcurrentDictionary<(uint, bool), ImageLoadingResult>();

    private static readonly List<Func<byte[], byte[]>> _conversionsToBitmap = new List<Func<byte[], byte[]>>
    {
        (byte[] b) => b
    };

    private static volatile bool ThreadRunning = false;
    private static readonly HttpClient httpClient = new();
    public static void ClearAll()
    {
        foreach (KeyValuePair<string, ImageLoadingResult> x2 in CachedTextures)
        {
            GenericHelpers.Safe(delegate
            {
                x2.Value.TextureWrap?.Dispose();
            });
        }

        GenericHelpers.Safe(CachedTextures.Clear);
        foreach (KeyValuePair<(uint, bool), ImageLoadingResult> x in CachedIcons)
        {
            GenericHelpers.Safe(delegate
            {
                x.Value.TextureWrap?.Dispose();
            });
        }

        GenericHelpers.Safe(CachedIcons.Clear);
    }

    public static void TryGetOrLoadTextureWrap(string url, string token, out IDalamudTextureWrap textureWrap, Action<IDalamudTextureWrap> onLoadComplete)
    {
        textureWrap = null;
        BeginThreadIfNotRunning(url, token, onLoadComplete);

    }
    private static void BeginThreadIfNotRunning(string url, string token, Action<IDalamudTextureWrap> onLoadComplete)
    {
        if (ThreadRunning)
        {
            return;
        }

        ThreadRunning = true;
        new Thread(() =>
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage result = httpClient.SendAsync(requestMessage).Result;
                result.EnsureSuccessStatusCode();

                byte[] imageBytes = result.Content.ReadAsByteArrayAsync().Result;

                using (var ms = new MemoryStream(imageBytes))
                {
                    var textureWrap = Svc.Texture.CreateFromImageAsync(ms).Result;

                    onLoadComplete?.Invoke(textureWrap);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ThreadRunning = false;
            }
        }).Start();
    }

    public static void AddConversionToBitmap(Func<byte[], byte[]> conversion)
    {
        _conversionsToBitmap.Add(conversion);
    }

    public static void RemoveConversionToBitmap(Func<byte[], byte[]> conversion)
    {
        _conversionsToBitmap.Remove(conversion);
    }
}