using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace mj.gist {
    public class RTUtil {
        public static RenderTexture Create(
            int width,
            int height,
            int depth,
            RenderTextureFormat format,
            bool enableRandomWrite = true,
            bool useMipMap = false,
            bool autoGenerateMips = false,
            TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            FilterMode filterMode = FilterMode.Bilinear
            ) {

            var rt = new RenderTexture(width, height, depth, format);
            rt.useMipMap = useMipMap;
            rt.autoGenerateMips = autoGenerateMips;
            rt.enableRandomWrite = enableRandomWrite;
            rt.filterMode = filterMode;
            rt.wrapMode = wrapMode;
            rt.Create();
            return rt;
        }

        public static Texture2D RenderTextureToTexture2D(RenderTexture rt) {
            TextureFormat format;

            switch (rt.format) {
                case RenderTextureFormat.ARGBFloat:
                    format = TextureFormat.RGBAFloat;
                    break;
                case RenderTextureFormat.ARGBHalf:
                    format = TextureFormat.RGBAHalf;
                    break;
                case RenderTextureFormat.ARGBInt:
                    format = TextureFormat.RGBA32;
                    break;
                case RenderTextureFormat.ARGB32:
                    format = TextureFormat.ARGB32;
                    break;
                default:
                    format = TextureFormat.ARGB32;
                    Debug.LogWarning("Unsuported RenderTextureFormat.");
                    break;
            }

            var tex2D = new Texture2D(rt.width, rt.height, format, false);
            var rect = Rect.MinMaxRect(0f, 0f, tex2D.width, tex2D.height);
            RenderTexture.active = rt;
            tex2D.ReadPixels(rect, 0, 0);
            RenderTexture.active = null;
            return tex2D;
        }

        public static bool IsResolutionChanged(RenderTexture rt, int width, int height) {
            if (rt.width != width || rt.height != height)
                return true;
            else
                return false;
        }

        public static RenderTexture NewFloat(int w, int h)
          => new RenderTexture(w, h, 0, RenderTextureFormat.RFloat);

        public static RenderTexture NewFloat4(int w, int h)
          => new RenderTexture(w, h, 0, RenderTextureFormat.ARGBFloat);

        public static RenderTexture NewUAV(int w, int h, int d = 0, RenderTextureFormat format = RenderTextureFormat.ARGBFloat, GraphicsFormat graphicsFormat = GraphicsFormat.R32G32B32A32_SFloat) {
            var rt = new RenderTexture(w, h, d, format);
            rt.graphicsFormat = graphicsFormat;
            rt.enableRandomWrite = true;
            rt.Create();
            return rt;
        }

        public static RenderTexture NewArgbUav(int w, int h) {
            var rt = new RenderTexture
              (w, h, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            rt.enableRandomWrite = true;
            rt.Create();
            return rt;
        }

        public static RenderTextureFormat SingleChannelRTFormat => SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8)
                                                                   ? RenderTextureFormat.R8 : RenderTextureFormat.Default;
        public static RenderTextureFormat SingleChannelHalfRTFormat => SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf)
                                                                    ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGBHalf;

        public static RenderTexture NewSingleChannelRT(int width, int height)
          => new RenderTexture(width, height, 0, SingleChannelRTFormat);

        public static RenderTexture NewSingleChannelHalfRT(int width, int height)
          => new RenderTexture(width, height, 0, SingleChannelHalfRTFormat);

        public static void Clear(RenderTexture tgt) {
            var tmp = RenderTexture.active;
            RenderTexture.active = tgt;
            GL.Clear(false, true, Color.clear);
            RenderTexture.active = tmp;
        }

        public static void Destroy(Texture tgt) {
            if (tgt != null) {
                Object.Destroy(tgt);
                tgt = null;
            }
        }
    }
}