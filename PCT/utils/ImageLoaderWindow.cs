using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;
using System.Numerics;

namespace Cindy_Master.Util
{
    public class ImageLoaderWindow
    {
        private ImageProvider imageProvider;
        private IDalamudTextureWrap textureWrap;
        private bool isImageReady = false;
        private bool isWindowOpen = false;
        private bool isTransitioning = false;
        private DateTime transitionStartTime;
        private readonly TimeSpan transitionDuration = TimeSpan.FromSeconds(1);
        private List<string> availablePrefixes = new List<string>();
        private int selectedPrefixIndex = 0;

        public bool IsWindowOpen
        {
            get => isWindowOpen;
            set => isWindowOpen = value;
        }

        private async Task LoadAvailablePrefixes()
        {
            availablePrefixes = await imageProvider.FetchPrefixesAsync();
            if (availablePrefixes.Count == 0)
            {
                availablePrefixes.Add("imageflowpublic");
            }
        }

        public async void OnButtonClick()
        {
            if (!isWindowOpen)
            {
                isWindowOpen = true;
            }

            imageProvider = new ImageProvider();

            await LoadAvailablePrefixes();
            imageProvider.SetSelectedPrefix(availablePrefixes[selectedPrefixIndex]);

            if (await imageProvider.LoadImageQueueAsync())
            {
                LoadNextImage();
            }
        }

        private void LoadNextImage()
        {
            isImageReady = false;
            imageProvider.LoadNextImageAsync(texture =>
            {
                if (texture != null)
                {
                    textureWrap = texture;
                    isImageReady = true;
                    StartTransition();
                }
            });
        }

        private void StartTransition()
        {
            isTransitioning = true;
            transitionStartTime = DateTime.Now;
        }

        private float GetTransitionAlpha()
        {
            if (!isTransitioning)
                return 1.0f;

            var elapsed = (DateTime.Now - transitionStartTime).TotalSeconds;
            if (elapsed >= transitionDuration.TotalSeconds)
            {
                isTransitioning = false;
                return 1.0f;
            }

            return (float)(elapsed / transitionDuration.TotalSeconds);
        }

        private Vector2 previousImageSize = new Vector2(400, 300);

        public void RenderWindow()
        {
            if (!isWindowOpen)
            {
                return;
            }

            float screenHeight = ImGui.GetIO().DisplaySize.Y;

            Vector2 imageSize = previousImageSize;
            if (isImageReady && textureWrap != null && !isTransitioning)
            {
                imageSize = new Vector2(textureWrap.Width, textureWrap.Height);
                previousImageSize = imageSize;
            }

            ImGui.SetNextWindowSize(imageSize + new Vector2(20, 80));
            Vector2 windowPos = new Vector2(20, (screenHeight - (imageSize.Y + 80)) / 2);
            ImGui.SetNextWindowPos(windowPos, ImGuiCond.FirstUseEver);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));

            try
            {
                if (ImGui.Begin("色图", ref isWindowOpen, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoBackground))

                {
                    if (ImGui.IsWindowCollapsed())
                    {
                        ImGui.End();
                        ImGui.PopStyleVar();
                        return;
                    }

                    float alpha = GetTransitionAlpha();
                    ImGui.PushStyleVar(ImGuiStyleVar.Alpha, alpha);

                    Vector2 windowSize = ImGui.GetWindowSize();
                    Vector2 centeredPos = new Vector2(
                        (windowSize.X - imageSize.X) / 2
                    );
                    ImGui.SetCursorPos(centeredPos);

                    if (textureWrap != null && textureWrap.ImGuiHandle != IntPtr.Zero)
                    {
                        ImGui.Image(textureWrap.ImGuiHandle, imageSize);

                        if (isImageReady && !isTransitioning && ImGui.IsItemClicked())
                        {
                            LoadNextImage();
                        }
                    }
                    else
                    {
                        ImGui.Text("加载中...");
                    }

                    ImGui.PopStyleVar();

                    ImGui.SetCursorPosY(ImGui.GetCursorPosY() + 10);
                    if (availablePrefixes.Count > 1)
                    {
                        if (ImGui.Combo("选择图库", ref selectedPrefixIndex, availablePrefixes.ToArray(), availablePrefixes.Count))
                            imageProvider.SetSelectedPrefix(availablePrefixes[selectedPrefixIndex]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                ImGui.PopStyleVar();
                ImGui.End();
            }
        }
    }
}
