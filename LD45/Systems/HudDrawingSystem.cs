﻿using Artemis;
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
        private readonly RendererSettings _rendererSettings = new RendererSettings {
            SamplerState = SamplerState.PointClamp
        };

        private SpriteFont _font;
        private Texture2D _labelTexture, _bannerTexture, _bannerLeftTexture, _shieldTexture, _tagTexture;

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
            _tagTexture = content.Load<Texture2D>("Textures/Tag");
        }

        protected override void Begin() {
            base.Begin();

            _topLeft = new Vector2(12f);
            _indentNext = true;

            // A little hacky.
            _renderer.End();
            _renderer.Begin(_rendererSettings);
        }

        public override void Process(Entity entity) {
            var commanderComponent = entity.GetComponent<CommanderComponent>();
            var spriteComponent = entity.GetComponent<SpriteComponent>();

            _renderer.Draw(_labelTexture, _topLeft);
            _renderer.Draw(spriteComponent.Texture, _topLeft + new Vector2(9f, 5f));
            _renderer.Draw(_font, commanderComponent.Name + " (" + commanderComponent.StatSum() + ")", _topLeft + new Vector2(26f, 7f), Color.Lerp(Color.Black, Color.White, 0.1f));

            if (commanderComponent.Weapon.Icon != null) {
                _renderer.Draw(_tagTexture, _topLeft + new Vector2(192f, 0f));
                _renderer.Draw(commanderComponent.Weapon.Icon, _topLeft + new Vector2(195f, 3f));
            }

            if (commanderComponent.Squad.Count > 0) {
                _renderer.Draw(_bannerLeftTexture, _topLeft + new Vector2(16f, 14f), color: commanderComponent.FlagColor);

                var bannerRect = new Rectangle(0, 0, (int)_font.MeasureString(commanderComponent.SquadName).X + 8, _bannerTexture.Height);
                _renderer.Draw(_bannerTexture, _topLeft + new Vector2(16f + _bannerLeftTexture.Width, 14f),
                    color: commanderComponent.FlagColor, sourceRectangle: bannerRect);

                _renderer.Draw(_font, commanderComponent.SquadName, _topLeft + new Vector2(23f, 22f), Color.Lerp(Color.Black, Color.White, 0.25f));
                _renderer.Draw(_font, commanderComponent.SquadName, _topLeft + new Vector2(22f, 21f), Color.Lerp(Color.Black, Color.White, 0.95f));
                _renderer.Draw(_shieldTexture, _topLeft + new Vector2(16f + bannerRect.Width, 14f));
            }

            _topLeft = new Vector2(12f + (_indentNext ? _indentWidth : 0f), _topLeft.Y + _labelGap);

            _indentNext = !_indentNext;
        }
    }
}
