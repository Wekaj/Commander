﻿using Artemis;
using LD45.Actions;
using LD45.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LD45.AI {
    public sealed class StandardUnitStrategy : IUnitStrategy {
        private const float _passingDistance = 12f;
        private const float _actionRadius = 32f;

        private readonly Aspect _aspect = Aspect.All(typeof(UnitComponent), typeof(BodyComponent));

        public void Update(Entity unit) {
            if (!_aspect.Interests(unit)) {
                return;
            }

            var unitComponent = unit.GetComponent<UnitComponent>();
            var bodyComponent = unit.GetComponent<BodyComponent>();

            Vector2? squadPosition = null;
            if (unitComponent.Commander != null) {
                var commanderComponent = unitComponent.Commander.GetComponent<CommanderComponent>();

                if (commanderComponent.Path.Count > 0) {
                    squadPosition = commanderComponent.Path[0];
                }
            }

            float unitDistanceToSquad = 0f;
            if (squadPosition != null) {
                unitDistanceToSquad = Vector2.Distance(squadPosition.Value, bodyComponent.Position);
            }

            if (unitDistanceToSquad < _actionRadius) {
                if (unitComponent.Action != null) {
                    bool HasFlag(ActionFlags flag) {
                        return unitComponent.Action.Flags.HasFlag(flag);
                    }

                    float CalculateScore(Entity target) {
                        var targetUnitComponent = target.GetComponent<UnitComponent>();
                        var targetBodyComponent = target.GetComponent<BodyComponent>();

                        float distance = Vector2.Distance(bodyComponent.Position, targetBodyComponent.Position);

                        float score = 1000000f;

                        if (HasFlag(ActionFlags.PrefersClose)) {
                            score -= distance * unitComponent.DistanceWeight;
                        }
                        if (HasFlag(ActionFlags.PrefersFar)) {
                            score += distance * unitComponent.DistanceWeight;
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

                        if (squadPosition != null) {
                            float distanceToSquad = Vector2.Distance(squadPosition.Value, targetBodyComponent.Position);

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
                            float distance = Vector2.Distance(targetBodyComponent.Position, bodyComponent.Position);

                            if (distance > unitComponent.Action.Range) {
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

            if (squadPosition != null) {
                Vector2 target = squadPosition.Value + unitComponent.Tendency;

                float targetDistance = Vector2.Distance(bodyComponent.Position, target);

                if (targetDistance > _passingDistance) {
                    bodyComponent.Force += Vector2.Normalize(target - bodyComponent.Position) * 150f;

                    if (unit.HasComponent<HopComponent>()) {
                        unit.GetComponent<HopComponent>().IsHopping = true;
                    }
                }
            }
        }
    }
}
