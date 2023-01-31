using System;
using System.Collections.Generic;

namespace PathFinding
{
    public class AStarCellsHandler
    {
        private AStarCell[][] AStarGrid { get; set; }
        private List<AStarCell> touchedCells;

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

        public IEnumerable<AStarCell> TouchArroundPoint(int x, int y, bool noDiagonals)
        {
            for (int xOff = -1; xOff <= 1; xOff++)
            {
                for (int yOff = -1; yOff <= 1; yOff++)
                {
                    if (xOff == 0 && yOff == 0) continue;
                    if (noDiagonals)
                    {
                        var diagonalSum = Math.Abs(xOff) + Math.Abs(yOff);
                        if (diagonalSum >= 2)
                        {
                            continue;
                        }
                    }

                    var targetX = x + xOff;
                    var targetY = y + yOff;
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
}
