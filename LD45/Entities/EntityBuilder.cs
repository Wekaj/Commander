using Artemis;
using LD45.Actions;
using LD45.AI;
using LD45.Components;
using LD45.Extensions;
using LD45.Graphics;
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
            _flagPoleTexture, _flagTexture;

        public EntityBuilder(IServiceProvider services) {
            _entityWorld = services.GetRequiredService<EntityWorld>();
            _random = services.GetRequiredService<Random>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _personTexture = content.Load<Texture2D>("Textures/Person");
            _spiderTexture = content.Load<Texture2D>("Textures/Spider");
            _swordIconTexture = content.Load<Texture2D>("Textures/SwordIcon");
            _flagPoleTexture = content.Load<Texture2D>("Textures/FlagPole");
            _flagTexture = content.Load<Texture2D>("Textures/Flag");
        }

        public Entity CreateUnit(Vector2 position, int team, IUnitStrategy strategy, IUnitAction action) {
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

        public Entity CreateSpider(Vector2 position) {
            Entity spider = CreateUnit(position, 1, new StandardUnitStrategy(), new HitAction());

            spider.GetComponent<UnitComponent>().StatDropRate = 0.5f;

            spider.AddComponent(new SpriteComponent {
                Texture = _spiderTexture,
                Origin = new Vector2(6f, 8f)
            });

            return spider;
        }

        public Entity CreatePerson(Vector2 position, IUnitStrategy strategy) {
            Entity person = CreateUnit(position, 0, strategy, null);

            person.AddComponent(new SpriteComponent {
                Texture = _personTexture,
                Origin = new Vector2(4.5f, 11f)
            });

            return person;
        }

        public Entity CreateCommander(Vector2 position, IWeapon weapon, Color flagColor) {
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

        private StatDropComponent CreateRandomStatDropComponent() {
            switch (_random.Next(8)) {
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
                        Message = "+1 Magic",
                        Magic = 1,
                    };
                }
                case 3: {
                    return new StatDropComponent {
                        Color = Color.Cyan,
                        Message = "+1 Resistance",
                        Resistance = 1,
                    };
                }
                case 4: {
                    return new StatDropComponent {
                        Color = Color.DarkGray,
                        Message = "+1 Force",
                        Force = 1,
                    };
                }
                case 5: {
                    return new StatDropComponent {
                        Color = Color.RosyBrown,
                        Message = "+1 Stability",
                        Stability = 1,
                    };
                }
                case 6: {
                    return new StatDropComponent {
                        Color = Color.Yellow,
                        Message = "+1 Speed",
                        Speed = 1,
                    };
                }
                case 7: {
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
