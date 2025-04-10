﻿using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.TextureWraps;

namespace Cindy_Master.Util
{
    internal class ImageLoadingResult
    {
        internal ISharedImmediateTexture? ImmediateTexture;

        internal IDalamudTextureWrap? TextureWrap;

        internal bool IsCompleted;

        internal IDalamudTextureWrap? Texture => ImmediateTexture?.GetWrapOrDefault() ?? TextureWrap;

        public ImageLoadingResult(ISharedImmediateTexture? immediateTexture)
        {
            ImmediateTexture = immediateTexture;
        }

        public ImageLoadingResult(IDalamudTextureWrap? textureWrap)
        {
            TextureWrap = textureWrap;
        }

        public ImageLoadingResult()
        {
        }
    }
}
