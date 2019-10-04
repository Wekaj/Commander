using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Graphics {
    public sealed class Renderer2D {
        private readonly RendererSettings _defaultSettings = new RendererSettings();

        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;

        private readonly GameWindow _window;
        private readonly int _scale;
        private RenderTarget2D _renderTarget;
        private bool _boundsUpdatePending = false;

        private RendererSettings _settings;
        private RenderTargetBinding[] _defaultTargets;

        public Renderer2D(GraphicsDevice graphicsDevice, GameWindow window, int scale) {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);

            _window = window;
            _scale = scale;

            Bounds = Scale(window.ClientBounds);
            _renderTarget = new RenderTarget2D(graphicsDevice, Bounds.Width, Bounds.Height);

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public Rectangle Bounds { get; private set; }

        public void Begin(RendererSettings settings = null) {
            _settings = settings ?? _defaultSettings;

            _defaultTargets = _graphicsDevice.GetRenderTargets();
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.TransparentBlack);

            _spriteBatch.Begin(_settings.SortMode, _settings.BlendState, _settings.SamplerState,
                _settings.DepthStencilState, _settings.RasterizerState, _settings.SpriteEffect, 
                _settings.TransformMatrix);
        }

        public void Draw(Texture2D texture, Vector2 position,
            Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0f) {

            _spriteBatch.Draw(texture, position, sourceRectangle, Color.White, rotation, origin ?? Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteFont font, string text, Vector2 position,
            Color? color = null) {

            _spriteBatch.DrawString(font, text, position, color ?? Color.White);
        }

        public void End() {
            _spriteBatch.End();

            _graphicsDevice.SetRenderTargets(_defaultTargets);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _settings.LayerEffect);
            _spriteBatch.Draw(_renderTarget, Vector2.Zero, color: Color.White, scale: new Vector2(_scale));
            _spriteBatch.End();

            if (_boundsUpdatePending) {
                Bounds = Scale(_window.ClientBounds);

                _renderTarget.Dispose();
                _renderTarget = new RenderTarget2D(_graphicsDevice, Bounds.Width, Bounds.Height);

                _boundsUpdatePending = false;
            }
        }

        private Rectangle Scale(Rectangle bounds) {
            bounds.Width /= _scale;
            bounds.Height /= _scale;
            return bounds;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e) {
            _boundsUpdatePending = true;
        }
    }
}
