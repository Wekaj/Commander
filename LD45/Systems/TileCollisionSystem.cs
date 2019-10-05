using Artemis;
using Artemis.System;
using LD45.Components;
using LD45.Tiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;

namespace LD45.Systems {
    public sealed class TileCollisionSystem : EntityProcessingSystem {
        private readonly TileMap _tileMap;

        public TileCollisionSystem(IServiceProvider services) 
            : base(Aspect.All(typeof(BodyComponent))) {

            _tileMap = services.GetRequiredService<TileMap>();
        }

        public override void Process(Entity entity) {
            var bodyComponent = entity.GetComponent<BodyComponent>();

            int tileX = (int)Math.Floor(bodyComponent.Position.X / Constants.TileSize);
            int tileY = (int)Math.Floor(bodyComponent.Position.Y / Constants.TileSize);

            if (_tileMap.IsWithinBounds(tileX, tileY)) {
                if (_tileMap[tileX, tileY].Type.IsSolid()) {
                    bodyComponent.Force += Vector2.Normalize(bodyComponent.Position - new Vector2(tileX + 0.5f, tileY + 0.5f) * Constants.TileSize) * 500f;
                }
            }
        }
    }
}
