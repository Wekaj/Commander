﻿using Artemis;
using LD45.Components;
using LD45.Graphics;
using LD45.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace LD45.Controllers {
    public sealed class SquadController {
        private const float _selectionRadius = 32f;
        private const float _minNodeDistance = 2f;

        private readonly Aspect _commanderAspect = Aspect.All(typeof(CommanderComponent), typeof(BodyComponent));

        private readonly InputManager _input;
        private readonly EntityWorld _entityWorld;
        private readonly Renderer2D _renderer;
        private readonly Camera2D _camera;

        private Entity _selectedCommander;
        private Vector2? _panPosition = null;

        public SquadController(IServiceProvider services) {
            _input = services.GetRequiredService<InputManager>();
            _entityWorld = services.GetRequiredService<EntityWorld>();
            _renderer = services.GetRequiredService<Renderer2D>();
            _camera = services.GetRequiredService<Camera2D>();
        }

        public void Update() {
            Vector2 mousePosition = _camera.ToWorld(_input.State.MouseState.Position.ToVector2() / _renderer.Scale);

            if (_selectedCommander != null && _selectedCommander.DeletingState) {
                _selectedCommander = null;
            }

            if (_input.Bindings.JustPressed(BindingId.RightClick)) {
                Entity closestCommander = null;
                float closestDistance = float.PositiveInfinity;

                Entity closestRing = null;
                float closestRingDistance = float.PositiveInfinity;

                foreach (Entity commander in _entityWorld.EntityManager.GetEntities(_commanderAspect)) {
                    var bodyComponent = commander.GetComponent<BodyComponent>();

                    float distance = Vector2.Distance(mousePosition, bodyComponent.Position);

                    if (distance < closestDistance) {
                        closestCommander = commander;
                        closestDistance = distance;
                    }

                    var commanderComponent = commander.GetComponent<CommanderComponent>();

                    if (commanderComponent.Path.Count > 0) {
                        float ringDistance = Vector2.Distance(mousePosition, commanderComponent.Path.Last());

                        if (ringDistance < closestRingDistance) {
                            closestRing = commander;
                            closestRingDistance = ringDistance;
                        }
                    }
                }

                if (closestCommander != null && closestDistance < _selectionRadius) {
                    _selectedCommander = closestCommander;

                    var commanderComponent = _selectedCommander.GetComponent<CommanderComponent>();
                    commanderComponent.Path.Clear();

                    commanderComponent.Path.Add(mousePosition);
                }
                else if (closestRing != null && closestRingDistance < _selectionRadius) {
                    _selectedCommander = closestRing;

                    var commanderComponent = _selectedCommander.GetComponent<CommanderComponent>();

                    commanderComponent.Path.Add(mousePosition);
                }
            }
            else if (_input.Bindings.IsPressed(BindingId.RightClick)) {
                if (_selectedCommander != null) {
                    var commanderComponent = _selectedCommander.GetComponent<CommanderComponent>();

                    if (commanderComponent.Path.Count > 0) {
                        float nodeDistance = Vector2.Distance(mousePosition, commanderComponent.Path.Last());

                        if (nodeDistance > _minNodeDistance) {
                            commanderComponent.Path.Add(mousePosition);
                        }
                    }
                    else {
                        commanderComponent.Path.Add(mousePosition);
                    }
                }
            }
            else if (_input.Bindings.JustReleased(BindingId.RightClick)) {
                _selectedCommander = null;
            }

            if (_input.Bindings.JustPressed(BindingId.LeftClick)) {
                _panPosition = mousePosition;
            }
            else if (_input.Bindings.IsPressed(BindingId.LeftClick) && _panPosition != null) {
                _camera.Position += _panPosition.Value - mousePosition;
            }
        }
    }
}
