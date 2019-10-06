using Artemis;
using Artemis.Manager;
using LD45.Actions;
using LD45.Controllers;
using LD45.Entities;
using LD45.Extensions;
using LD45.Graphics;
using LD45.Systems;
using LD45.Tiles;
using LD45.Utilities;
using LD45.Weapons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        private readonly EntityWorld _entityWorld = new EntityWorld();
        private readonly Camera2D _camera = new Camera2D();
        private readonly Random _random = new Random();
        private TileMap _tileMap;

        private ServiceContainer _screenServices;

        private Renderer2D _renderer;
        private RendererSettings _rendererSettings;
        private TileMapRenderer _tileMapRenderer;
        private SquadController _squadController;
        private EntityBuilder _entityBuilder;

        private Texture2D _swordIconTexture;

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            CreateServiceContainer(services);

            _entityBuilder = new EntityBuilder(_screenServices);
            _screenServices.SetService(_entityBuilder);

            _renderer = _screenServices.GetRequiredService<Renderer2D>();
            _rendererSettings = new RendererSettings {
                SamplerState = SamplerState.PointClamp
            };
            _tileMapRenderer = new TileMapRenderer(_screenServices);
            _squadController = new SquadController(_screenServices);

            LoadContent(_screenServices);

            var map = new TmxMap("Content/Maps/Map1.tmx");

            _tileMap = new TileMap(map.Width, map.Height);
            _screenServices.SetService(_tileMap);

            var random = new Random();
            for (int y = 0; y < map.Height; y++) {
                for (int x = 0; x < map.Width; x++) {
                    _tileMap[x, y] = new Tile {
                        Texture = map.Layers[0].Tiles[x + y * map.Width].Gid - 1,
                        Type = (TileType)map.Layers[1].Tiles[x + y * map.Width].Gid
                    };
                }
            }

            InitializeSystems(_screenServices);

            _entityBuilder.CreateCommander(new Vector2(32f, 32f), new Weapon { Action = new HitAction(), Icon = _swordIconTexture }, Color.SeaGreen);
            _entityBuilder.CreateCommander(new Vector2(64f, 32f), new Weapon { Action = new ShootAction(), Icon = _swordIconTexture }, Color.PaleVioletRed);

            for (int i = 0; i < 10; i++) {
                _entityBuilder.CreateRecruit(new Vector2(32f + random.NextSingle(128f), 32f + random.NextSingle(128f)));
            }

            for (int i = 0; i < 10; i++) {
                _entityBuilder.CreateSpider(new Vector2(256f) + new Vector2(random.NextSingle(64f), random.NextSingle(64f)));
            }

            _entityBuilder.CreateWeapon(new Vector2(128f, 128f), new Weapon { Action = new HitAction(), Icon = _swordIconTexture });

            _entityBuilder.CreateStatDrop(new Vector2(160f, 128f));
            _entityBuilder.CreateStatDrop(new Vector2(160f, 160f));
        }

        private void CreateServiceContainer(IServiceProvider services) {
            _screenServices = new ServiceContainer(services);

            _screenServices.SetService(_entityWorld);
            _screenServices.SetService(_camera);
            _screenServices.SetService(_random);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _swordIconTexture = content.Load<Texture2D>("Textures/SwordIcon");
        }

        private void InitializeSystems(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new UnitInteractionsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitStrategySystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new TileCollisionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyTransformSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new RecruitingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new WeaponPickupSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new StatPickupSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitActionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitCooldownSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new PacketSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new ParticleAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new IndicatorAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new StatAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderWeaponSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new LinkSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new AnimationSystem(), GameLoopType.Update);

            _entityWorld.SystemManager.SetSystem(new ShadowDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new PathDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new SpriteDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new StatDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new ParticleDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new IndicatorDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new HudDrawingSystem(services), GameLoopType.Draw);
        }

        public void Update(GameTime gameTime) {
            _squadController.Update();

            _entityWorld.Update(gameTime.ElapsedGameTime.Ticks);
        }

        public void Draw(GameTime gameTime) {
            _renderer.Refresh();

            _rendererSettings.TransformMatrix = _camera.GetTransformMatrix();
            _renderer.Begin(_rendererSettings);

            _tileMapRenderer.Draw(_tileMap);
            _entityWorld.Draw();

            _renderer.End();
            _renderer.Output();
        }
    }
}
