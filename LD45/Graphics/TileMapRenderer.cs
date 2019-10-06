using LD45.Tiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Graphics {
    public sealed class TileMapRenderer {
        private readonly Renderer2D _renderer;

        private Texture2D _tilesTexture;

        public TileMapRenderer(IServiceProvider services) {
            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _tilesTexture = content.Load<Texture2D>("Textures/Tiles");
        }

        public void Draw(TileMap tileMap) {
            for (int y = 0; y < tileMap.Height; y++) {
                for (int x = 0; x < tileMap.Width; x++) {
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
