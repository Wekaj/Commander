using LD45.Graphics;
using LD45.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace LD45.Screens {
    public sealed class VictoryScreen : IScreen {
        private Renderer2D _renderer;
        private InputManager _input;
        private ContentManager _content;

        private SpriteFont _basicFont;
        private Texture2D _titleTexture;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            _input = services.GetRequiredService<InputManager>();
            _renderer = services.GetRequiredService<Renderer2D>();
            _content = services.GetRequiredService<ContentManager>();

            LoadContent(_content);
        }

        private void LoadContent(ContentManager content) {
            _basicFont = content.Load<SpriteFont>("Fonts/Small");
        }

        public void Update(GameTime gameTime) {
            if (_input.Bindings.IsPressed(BindingId.LeftClick) || _input.Bindings.IsPressed(BindingId.RightClick)) {
                ReplacedSelf?.Invoke(this, new ScreenEventArgs(new TitleScreen()));
            }
        }

        public void Draw(GameTime gameTime) {
            _renderer.Refresh();
            _renderer.Begin(color: Color.Lerp(Color.White, Color.Black, 0.95f));

            _renderer.Draw(_basicFont, "You killed the dragon! You win!", new Vector2(4f), color: Color.White);

            _renderer.End();
            _renderer.Output();
        }
    }
}
