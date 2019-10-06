using Artemis;
using LD45.Actions;
using LD45.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD45.AI {
    public sealed class CommanderStrategy : IUnitStrategy {
        private const float _passingDistance = 12f;
        private const float _actionRadius = 32f;

        private readonly Aspect _aspect = Aspect.All(typeof(CommanderComponent), typeof(UnitComponent), typeof(BodyComponent));

        public void Update(Entity unit) {
            if (!_aspect.Interests(unit)) {
                return;
            }

            var commanderComponent = unit.GetComponent<CommanderComponent>();
            var unitComponent = unit.GetComponent<UnitComponent>();
            var bodyComponent = unit.GetComponent<BodyComponent>();

            if (commanderComponent.Path.Count > 0) {
                Vector2 targetPosition = commanderComponent.Path[0];

                float distance = Vector2.Distance(bodyComponent.Position, targetPosition);

                if (commanderComponent.Path.Count == 1 && distance < _actionRadius) {
                    if (unitComponent.Action != null) {
                        bool HasFlag(ActionFlags flag) {
                            return unitComponent.Action.Flags.HasFlag(flag);
                        }

                        float CalculateScore(Entity target) {
                            var targetUnitComponent = target.GetComponent<UnitComponent>();
                            var targetBodyComponent = target.GetComponent<BodyComponent>();

                            float targetDistance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);

                            float score = 1000000f;

                            if (HasFlag(ActionFlags.PrefersClose)) {
                                score -= targetDistance * unitComponent.DistanceWeight;
                            }
                            if (HasFlag(ActionFlags.PrefersFar)) {
                                score += targetDistance * unitComponent.DistanceWeight;
                            }
                            if (HasFlag(ActionFlags.PrefersLowHealth)) {
                                score -= targetUnitComponent.Health * unitComponent.HealthWeight;
                            }

                            if (targetUnitComponent.Team == unitComponent.Team && !unitComponent.Action.TargetsAllies) {
                                score = float.NegativeInfinity;
                            }
                            if (targetUnitComponent.Team != unitComponent.Team && unitComponent.Action.TargetsAllies) {
                                score = float.NegativeInfinity;
                            }

                            if (targetPosition != null) {
                                float distanceToSquad = Vector2.Distance(targetPosition, targetBodyComponent.Position);

                                if (distanceToSquad > _actionRadius + unitComponent.Action.Range) {
                                    score = float.NegativeInfinity;
                                }
                            }

                            return score;
                        }

                        Entity chosenTarget = unitComponent.VisibleUnits.OrderByDescending(CalculateScore).FirstOrDefault();

                        if (chosenTarget != null) {
                            var targetUnitComponent = chosenTarget.GetComponent<UnitComponent>();
                            var targetBodyComponent = chosenTarget.GetComponent<BodyComponent>();

                            if (CalculateScore(chosenTarget) >= 0f) {
                                float targetDistance = Vector2.Distance(targetBodyComponent.Position, bodyComponent.Position);

                                if (targetDistance > unitComponent.Action.Range) {
                                    bodyComponent.Force += Vector2.Normalize(targetBodyComponent.Position - bodyComponent.Position) * 150f;

                                    if (unit.HasComponent<HopComponent>()) {
                                        unit.GetComponent<HopComponent>().IsHopping = true;
                                    }
                                }
                                else if (unitComponent.CooldownTimer <= 0f) {
                                    unitComponent.ActionTarget = chosenTarget;
                                }

                                return;
                            }
                        }
                    }
                }

                if (distance > _passingDistance) {
                    bodyComponent.Force += Vector2.Normalize(targetPosition - bodyComponent.Position) * 150f;

                    if (unit.HasComponent<HopComponent>()) {
                        unit.GetComponent<HopComponent>().IsHopping = true;
                    }
                }
                else if (commanderComponent.Path.Count > 1) {
                    bodyComponent.Force += Vector2.Normalize(targetPosition - bodyComponent.Position) * 150f;

                    if (unit.HasComponent<HopComponent>()) {
                        unit.GetComponent<HopComponent>().IsHopping = true;
                    }

                    commanderComponent.Path.RemoveAt(0);
                }
            }
        }
    }
}
