namespace LD45.Tiles {
    public sealed class TileMap {
        private readonly Tile[,] _tiles;

        public TileMap(int width, int height) {
            _tiles = new Tile[width, height];

            Width = width;
            Height = height;
        }

        public Tile this[int x, int y] {
            get => _tiles[x, y];
            set => _tiles[x, y] = value;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
