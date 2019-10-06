using Artemis;
using Artemis.Manager;
using LD45.Actions;
using LD45.Audio;
using LD45.Components;
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
using System.Collections.Generic;
using TiledSharp;

namespace LD45.Screens {
    public sealed class GameScreen : IScreen {
        private readonly EntityWorld _entityWorld = new EntityWorld();
        private readonly Camera2D _camera = new Camera2D();
        private readonly Random _random = new Random();
        private readonly ComponentRemover _componentRemover = new ComponentRemover();
        private TileMap _tileMap;

        private ServiceContainer _screenServices;

        private Renderer2D _renderer;
        private RendererSettings _rendererSettings;
        private TileMapRenderer _tileMapRenderer;
        private SquadController _squadController;
        private EntitySpawner _spawner;
        private EntityBuilder _entityBuilder;
        private ActionList _actions;
        private SoundPlayer _soundPlayer;

        private Texture2D _swordIconTexture, _stickIconTexture, _bowIconTexture, _staffIconTexture;
        private Effect _fogEffect;

        private readonly List<Color> _flagColors = new List<Color> {
            Color.SeaGreen,
            Color.Lerp(Color.PaleVioletRed, Color.Red, 0.5f),
            Color.Lerp(Color.LightBlue, Color.Blue, 0.5f),
            Color.Lerp(Color.LightYellow, Color.OrangeRed, 0.75f),
            Color.Lerp(Color.RosyBrown, Color.Chocolate, 0.5f),
            Color.HotPink,
            Color.Purple,
            Color.Lerp(Color.White, Color.Black, 0.8f),
        };

        private Color PopColor() {
            int r = _random.Next(_flagColors.Count);

            Color c = _flagColors[r];

            _flagColors.RemoveAt(r);

            return c;
        }

        private readonly Aspect _commanderAspect = Aspect.All(typeof(CommanderComponent), typeof(TransformComponent));

        public event ScreenEventHandler PushedScreen;
        public event ScreenEventHandler ReplacedSelf;
        public event EventHandler PoppedSelf;

        public void Initialize(IServiceProvider services) {
            CreateServiceContainer(services);

            _screenServices.SetService((Action)(() => {
                ReplacedSelf?.Invoke(this, new ScreenEventArgs(new VictoryScreen()));
            }));

            _spawner = new EntitySpawner(_screenServices);
            _screenServices.SetService(_spawner);

            _entityBuilder = new EntityBuilder(_screenServices);
            _screenServices.SetService(_entityBuilder);

            _soundPlayer = new SoundPlayer(_screenServices);
            _screenServices.SetService(_soundPlayer);

            _actions = new ActionList(_screenServices);
            _screenServices.SetService(_actions);

            _entityBuilder.Actions = _actions;

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

            foreach (TmxObject obj in map.ObjectGroups[0].Objects) {
                var center = new Vector2((float)(obj.X + obj.Width / 2f), (float)(obj.Y - obj.Height / 2f));

                switch (obj.Type) {
                    case "StartingCommander": {
                        _entityBuilder.CreateCommander(center, obj.Properties["Name"], 
                            new Weapon { Action = _actions.Punch, Icon = null }, PopColor());

                        _camera.Position = center - _renderer.Bounds.Size.ToVector2() / 2f;
                        break;
                    }
                    case "Commander": {
                        _entityBuilder.CreateRecruitableCommander(center, obj.Properties["Name"], 
                            new Weapon { Action = _actions.Punch, Icon = null }, PopColor());
                        break;
                    }
                    case "Recruit": {
                        _entityBuilder.CreateRecruit(center);
                        break;
                    }

                    case "Spider": {
                        _entityBuilder.CreateSpider(center);
                        break;
                    }
                    case "SpiderMother": {
                        _entityBuilder.CreateSpiderMother(center);
                        break;
                    }
                    case "BombGremlin": {
                        _entityBuilder.CreateBombGremlin(center);
                        break;
                    }
                    case "Gremlin": {
                        _entityBuilder.CreateGremlin(center);
                        break;
                    }
                    case "GremlinBoss": {
                        _entityBuilder.CreateGremlinBoss(center);
                        break;
                    }
                    case "Wizard": {
                        _entityBuilder.CreateWizard(center);
                        break;
                    }
                    case "Dragon": {
                        _entityBuilder.CreateDragon(center);
                        break;
                    }

                    case "Sword": {
                        _entityBuilder.CreateWeapon(center, new Weapon {
                            Action = _actions.SwordSlash,
                            Icon = _swordIconTexture
                        });
                        break;
                    }
                    case "Stick": {
                        _entityBuilder.CreateWeapon(center, new Weapon {
                            Action = _actions.StickSlash,
                            Icon = _stickIconTexture
                        });
                        break;
                    }
                    case "Bow": {
                        _entityBuilder.CreateWeapon(center, new Weapon {
                            Action = _actions.BowShoot,
                            Icon = _bowIconTexture
                        });
                        break;
                    }
                    case "Staff": {
                        _entityBuilder.CreateWeapon(center, new Weapon {
                            Action = _actions.StaffHeal,
                            Icon = _staffIconTexture
                        });
                        break;
                    }
                    case "Stat": {
                        _entityBuilder.CreateStatDrop(center);
                        break;
                    }
                }
            }

            //_entityBuilder.CreateCommander(new Vector2(32f, 32f), new Weapon { Action = _actions.Punch, Icon = null }, Color.SeaGreen);
            //_entityBuilder.CreateCommander(new Vector2(64f, 32f), new Weapon { Action = _actions.Punch, Icon = null }, Color.PaleVioletRed);

            _entityBuilder.CreateStatDrop(new Vector2(160f, 128f));
            _entityBuilder.CreateStatDrop(new Vector2(160f, 160f));

            int i = 0;
            foreach (Entity commander in _entityWorld.EntityManager.GetEntities(_commanderAspect)) {
                var transformComponent = commander.GetComponent<TransformComponent>();

                _fogEffect.Parameters["Center" + i].SetValue(_camera.ToView(transformComponent.Position - transformComponent.Offset));

                i++;
                if (i >= 8) {
                    break;
                }
            }

            for (; i < 8; i++) {
                _fogEffect.Parameters["Center" + i].SetValue(new Vector2(-256f));
            }
        }

