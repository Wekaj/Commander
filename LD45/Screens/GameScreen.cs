using Artemis;
using Artemis.Manager;
using LD45.Components;
using LD45.Graphics;
using LD45.Systems;
using LD45.Tiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        private readonly EntityWorld _entityWorld = new EntityWorld();
        private readonly TileMap _tileMap = new TileMap(64, 64);

        private Renderer2D _renderer;
        private TileMapRenderer _tileMapRenderer;

        private Texture2D _personTexture;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            _renderer = services.GetRequiredService<Renderer2D>();
            _tileMapRenderer = new TileMapRenderer(services);

            LoadContent(services);
            InitializeSystems(services);

            for (int y = 0; y < _tileMap.Height; y++) {
                for (int x = 0; x < _tileMap.Width; x++) {
                    _tileMap[x, y] = new Tile();
                }
            }

            Entity person = _entityWorld.CreateEntity();
            person.AddComponent(new BodyComponent());
            person.AddComponent(new TransformComponent());
            person.AddComponent(new SpriteComponent {
                Texture = _personTexture
            });
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _personTexture = content.Load<Texture2D>("Textures/Person");
        }

        private void InitializeSystems(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyTransformSystem(), GameLoopType.Update);

            _entityWorld.SystemManager.SetSystem(new SpriteDrawingSystem(services), GameLoopType.Draw);
        }

        public void Update(GameTime gameTime) {
            _entityWorld.Update(gameTime.ElapsedGameTime.Ticks);
        }

        public void Draw(GameTime gameTime) {
            _renderer.Begin();

            _tileMapRenderer.Draw(_tileMap);
            _entityWorld.Draw();

            _renderer.End();
        }
    }
}
