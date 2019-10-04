using LD45.Graphics;
using LD45.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Screens {
    public sealed class TitleScreen : IScreen {
        private Renderer2D _renderer;
        private InputManager _input;

        private SpriteFont _basicFont;

        private int _clickCounter;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            _renderer = services.GetRequiredService<Renderer2D>();
            _input = services.GetRequiredService<InputManager>();

            LoadContent(services.GetRequiredService<ContentManager>());
        }

        private void LoadContent(ContentManager content) {
            _basicFont = content.Load<SpriteFont>("Fonts/Basic");
        }

        public void Update(GameTime gameTime) {
            if (_input.Bindings.JustReleased(BindingId.LeftClick)) {
                _clickCounter++;
            }
        }

        public void Draw(GameTime gameTime) {
            _renderer.Begin();

            _renderer.Draw(_basicFont, "Welcome to the title screen! Clicks: " + _clickCounter, new Vector2(8f));

            _renderer.End();
        }
    }
}
