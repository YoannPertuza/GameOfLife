namespace GameOfLife
{
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
