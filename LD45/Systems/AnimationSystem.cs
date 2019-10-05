using Artemis;
using Artemis.System;
using LD45.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD45.Systems {
    public sealed class AnimationSystem : EntityProcessingSystem {
        private float _deltaTime;

        public AnimationSystem() 
            : base(Aspect.All(typeof(SpriteComponent), typeof(AnimationComponent))) {
        }

        protected override void Begin() {
            base.Begin();

            _deltaTime = (float)TimeSpan.FromTicks(EntityWorld.Delta).TotalSeconds;
        }

        public override void Process(Entity entity) {
            var spriteComponent = entity.GetComponent<SpriteComponent>();
            var animationComponent = entity.GetComponent<AnimationComponent>();

            animationComponent.Animation.Timer += _deltaTime;

            spriteComponent.Texture = animationComponent.Animation.Texture;
            spriteComponent.SourceRectangle = animationComponent.Animation.GetCurrentFrame();
        }
    }
}
