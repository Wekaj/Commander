using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LD45.Systems {
    public sealed class HudDrawingSystem : EntityProcessingSystem {
        private const float _labelGap = 48f;
        private const float _indentWidth = 12f;

        private readonly Renderer2D _renderer;

        private SpriteFont _font;
        private Texture2D _labelTexture, _bannerTexture, _bannerLeftTexture, _shieldTexture;

        private Vector2 _topLeft;
        private bool _indentNext = true;

        public HudDrawingSystem(IServiceProvider services)
            : base(Aspect.All(typeof(CommanderComponent), typeof(SpriteComponent))) {

            _renderer = services.GetRequiredService<Renderer2D>();

            LoadContent(services);
        }

        private void LoadContent(IServiceProvider services) {
            var content = services.GetRequiredService<ContentManager>();

            _font = content.Load<SpriteFont>("Fonts/Small");
            _labelTexture = content.Load<Texture2D>("Textures/CommanderLabel");
            _bannerTexture = content.Load<Texture2D>("Textures/CommanderBanner");
            _bannerLeftTexture = content.Load<Texture2D>("Textures/BannerLeft");
            _shieldTexture = content.Load<Texture2D>("Textures/CommanderShield");
        }

        protected override void Begin() {
            base.Begin();

            _topLeft = new Vector2(12f);
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var spriteComponent = entity.GetComponent<SpriteComponent>();

            _renderer.Draw(_labelTexture, _topLeft);
            _renderer.Draw(spriteComponent.Texture, _topLeft + new Vector2(9f, 5f));
            _renderer.Draw(_font, "Cmdr. David", _topLeft + new Vector2(26f, 7f), Color.Lerp(Color.Black, Color.White, 0.1f));

            if (commanderComponent.Squad.Count > 0) {
                _renderer.Draw(_bannerLeftTexture, _topLeft + new Vector2(16f, 14f), color: commanderComponent.FlagColor);

                var bannerRect = new Rectangle(0, 0, (int)_font.MeasureString(commanderComponent.SquadName).X + 8, _bannerTexture.Height);
                _renderer.Draw(_bannerTexture, _topLeft + new Vector2(16f + _bannerLeftTexture.Width, 14f),
                    color: commanderComponent.FlagColor, sourceRectangle: bannerRect);

                _renderer.Draw(_font, commanderComponent.SquadName, _topLeft + new Vector2(23f, 22f), Color.Lerp(Color.Black, Color.White, 0.25f));
                _renderer.Draw(_font, commanderComponent.SquadName, _topLeft + new Vector2(22f, 21f), Color.Lerp(Color.Black, Color.White, 0.95f));
                _renderer.Draw(_shieldTexture, _topLeft + new Vector2(16f + bannerRect.Width, 14f));
                _renderer.Draw(_font, "" + commanderComponent.StatSum(), _topLeft + new Vector2(25f + bannerRect.Width, 23f),
                    Color.Lerp(commanderComponent.FlagColor, Color.Black, 0.5f), origin: new Vector2(0.5f));
                _renderer.Draw(_font, "" + commanderComponent.StatSum(), _topLeft + new Vector2(26f + bannerRect.Width, 24f),
                    commanderComponent.FlagColor, origin: new Vector2(0.5f));

                for (int i = 0; i < commanderComponent.Squad.Count; i++) {
                    var followerSpriteComponent = commanderComponent.Squad[i].GetComponent<SpriteComponent>();

                    _renderer.Draw(followerSpriteComponent.Texture, _topLeft + new Vector2(16f + bannerRect.Width - i * 8f, 30f));
                }
            }

            _topLeft += new Vector2(_indentNext ? _indentWidth : 0f, _labelGap);

            _indentNext = !_indentNext;
        }
    }
}
