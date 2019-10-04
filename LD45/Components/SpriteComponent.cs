using Artemis.Interface;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Components {
    public sealed class SpriteComponent : IComponent {
        public Texture2D Texture { get; set; }
    }
}
