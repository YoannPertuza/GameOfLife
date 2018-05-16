using NCalc;

namespace GameOfLife
{
    public class RealCoordonnate : CoordonnatesOperation
    {
        public RealCoordonnate(Coordonnate baseCoord, RelativeCoordonnate relativeCoord)
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


    
}
