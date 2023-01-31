using System;
using System.Collections.Generic;

namespace PathFinding
{
    public class AStarCellsHandler
    {
        private AStarCell[][] AStarGrid { get; set; }
        private List<AStarCell> touchedCells;

        private int[,] hexTouchArroundOffsets = new int[,]
        {
            { -1, -1 },
            {  1, -1 },
            { -2,  0 },
            {  2,  0 },
            { -1,  1 },
            {  1,  1 },
        };

        public AStarCellsHandler(AStarCell[][] aStarGrid)
        {
            AStarGrid = aStarGrid;
            touchedCells = new List<AStarCell>(AStarGrid.Length * AStarGrid[0].Length);
        }

        public void ResetTouchedCells()
        {
            foreach (var touchedCell in touchedCells)
            {
                touchedCell.Reset();
            }
            touchedCells.Clear();
        }

        public void TouchStart(int x, int y)
        {
            Touch(x, y).Start();
        }

        public void TouchEnd(int x, int y)
        {
            Touch(x, y).End();
        }

        public AStarCell Touch(int x, int y)
        {
            var cell = AStarGrid[x][y];
            touchedCells.Add(cell);
            return cell;
        }

        public IEnumerable<AStarCell> TouchArroundPoint(int x, int y)
        {
            for (int i = 0; i < hexTouchArroundOffsets.GetLength(0); i++)
            {
                var targetX = x + hexTouchArroundOffsets[i, 0];
                var targetY = y + hexTouchArroundOffsets[i, 1];
                if (targetX >= 0 && targetX < AStarGrid.Length)
                {
                    if (targetY >= 0 && targetY < AStarGrid[0].Length)
                    {
                        yield return Touch(targetX, targetY);
                    }
                }
            }
        }
    }
}
