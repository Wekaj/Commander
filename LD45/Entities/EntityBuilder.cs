using Artemis;
using LD45.Actions;
using LD45.AI;
using LD45.Components;
using LD45.Extensions;
using LD45.Graphics;
using LD45.Utilities;
using LD45.Weapons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Entities {
    public sealed class EntityBuilder {
        private readonly EntityWorld _entityWorld;
        private readonly Random _random;

        private Texture2D _personTexture, _spiderTexture, _swordIconTexture,
            _flagPoleTexture, _flagTexture, _spiderQueenTexture, _bombGremlinTexture,
            _bombTexture, _explosionTexture, _gremlinTexture, _gremlinBossTexture,
            _wizardTexture, _dragonTexture;
        private Texture2D[] _personTextures = new Texture2D[10];

        public EntityBuilder(IServiceProvider services) {
            _entityWorld = services.GetRequiredService<EntityWorld>();
            _random = services.GetRequiredService<Random>();

            LoadContent(services);
        }

        public ActionList Actions { get; set; }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _personTexture = content.Load<Texture2D>("Textures/Person1");
            _spiderTexture = content.Load<Texture2D>("Textures/Spider");
            _spiderQueenTexture = content.Load<Texture2D>("Textures/SpiderMother");
            _swordIconTexture = content.Load<Texture2D>("Textures/SwordIcon");
            _flagPoleTexture = content.Load<Texture2D>("Textures/FlagPole");
            _flagTexture = content.Load<Texture2D>("Textures/Flag");
            _bombGremlinTexture = content.Load<Texture2D>("Textures/BombGremlin");
            _bombTexture = content.Load<Texture2D>("Textures/Bomb");
            _explosionTexture = content.Load<Texture2D>("Textures/Explosion");
            _gremlinTexture = content.Load<Texture2D>("Textures/Gremlin");
            _gremlinBossTexture = content.Load<Texture2D>("Textures/GremlinBoss");
            _wizardTexture = content.Load<Texture2D>("Textures/Wizard");
            _dragonTexture = content.Load<Texture2D>("Textures/HellDragon");

            for (int i = 0; i < _personTextures.Length; i++) {
                _personTextures[i] = content.Load<Texture2D>("Textures/Person" + (i + 1));
            }
        }

        public Entity CreateUnit(Vector2 position, int team, int health, float mass, IUnitStrategy strategy, IUnitAction action) {
            Entity unit = _entityWorld.CreateEntity();

            unit.AddComponent(new BodyComponent {
                Position = position,
                Mass = mass,
            });
            unit.AddComponent(new TransformComponent());
            unit.AddComponent(new UnitComponent {
                MaxHealth = health,
                Health = health,
                Team = team,
                Strategy = strategy,
                Action = action,
                Tendency = _random.NextUnitVector() * _random.NextSingle(8f)
            });
            unit.AddComponent(new ShadowComponent {
                Type = ShadowType.Small
            });
            unit.AddComponent(new HopComponent());

            return unit;
        }

        public Entity CreateSpider(Vector2 position) {
            Entity spider = CreateUnit(position, 1, 20, 0.5f, new StandardUnitStrategy(), Actions.SpiderBite);

            spider.GetComponent<UnitComponent>().StatDropRate = 0.1f;

            spider.AddComponent(new SpriteComponent {
                Texture = _spiderTexture,
                Origin = new Vector2(6f, 8f)
            });

            return spider;
        }

        public Entity CreateSpiderMother(Vector2 position) {
            Entity spider = CreateUnit(position, 1, 200, 5f, new StandardUnitStrategy(), Actions.SpiderSpit);

            spider.GetComponent<UnitComponent>().StatDropRate = 1f;
            spider.GetComponent<UnitComponent>().ActionOrder.AddRange(new IUnitAction[] {
                Actions.SpiderSpit,
                Actions.SpiderSpit,
                Actions.SummonSpider,
            });

            spider.GetComponent<ShadowComponent>().Type = ShadowType.Big;

            spider.AddComponent(new SpriteComponent {
                Texture = _spiderQueenTexture,
                Origin = new Vector2(16f, 18f)
            });

            return spider;
        }

        public Entity CreateBombGremlin(Vector2 position) {
            Entity gremlin = CreateUnit(position, 1, 50, 1f, new StandardUnitStrategy(), Actions.ThrowBomb);

            gremlin.GetComponent<UnitComponent>().StatDropRate = 0.25f;

            gremlin.AddComponent(new SpriteComponent {
                Texture = _bombGremlinTexture,
                Origin = new Vector2(9f, 22f)
            });

            return gremlin;
        }

        public Entity CreateGremlin(Vector2 position) {
            Entity gremlin = CreateUnit(position, 1, 50, 1f, new StandardUnitStrategy(), Actions.GremlinHit);

            gremlin.GetComponent<UnitComponent>().StatDropRate = 0.15f;

            gremlin.AddComponent(new SpriteComponent {
                Texture = _gremlinTexture,
                Origin = new Vector2(9f, 22f)
            });

            return gremlin;
        }

        public Entity CreateWizard(Vector2 position) {
            Entity wizard = CreateUnit(position, 1, 50, 1f, new StandardUnitStrategy(), Actions.WizardMagic);

            wizard.GetComponent<UnitComponent>().StatDropRate = 0.3f;

            wizard.AddComponent(new SpriteComponent {
                Texture = _wizardTexture,
                Origin = new Vector2(5.5f, 14f)
            });

            return wizard;
        }

        public Entity CreateDragon(Vector2 position) {
            Entity dragon = CreateUnit(position, 1, 1000, 1f, new StandardUnitStrategy(), Actions.DragonBreath);

            dragon.GetComponent<UnitComponent>().StatDropRate = 1f;
            dragon.GetComponent<UnitComponent>().IsBoss = true;
            dragon.GetComponent<ShadowComponent>().Type = ShadowType.Big;

            dragon.AddComponent(new SpriteComponent {
                Texture = _dragonTexture,
                Origin = new Vector2(17f, 23f)
            });

            return dragon;
        }

        public Entity CreateGremlinBoss(Vector2 position) {
            Entity gremlin = CreateUnit(position, 1, 300, 1f, new StandardUnitStrategy(), Actions.GremlinBossHit);

            gremlin.GetComponent<UnitComponent>().StatDropRate = 1f;

            gremlin.AddComponent(new SpriteComponent {
                Texture = _gremlinBossTexture,
                Origin = new Vector2(12f, 22f)
            });

            return gremlin;
        }

        public Entity CreateBomb(Vector2 start, Vector2 end) {
            Entity bomb = _entityWorld.CreateEntity();

            float distance = Vector2.Distance(start, end);

            float speed = 64f * MathHelper.Clamp(1f - (32f - distance) / 32f, 0.1f, 1f);
            float height = 64f * MathHelper.Clamp(1f - (32f - distance) / 32f, 0.1f, 1f);

            bomb.AddComponent(new BodyComponent {
                Position = start
            });
            bomb.AddComponent(new TransformComponent());
            bomb.AddComponent(new ShadowComponent {
                Type = ShadowType.Small
            });
            bomb.AddComponent(new ProjectileComponent {
                Start = start,
                End = end,
                Speed = speed,
                HeightFunction = p => (float)Math.Sin(p * Math.PI) * height
            });
            bomb.AddComponent(new SpriteComponent {
                Texture = _bombTexture,
                Origin = new Vector2(8f, 12f)
            });
            bomb.AddComponent(new BombComponent {
                Countdown = 4f,
                Damage = 50,
                Radius = 24f,
                Force = 400f,
            });

            return bomb;
        }

        public Entity CreatePerson(Vector2 position, IUnitStrategy strategy) {
            Entity person = CreateUnit(position, 0, 100, 1f, strategy, null);

            person.AddComponent(new SpriteComponent {
                Texture = _personTextures[_random.Next(_personTextures.Length)],
                Origin = new Vector2(4.5f, 11f)
            });

            return person;
        }

        public Entity CreateCommander(Vector2 position, string name, IWeapon weapon, Color flagColor) {
            Entity commander = CreatePerson(position, new CommanderStrategy());

            commander.AddComponent(new CommanderComponent {
                Weapon = weapon,
                FlagColor = flagColor,
                SquadName = SquadNames.Generate(_random),
                Name = "Cmdr. " + name,
            });

            Entity flagPole = _entityWorld.CreateEntity();
            flagPole.AddComponent(new TransformComponent());
            flagPole.AddComponent(new SpriteComponent {
                Texture = _flagPoleTexture,
                Origin = new Vector2(15.5f, 26f - 0.001f),
            });
            flagPole.AddComponent(new LinkComponent {
                Parent = commander,
                Offset = new Vector2(0f, -0.001f),
            });

            Entity flag = _entityWorld.CreateEntity();
            flag.AddComponent(new TransformComponent());
            flag.AddComponent(new SpriteComponent {
                Texture = _flagTexture,
                Origin = new Vector2(15.5f, 26f - 0.002f),
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

        public Entity CreateRecruitableCommander(Vector2 position, string name, IWeapon weapon, Color flagColor) {
            Entity commander = CreateCommander(position, name, weapon, flagColor);

            var commanderComponent = commander.GetComponent<CommanderComponent>();
            commander.RemoveComponent<CommanderComponent>();
            commander.AddComponent(new RecruitableComponent {
                CommanderComponent = commanderComponent
            });

            return commander;
        }

        public Entity CreateRecruit(Vector2 position) {
            Entity recruit = CreatePerson(position, new StandardUnitStrategy());

            recruit.AddComponent(new RecruitableComponent());

            return recruit;
        }

        public Entity CreateWeapon(Vector2 position, IWeapon weapon) {
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

        public Entity CreateStatDrop(Vector2 position) {
            Entity stat = _entityWorld.CreateEntity();

            stat.AddComponent(new BodyComponent {
                Position = position
            });
            stat.AddComponent(new TransformComponent {
                Offset = new Vector2(0f, -7f)
            });
            stat.AddComponent(CreateRandomStatDropComponent());
            stat.AddComponent(new ShadowComponent {
                Type = ShadowType.Small
            });

            return stat;
        }

        public void CreateExplosion(Vector2 position) {
            Entity explosion = _entityWorld.CreateEntity();
            explosion.AddComponent(new ParticleComponent {
                Texture = _explosionTexture,
                Origin = new Vector2(32f, 32f),
                ScaleFunction = p => new Vector2(1f - p),
                LifeDuration = 0.2f,
            });
            explosion.AddComponent(new TransformComponent {
                Position = position
            });
        }

        private StatDropComponent CreateRandomStatDropComponent() {
            switch (_random.Next(7)) {
                case 0: {
                    return new StatDropComponent {
                        Color = Color.Red,
                        Message = "+1 Strength",
                        Strength = 1,
                    };
                }
                case 1: {
                    return new StatDropComponent {
                        Color = Color.Blue,
                        Message = "+1 Armor",
                        Armor = 1,
                    };
                }
                case 2: {
                    return new StatDropComponent {
                        Color = Color.Purple,
                        Message = "+1 Resistance",
                        Resistance = 1,
                    };
                }
                case 3: {
                    return new StatDropComponent {
                        Color = Color.DarkGray,
                        Message = "+1 Force",
                        Force = 1,
                    };
                }
                case 4: {
                    return new StatDropComponent {
                        Color = Color.Black,
                        Message = "+1 Stability",
                        Stability = 1,
                    };
                }
                case 5: {
                    return new StatDropComponent {
                        Color = Color.Yellow,
                        Message = "+1 Speed",
                        Speed = 1,
                    };
                }
                case 6: {
                    return new StatDropComponent {
                        Color = Color.Green,
                        Message = "+1 Accuracy",
                        Accuracy = 1,
                    };
                }
                default: {
                    return new StatDropComponent {
                        Color = Color.Black,
                        Message = "???",
                        Speed = 10,
                    };
                }
            }
        }
    }
}
