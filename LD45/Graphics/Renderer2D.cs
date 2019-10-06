using LD45.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Graphics {
    public sealed class Renderer2D {
        private readonly RendererSettings _defaultSettings = new RendererSettings();

        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;

        private readonly GameWindow _window;
        private RenderTarget2D _renderTarget1, _renderTarget2;
        private bool _boundsUpdatePending = false;

        private RendererSettings _settings;
        private RenderTargetBinding[] _defaultTargets;

        public Renderer2D(GraphicsDevice graphicsDevice, GameWindow window, int scale) {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);

            _window = window;
            Scale = scale;

            Bounds = ScaleBounds(window.ClientBounds);
            _renderTarget1 = new RenderTarget2D(graphicsDevice, Bounds.Width, Bounds.Height);
            _renderTarget2 = new RenderTarget2D(graphicsDevice, Bounds.Width, Bounds.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public int Scale { get; }
        public Rectangle Bounds { get; private set; }

        public void Refresh() {
            _defaultTargets = _graphicsDevice.GetRenderTargets();

            _graphicsDevice.SetRenderTarget(_renderTarget2);
            _graphicsDevice.Clear(Color.TransparentBlack);
        }

        public void Begin(RendererSettings settings = null) {
            _settings = settings ?? _defaultSettings;

            _graphicsDevice.SetRenderTarget(_renderTarget1);
            _graphicsDevice.Clear(Color.TransparentBlack);

            _spriteBatch.Begin(_settings.SortMode, _settings.BlendState, _settings.SamplerState,
                _settings.DepthStencilState, _settings.RasterizerState, _settings.SpriteEffect, 
                _settings.TransformMatrix);
        }

        public void Draw(Texture2D texture, Vector2 position,
            Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0f, 
            Vector2? scale = null, Color? color = null) {

            _spriteBatch.Draw(texture, position.Floor(), sourceRectangle, color ?? Color.White, rotation, (origin ?? Vector2.Zero).Floor(), scale ?? Vector2.One, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteFont font, string text, Vector2 position,
            Color? color = null, float scale = 1f, Vector2? origin = null) {

            origin = origin ?? Vector2.Zero;

            Vector2 size = font.MeasureString(text);

            _spriteBatch.DrawString(font, text, position, color ?? Color.White, 0f, size * origin.Value, scale, SpriteEffects.None, 0f);
        }

        public void End() {
            _spriteBatch.End();

            _graphicsDevice.SetRenderTarget(_renderTarget2);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _settings.LayerEffect);
            _spriteBatch.Draw(_renderTarget1, Vector2.Zero, color: Color.White);
            _spriteBatch.End();
        }

        public void Output() {
            _graphicsDevice.SetRenderTargets(_defaultTargets);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget2, Vector2.Zero, color: Color.White, scale: new Vector2(Scale));
            _spriteBatch.End();

            if (_boundsUpdatePending) {
                Bounds = ScaleBounds(_window.ClientBounds);

                _renderTarget1.Dispose();
                _renderTarget1 = new RenderTarget2D(_graphicsDevice, Bounds.Width, Bounds.Height);

                _renderTarget2.Dispose();
                _renderTarget2 = new RenderTarget2D(_graphicsDevice, Bounds.Width, Bounds.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

                _boundsUpdatePending = false;
            }
        }

        private Rectangle ScaleBounds(Rectangle bounds) {
            bounds.Width /= Scale;
            bounds.Height /= Scale;
            return bounds;
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e) {
            _boundsUpdatePending = true;
        }
    }
}
