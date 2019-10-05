using Artemis;
using Artemis.Manager;
using LD45.Actions;
using LD45.AI;
using LD45.Components;
using LD45.Controllers;
using LD45.Extensions;
using LD45.Graphics;
using LD45.Systems;
using LD45.Tiles;
using LD45.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        private readonly EntityWorld _entityWorld = new EntityWorld();
        private readonly TileMap _tileMap = new TileMap(64, 64);
        private readonly Camera2D _camera = new Camera2D();
        private readonly Random _random = new Random();

        private ServiceContainer _screenServices;

        private Renderer2D _renderer;
        private RendererSettings _rendererSettings;
        private TileMapRenderer _tileMapRenderer;
        private SquadController _squadController;

        private Texture2D _personTexture, _spiderTexture;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            CreateServiceContainer(services);

            _renderer = _screenServices.GetRequiredService<Renderer2D>();
            _rendererSettings = new RendererSettings {
                SamplerState = SamplerState.PointClamp
            };
            _tileMapRenderer = new TileMapRenderer(_screenServices);
            _squadController = new SquadController(_screenServices);

            LoadContent(_screenServices);
            InitializeSystems(_screenServices);

            var random = new Random();
            for (int y = 0; y < _tileMap.Height; y++) {
                for (int x = 0; x < _tileMap.Width; x++) {
                    _tileMap[x, y] = new Tile {
                        Type = random.Next(6) == 0 ? TileType.Rocks : TileType.Plains
                    };
                }
            }

            CreateCommander(new Vector2(32f, 32f));
            CreateCommander(new Vector2(64f, 32f));

            for (int i = 0; i < 100; i++) {
                CreateRecruit(new Vector2(32f + random.NextSingle(256f), 32f + random.NextSingle(256f)));
            }

            for (int i = 0; i < 10; i++) {
                CreateSpider(new Vector2(128f) + new Vector2(random.NextSingle(64f), random.NextSingle(64f)));
            }
        }

        private void CreateServiceContainer(IServiceProvider services) {
            _screenServices = new ServiceContainer(services);

            _screenServices.SetService(_entityWorld);
            _screenServices.SetService(_tileMap);
            _screenServices.SetService(_camera);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _personTexture = content.Load<Texture2D>("Textures/Person");
            _spiderTexture = content.Load<Texture2D>("Textures/Spider");
        }

        private void InitializeSystems(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new UnitInteractionsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitStrategySystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new TileCollisionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyTransformSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new RecruitingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitCooldownSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new PacketSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new IndicatorAnimatingSystem(), GameLoopType.Update);

            _entityWorld.SystemManager.SetSystem(new PathDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new SpriteDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new IndicatorDrawingSystem(services), GameLoopType.Draw);
        }

        public void Update(GameTime gameTime) {
            _squadController.Update();

            _entityWorld.Update(gameTime.ElapsedGameTime.Ticks);
        }

        public void Draw(GameTime gameTime) {
            _rendererSettings.TransformMatrix = _camera.GetTransformMatrix();
            _renderer.Begin(_rendererSettings);

            _tileMapRenderer.Draw(_tileMap);
            _entityWorld.Draw();

            _renderer.End();
        }

        private Entity CreateUnit(Vector2 position, int team, IUnitStrategy strategy) {
            Entity unit = _entityWorld.CreateEntity();

            unit.AddComponent(new BodyComponent {
                Position = position
            });
            unit.AddComponent(new TransformComponent());
            unit.AddComponent(new UnitComponent {
                MaxHealth = 100,
                Health = 100,
                Team = team,
                Strategy = strategy,
                Action = new HitAction(),
                Tendency = _random.NextUnitVector() * _random.NextSingle(8f)
            });

            return unit;
        }

        private Entity CreateSpider(Vector2 position) {
            Entity spider = CreateUnit(position, 1, new StandardUnitStrategy());

            spider.AddComponent(new SpriteComponent {
                Texture = _spiderTexture,
                Origin = new Vector2(6f, 7f)
            });

            return spider;
        }

        private Entity CreatePerson(Vector2 position, IUnitStrategy strategy) {
            Entity person = CreateUnit(position, 0, strategy);

            person.AddComponent(new SpriteComponent {
                Texture = _personTexture,
                Origin = new Vector2(4.5f, 11f)
            });

            return person;
        }

        private Entity CreateCommander(Vector2 position) {
            Entity commander = CreatePerson(position, new CommanderStrategy());

            commander.AddComponent(new CommanderComponent());

            return commander;
        }

        private Entity CreateRecruit(Vector2 position) {
            Entity recruit = CreatePerson(position, new StandardUnitStrategy());

            recruit.AddComponent(new RecruitableComponent());

            return recruit;
        }
    }
}
