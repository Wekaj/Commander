using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LD45.Screens {
    public sealed class ScreenManager {
        private readonly List<IScreen> _screens = new List<IScreen>();

        private readonly IServiceProvider _services;

        public ScreenManager(IServiceProvider services) {
            _services = services;
        }

        public bool IsEmpty => _screens.Count == 0;

        public void Push(IScreen screen) {
            if (_screens.Count > 0) {
                RemoveHandlers(Peek());
            }

            _screens.Add(screen);
            AddHandlers(screen);

            screen.Initialize(_services);
        }

        public IScreen Pop() {
            IScreen screen = Peek();

            RemoveHandlers(screen);
            _screens.RemoveAt(_screens.Count - 1);

            if (_screens.Count > 0) {
                AddHandlers(Peek());
            }

            return screen;
        }

        public IScreen Peek() {
            return _screens[_screens.Count - 1];
        }

        public void Update(GameTime gameTime) {
            if (!IsEmpty) {
                Peek().Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime) {
            if (!IsEmpty) {
                Peek().Draw(gameTime);
            }
        }

        private void AddHandlers(IScreen screen) {
            screen.PushedScreen += Screen_PushedScreen;
            screen.ReplacedSelf += Screen_ReplacedSelf;
            screen.PoppedSelf += Screen_PoppedSelf;
        }

        private void RemoveHandlers(IScreen screen) {
            screen.PushedScreen -= Screen_PushedScreen;
            screen.ReplacedSelf -= Screen_ReplacedSelf;
            screen.PoppedSelf -= Screen_PoppedSelf;
        }

        private void Screen_PushedScreen(object sender, ScreenEventArgs e) {
            Push(e.Screen);
        }

        private void Screen_ReplacedSelf(object sender, ScreenEventArgs e) {
            Pop();
            Push(e.Screen);
        }

        private void Screen_PoppedSelf(object sender, EventArgs e) {
            Pop();
        }
    }
}
