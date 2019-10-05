namespace LD45.Tiles {
    public enum TileType {
        Plains,
        Rocks,
    }

    public static class TileTypes {
        public static int GetTexture(this TileType type) {
            switch (type) {
                case TileType.Plains: {
                    return 1;
                }
                case TileType.Rocks: {
                    return 2;
                }
                default: {
                    return 0;
                }
            }
        }

        public static bool IsSolid(this TileType type) {
            switch (type) {
                case TileType.Plains: {
                    return false;
                }
                case TileType.Rocks: {
                    return true;
                }
                default: {
                    return true;
                }
            }
        }
    }
}
