using Microsoft.Xna.Framework;
using System;

namespace LD45.Screens {
    public interface IScreen {
        event ScreenEventHandler PushedScreen;
        event ScreenEventHandler ReplacedSelf;
        event EventHandler PoppedSelf;

        void Initialize(IServiceProvider services);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
