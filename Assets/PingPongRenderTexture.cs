using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace mj.gist {
    public class PingPongRenderTexture : IDisposable {
        protected RenderTexture rt0, rt1;
        public RenderTexture Read => rt0;
        public RenderTexture Write => rt1;

        public PingPongRenderTexture(int w, int h, int depth, GraphicsFormat format, bool enableRandomWrite = true, FilterMode filter = FilterMode.Point) {
            
            rt0 = new RenderTexture(w, h, depth, format) {
                filterMode = filter,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave,
                enableRandomWrite = enableRandomWrite
            };
            rt1 = new RenderTexture(w, h, depth, format) {
                filterMode = filter,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.HideAndDontSave,
                enableRandomWrite = enableRandomWrite
            };
            rt0.Create();
            rt1.Create();
        }

        public PingPongRenderTexture(RenderTexture rt) {
            rt0 = new RenderTexture(rt);
            rt1 = new RenderTexture(rt);
            rt0.Create();
            rt1.Create();
        }

        public void Copy() {
            Graphics.Blit(rt0, rt1);
        }

        public void Swap() {
            var tmp = rt0;
            rt0 = rt1;
            rt1 = tmp;
        }

        public void Dispose() {
            RTUtil.Destroy(rt0);
            RTUtil.Destroy(rt1);
        }

        public static implicit operator RenderTexture(PingPongRenderTexture prt) => prt.Read;
    }
}