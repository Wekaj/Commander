using Microsoft.Xna.Framework;
using System;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
        }

        public void Update(GameTime gameTime) {
        }

        public void Draw(GameTime gameTime) {
        }
    }
}
