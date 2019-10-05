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
using LD45.Weapons;
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

        private Texture2D _personTexture, _spiderTexture, _swordIconTexture,
            _flagPoleTexture, _flagTexture;

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

            CreateCommander(new Vector2(32f, 32f), new Weapon { Action = new HitAction(), Icon = _swordIconTexture }, Color.Green);
            CreateCommander(new Vector2(64f, 32f), new Weapon { Action = new ShootAction(), Icon = _swordIconTexture }, Color.Red);

            for (int i = 0; i < 10; i++) {
                CreateRecruit(new Vector2(32f + random.NextSingle(128f), 32f + random.NextSingle(128f)));
            }

            for (int i = 0; i < 10; i++) {
                CreateSpider(new Vector2(256f) + new Vector2(random.NextSingle(64f), random.NextSingle(64f)));
            }

            CreateWeapon(new Vector2(128f, 128f), new Weapon { Action = new HitAction(), Icon = _swordIconTexture });
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
            _swordIconTexture = content.Load<Texture2D>("Textures/SwordIcon");
            _flagPoleTexture = content.Load<Texture2D>("Textures/FlagPole");
            _flagTexture = content.Load<Texture2D>("Textures/Flag");
        }

        private void InitializeSystems(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new UnitInteractionsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitStrategySystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new TileCollisionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyTransformSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new RecruitingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new WeaponPickupSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitActionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitCooldownSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new PacketSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new ParticleAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new IndicatorAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderWeaponSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new LinkSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new AnimationSystem(), GameLoopType.Update);

            _entityWorld.SystemManager.SetSystem(new ShadowDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new PathDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new SpriteDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new ParticleDrawingSystem(services), GameLoopType.Draw);
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

        private Entity CreateUnit(Vector2 position, int team, IUnitStrategy strategy, IUnitAction action) {
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
                Action = action,
                Tendency = _random.NextUnitVector() * _random.NextSingle(8f)
            });
            unit.AddComponent(new ShadowComponent {
                Type = ShadowType.Small
            });

            return unit;
        }

        private Entity CreateSpider(Vector2 position) {
            Entity spider = CreateUnit(position, 1, new StandardUnitStrategy(), new HitAction());

            spider.AddComponent(new SpriteComponent {
                Texture = _spiderTexture,
                Origin = new Vector2(6f, 8f)
            });

            return spider;
        }

        private Entity CreatePerson(Vector2 position, IUnitStrategy strategy) {
            Entity person = CreateUnit(position, 0, strategy, null);

            person.AddComponent(new SpriteComponent {
                Texture = _personTexture,
                Origin = new Vector2(4.5f, 11f)
            });

            return person;
        }

        private Entity CreateCommander(Vector2 position, IWeapon weapon, Color flagColor) {
            Entity commander = CreatePerson(position, new CommanderStrategy());

            commander.AddComponent(new CommanderComponent {
                Weapon = weapon,
                FlagColor = flagColor,
            });

            Entity flagPole = _entityWorld.CreateEntity();
            flagPole.AddComponent(new TransformComponent());
            flagPole.AddComponent(new SpriteComponent {
                Texture = _flagPoleTexture,
                Origin = new Vector2(15.5f, 26f),
            });
            flagPole.AddComponent(new LinkComponent {
                Parent = commander,
                Offset = new Vector2(0f, -0.001f),
            });

            Entity flag = _entityWorld.CreateEntity();
            flag.AddComponent(new TransformComponent());
            flag.AddComponent(new SpriteComponent {
                Texture = _flagTexture,
                Origin = new Vector2(15.5f, 26f),
                Color = flagColor,
            });
            flag.AddComponent(new LinkComponent {
                Parent = commander,
                Offset = new Vector2(0f, -0.002f),
            });
            flag.AddComponent(new AnimationComponent {
                Animation = new Animation(_flagTexture, 4, 0.1f)
            });

            return commander;
        }

        private Entity CreateRecruit(Vector2 position) {
            Entity recruit = CreatePerson(position, new StandardUnitStrategy());

            recruit.AddComponent(new RecruitableComponent());

            return recruit;
        }

        private Entity CreateWeapon(Vector2 position, IWeapon weapon) {
            Entity weaponEntity = _entityWorld.CreateEntity();

            weaponEntity.AddComponent(new BodyComponent {
                Position = position
            });
            weaponEntity.AddComponent(new TransformComponent());
            weaponEntity.AddComponent(new WeaponComponent {
                Weapon = weapon
            });
            weaponEntity.AddComponent(new SpriteComponent {
                Texture = weapon.Icon,
                Origin = new Vector2(weapon.Icon.Width / 2f, weapon.Icon.Height)
            });
            weaponEntity.AddComponent(new ShadowComponent {
                Type = ShadowType.Small
            });

            return weaponEntity;
        }
    }
}
