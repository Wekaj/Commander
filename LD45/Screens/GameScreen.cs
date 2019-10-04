using Artemis;
using Artemis.Manager;
using LD45.Systems;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        private readonly EntityWorld _entityWorld = new EntityWorld();

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
        }

        public void Update(GameTime gameTime) {
            _entityWorld.Update(gameTime.ElapsedGameTime.Ticks);
        }

        public void Draw(GameTime gameTime) {
            _entityWorld.Draw();
        }
    }
}
