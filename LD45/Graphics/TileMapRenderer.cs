using LD45.Tiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Graphics {
    public sealed class TileMapRenderer {
        private readonly Renderer2D _renderer;
        private readonly Camera2D _camera;

        private Texture2D _tilesTexture;

        public TileMapRenderer(IServiceProvider services) {
            _renderer = services.GetRequiredService<Renderer2D>();
            _camera = services.GetRequiredService<Camera2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _tilesTexture = content.Load<Texture2D>("Textures/Tiles");
        }

        public void Draw(TileMap tileMap) {
            Rectangle bounds = _renderer.Bounds;
            bounds.X = 0;
            bounds.Y = 0;
            bounds.Offset(_camera.Position);

            int startX = Math.Max(bounds.Left / Constants.TileSize - 1, 0);
            int startY = Math.Max(bounds.Top / Constants.TileSize - 1, 0);
            int endX = Math.Min(bounds.Right / Constants.TileSize + 1, tileMap.Width - 1);
            int endY = Math.Min(bounds.Bottom / Constants.TileSize + 1, tileMap.Height - 1);

            for (int y = startY; y <= endY; y++) {
                for (int x = startX; x <= endX; x++) {
                    int texture = tileMap[x, y].Texture;

                    int textureX = texture % 8;
                    int textureY = texture / 8;

                    _renderer.Draw(_tilesTexture, new Vector2(x * Constants.TileSize, y * Constants.TileSize), 
                        sourceRectangle: new Rectangle(textureX * Constants.TileSize, textureY * Constants.TileSize, Constants.TileSize, Constants.TileSize));
                }
            }
        }
    }
}