        private void CreateServiceContainer(IServiceProvider services) {
            _screenServices = new ServiceContainer(services);

            _screenServices.SetService(_entityWorld);
            _screenServices.SetService(_camera);
            _screenServices.SetService(_random);
            _screenServices.SetService(_componentRemover);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _swordIconTexture = content.Load<Texture2D>("Textures/SwordIcon");
            _stickIconTexture = content.Load<Texture2D>("Textures/StickIcon");
            _bowIconTexture = content.Load<Texture2D>("Textures/BowIcon");
            _staffIconTexture = content.Load<Texture2D>("Textures/StaffIcon");

            _fogEffect = content.Load<Effect>("Effects/Fog");
            _fogEffect.Parameters["ClearRadiusSqr"].SetValue(256f * 256f);
            _fogEffect.Parameters["ClearBorderRadiusSqr"].SetValue(254f * 254f);
            _fogEffect.Parameters["FogColor"].SetValue(Color.Lerp(Color.White, Color.Black, 0.95f).ToVector4());
            _rendererSettings.LayerEffect = _fogEffect;
        }

        private void InitializeSystems(IServiceProvider services) {
            _entityWorld.SystemManager.SetSystem(new UnitInteractionsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitStrategySystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new TileCollisionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyTransformSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new RecruitingSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new WeaponPickupSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new StatPickupSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitActionSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new UnitCooldownSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BombSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new PacketSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new BodyPhysicsSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new ProjectileSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new ParticleAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new IndicatorAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new StatAnimatingSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new CommanderWeaponSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new HopSystem(services), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new LinkSystem(), GameLoopType.Update);
            _entityWorld.SystemManager.SetSystem(new AnimationSystem(), GameLoopType.Update);

            _entityWorld.SystemManager.SetSystem(new ShadowDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new PathDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new SpriteDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new StatDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new HealthBarDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new ParticleDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new IndicatorDrawingSystem(services), GameLoopType.Draw);
            _entityWorld.SystemManager.SetSystem(new HudDrawingSystem(services), GameLoopType.Draw);
        }

        public void Update(GameTime gameTime) {
            _squadController.Update();

            _entityWorld.Update(gameTime.ElapsedGameTime.Ticks);

            _spawner.Spawn();
            _componentRemover.Execute();

            int i = 0;
            foreach (Entity commander in _entityWorld.EntityManager.GetEntities(_commanderAspect)) {
                var transformComponent = commander.GetComponent<TransformComponent>();

                _fogEffect.Parameters["Center" + i].SetValue(_camera.ToView(transformComponent.Position - transformComponent.Offset));

                i++;
                if (i >= 8) {
                    break;
                }
            }

            if (i == 0) {
                ReplacedSelf?.Invoke(this, new ScreenEventArgs(new FailureScreen()));
                return;
            }

            for (; i < 8; i++) {
                _fogEffect.Parameters["Center" + i].SetValue(new Vector2(-256f));
            }
        }

        public void Draw(GameTime gameTime) {
            _renderer.Refresh();

            _fogEffect.Parameters["Dimensions"].SetValue(_renderer.Bounds.Size.ToVector2());

            _rendererSettings.TransformMatrix = _camera.GetTransformMatrix();
            _renderer.Begin(_rendererSettings);

            _tileMapRenderer.Draw(_tileMap);
            _entityWorld.Draw();

            _renderer.End();
            _renderer.Output();
        }
    }
}
