using System.Linq;

namespace GameOfLife
{
    public class ReferenceCoordonnate : CoordonnatesOperation
    {
        public ReferenceCoordonnate(BoardCoordonnates board)
        {
            this.board = board;
        }

        private BoardCoordonnates board;

        public Coordonnate Select()
        {
            return board.Coordonnates().OrderBy(coord => coord, new CoordonnateCompare()).First();
        }
    }


    
}
