using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD45.Graphics {
    public sealed class Animation {
        private readonly int _frameWidth;
        private readonly float _totalTime;

        public Animation(Texture2D texture, int frames, float frameDuration) {
            Texture = texture;
            Frames = frames;
            FrameDuration = frameDuration;

            _frameWidth = Texture.Width / frames;
            _totalTime = frames * frameDuration;
        }

        public Texture2D Texture { get; }
        public int Frames { get; }
        public float FrameDuration { get; }

        public float Timer { get; set; } = 0f;


        public Rectangle GetCurrentFrame() {
            int frame = (int)((Timer % _totalTime) / FrameDuration);

            return new Rectangle(frame * _frameWidth, 0, _frameWidth, Texture.Height);
        }
    }
}
