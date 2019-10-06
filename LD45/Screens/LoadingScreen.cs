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
    public sealed class LoadingScreen : IScreen {
        private Renderer2D _renderer;
        private InputManager _input;
        private ContentManager _content;

        private SpriteFont _basicFont;
        private Texture2D _titleTexture;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        private bool _rendered = false;

        public void Initialize(IServiceProvider services) {
            _renderer = services.GetRequiredService<Renderer2D>();
            _content = services.GetRequiredService<ContentManager>();

            LoadContent(_content);
        }

        private void LoadContent(ContentManager content) {
            _basicFont = content.Load<SpriteFont>("Fonts/Small");
        }

        public void Update(GameTime gameTime) {
            if (!_rendered) {
                return;
            }

            string Cut(string s) {
                return s.Substring(s.LastIndexOf('\\') + 1);
            }
            string RemoveExt(string s) {
                return s.Remove(s.LastIndexOf('.'));
            }

            foreach (string filename in Directory.GetFiles("Content/Textures")) {
                string contentName = "Textures/" + RemoveExt(Cut(filename));

                _content.Load<Texture2D>(contentName);
            }

            foreach (string filename in Directory.GetFiles("Content/Fonts")) {
                string contentName = "Fonts/" + RemoveExt(Cut(filename));

                _content.Load<SpriteFont>(contentName);
            }

            foreach (string filename in Directory.GetFiles("Content/Sounds")) {
                string contentName = "Sounds/" + RemoveExt(Cut(filename));

                _content.Load<SoundEffect>(contentName);
            }

            foreach (string filename in Directory.GetFiles("Content/Effects")) {
                string contentName = "Effects/" + RemoveExt(Cut(filename));

                _content.Load<Effect>(contentName);
            }

            ReplacedSelf?.Invoke(this, new ScreenEventArgs(new TitleScreen()));
        }

        public void Draw(GameTime gameTime) {
            _renderer.Refresh();
            _renderer.Begin(color: Color.Lerp(Color.White, Color.Black, 0.95f));

            _renderer.Draw(_basicFont, "Loading...", new Vector2(4f), color: Color.White);

            _renderer.End();
            _renderer.Output();

            _rendered = true;
        }
    }
}
