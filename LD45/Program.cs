using System;

namespace LD45 {
    public static class Program {
        [STAThread]
        private static void Main() {
            using (var game = new LD45Game()) {
                game.Run();
            }
        }
    }
}
