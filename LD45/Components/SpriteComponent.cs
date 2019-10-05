using Artemis.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Components {
    public sealed class SpriteComponent : IComponent {
        public Texture2D Texture { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; } = Color.White;
        public Rectangle? SourceRectangle { get; set; } = null;
    }
}
