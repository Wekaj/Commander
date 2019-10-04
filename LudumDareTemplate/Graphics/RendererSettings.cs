using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LudumDareTemplate.Graphics {
    public sealed class RendererSettings {
        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;
        public BlendState BlendState { get; set; } = null;
        public SamplerState SamplerState { get; set; } = null;
        public DepthStencilState DepthStencilState { get; set; } = null;
        public RasterizerState RasterizerState { get; set; } = null;
        public Effect SpriteEffect { get; set; } = null;
        public Effect LayerEffect { get; set; } = null;
        public Matrix TransformMatrix { get; set; } = Matrix.Identity;
    }
}
