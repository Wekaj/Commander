using LD45.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace LD45.Audio {
    public sealed class SoundPlayer {
        private readonly ContentManager _content;
        private readonly Random _random = new Random();

        public SoundPlayer(IServiceProvider services) {
            _content = services.GetRequiredService<ContentManager>();
        }

        public void Play(string sound) {
            _content.Load<SoundEffect>("Sounds/" + sound).Play(1f, -0.25f + _random.NextSingle() / 2f, 0f);
        }

        public SoundEffectInstance CreateInstance(string sound) {
            return _content.Load<SoundEffect>("Sounds/" + sound).CreateInstance();
        }
    }
}
