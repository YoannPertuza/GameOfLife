using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCalc;

namespace GameOfLife
{
    public class Coordonnate 
    {
        public Coordonnate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private int x;
        private int y;

        public Cell CelluleIn(IEnumerable<Cell> boardCells)
        {
            return boardCells.Where(cells => cells.Matche(x, y)).First();
        }

        public int CoordX()
        {
            return this.x;
        }

        public int CoordY()
        {
            return this.y;
        }

        public override int GetHashCode()
        {
            return this.x * 10 + this.y + this.y * 10 + this.x; 
        }

    }


    public class CoordonnateCompare :IComparer<Coordonnate>
    {
        
        public int Compare(Coordonnate x, Coordonnate y)
        {
            if (x.GetHashCode() == y.GetHashCode())
            {
                return 0;
            }
            else if (x.GetHashCode() > y.GetHashCode())
            {
                return 1;
            }
            else 
            {
                return -1;
            }
        }
    }

    public class CoordonnateComparer : IEqualityComparer<Coordonnate>
    {

        public bool Equals(Coordonnate x, Coordonnate y)
        {
            return x.CoordX() == y.CoordX() && x.CoordY() == y.CoordY();
        }

        public int GetHashCode(Coordonnate obj)
        {
            return obj.GetHashCode();
        }
    }

    public interface CoordonnatesOperation
    {
        Coordonnate Select();
    }

    public class MinCoordonnate : CoordonnatesOperation
    {
        public MinCoordonnate(BoardCoordonnates board)
        {
            this.board = board;
        }

        private BoardCoordonnates board;

        public Coordonnate Select()
        {
            return board.Coordonnates().OrderBy(coord => coord, new CoordonnateCompare()).First();
        }
    }

    public class FromRelativeCoordonnate : CoordonnatesOperation
    {
        public FromRelativeCoordonnate(Coordonnate baseCoord, RelativeCoordonnate relativeCoord)
        {
            this.baseCoord = baseCoord;
            this.relativeCoord = relativeCoord;
        }

        private Coordonnate baseCoord;
        private RelativeCoordonnate relativeCoord;

        public Coordonnate Select()
        {
            var coordX = new Expression(relativeCoord.CoordXCalculation());
            coordX.Parameters["x"] = baseCoord.CoordX();

            var coordY = new Expression(relativeCoord.CoordYCalculation());
            coordY.Parameters["y"] = baseCoord.CoordY();

            return new Coordonnate((int)coordX.Evaluate(), (int)coordY.Evaluate());
        }
    }

    public class InRelativeCoordonnates
    {
        public InRelativeCoordonnates(BoardCoordonnates board)
        {
            this.board = board;
            this.baseCoord = new MinCoordonnate(board);
        }

        private BoardCoordonnates board;
        private CoordonnatesOperation baseCoord; 

        public IEnumerable<RelativeCoordonnate> RelativeCoord()
        {
           return board.Coordonnates().Select(coord => new RelativeCoordonnate(baseCoord.Select(), coord));
        }
    }

    public class RelativeCoordonnate
    {
        public RelativeCoordonnate(Coordonnate baseCoord, Coordonnate coordToTransform)
        {
            this.baseCoord = baseCoord;
            this.coordToTransform = coordToTransform;
        }

        private Coordonnate baseCoord;
        private Coordonnate coordToTransform;

        public string CoordXCalculation()
        {
            return string.Format("[x] + ({0})", coordToTransform.CoordX() - baseCoord.CoordX());
        }

        public string CoordYCalculation()
        {
            return string.Format("[y] + ({0})", coordToTransform.CoordY() - baseCoord.CoordY());
        }
    }


    
}
