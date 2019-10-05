using Artemis;
using Artemis.System;
using LD45.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LD45.Systems {
    public sealed class UnitInteractionsSystem : EntitySystem {
        private const float _repulsionDistance = 8f;
        private const float _repulsionDistanceSqr = _repulsionDistance * _repulsionDistance;

        private readonly Aspect _commanderAspect = Aspect.All(typeof(CommanderComponent));

        private readonly List<Entity> _entities = new List<Entity>();

        public UnitInteractionsSystem() 
            : base(Aspect.All(typeof(UnitComponent), typeof(BodyComponent))) {
        }

        protected override void ProcessEntities(IDictionary<int, Entity> entities) {
            foreach (Entity entity in entities.Values) {
                var unitComponent = entity.GetComponent<UnitComponent>();

                unitComponent.VisibleUnits.Clear();

                _entities.Add(entity);
            }

            for (int i = 0; i < _entities.Count; i++) {
                Entity entity1 = _entities[i];

                var unitComponent1 = entity1.GetComponent<UnitComponent>();
                var bodyComponent1 = entity1.GetComponent<BodyComponent>();

                for (int j = i + 1; j < _entities.Count; j++) {
                    Entity entity2 = _entities[j];

                    var unitComponent2 = entity2.GetComponent<UnitComponent>();
                    var bodyComponent2 = entity2.GetComponent<BodyComponent>();

                    float distanceSqr = Vector2.DistanceSquared(bodyComponent1.Position, bodyComponent2.Position);

                    if (distanceSqr > 0f) {
                        if (distanceSqr < unitComponent1.SightRadius * unitComponent1.SightRadius) {
                            unitComponent1.VisibleUnits.Add(entity2);
                        }
                        if (distanceSqr < unitComponent2.SightRadius * unitComponent2.SightRadius) {
                            unitComponent2.VisibleUnits.Add(entity1);
                        }

                        if (distanceSqr < _repulsionDistanceSqr) {
                            var repulsionForce = Vector2.Normalize(bodyComponent1.Position - bodyComponent2.Position) * 500f;

                            float massSum = bodyComponent1.Mass + bodyComponent2.Mass;
                            if (!_commanderAspect.Interests(entity1)) {
                                bodyComponent1.Force += repulsionForce * bodyComponent2.Mass / massSum;
                            }
                            if (!_commanderAspect.Interests(entity2)) {
                                bodyComponent2.Force -= repulsionForce * bodyComponent1.Mass / massSum;
                            }
                        }
                    }
                }
            }

            _entities.Clear();
        }
    }
}
