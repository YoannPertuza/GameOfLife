namespace GameOfLife
{
    public class SimpleCell : CellFactory
    {
        public SimpleCell(bool alive)
        {
            this.alive = alive;
        }

        private bool alive;

        public Cell Cellule(Coordonnate coord)
        {
            return
                new Cell(
                    alive,
                    new Coordonnate(coord.CoordX(), coord.CoordY()),
                    new Neighborhood(coord.CoordX(), coord.CoordY())
                );
        }
    }
}
