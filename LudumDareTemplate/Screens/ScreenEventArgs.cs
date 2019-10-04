using System;

namespace LudumDareTemplate.Screens {
    public delegate void ScreenEventHandler(object sender, ScreenEventArgs e);

    public class ScreenEventArgs : EventArgs {
        public ScreenEventArgs(IScreen screen) {
            Screen = screen;
        }

        public IScreen Screen { get; }
    }
}
