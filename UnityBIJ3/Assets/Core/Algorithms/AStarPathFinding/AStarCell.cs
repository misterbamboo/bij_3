using System;

namespace PathFinding
{
    public class AStarCell
    {
        public int XPos { get; }

        public int YPos { get; }

        public int ParentX { get; private set; }

        public int ParentY { get; private set; }

        public bool IsStartCell { get; private set; }

        public bool IsEndCell { get; private set; }

        public AStarCell(int x, int y)
        {
            XPos = x;
            YPos = y;
        }

        public void Reset()
        {
            ParentX = 0;
            ParentY = 0;
            GCost = 0;
            HCost = 0;
            FCost = 0;
            State = AStarCellState.Initial;
            IsStartCell = false;
            IsEndCell = false;
        }

        /// <summary>
        /// Cost from starting node 
        /// </summary>
        public int GCost { get; private set; }

        /// <summary>
        /// Cost to ending node
        /// </summary>
        public int HCost { get; private set; }

        /// <summary>
        /// Total cell cost
        /// </summary>
        public int FCost { get; private set; }

        public AStarCellState State { get; private set; }

        public void Close()
        {
            State = AStarCellState.Closed;
        }

        private void DiscovedFrom(int xSource, int ySource)
        {
            State = AStarCellState.Discoved;
            ParentX = xSource;
            ParentY = ySource;
        }

        public void Start()
        {
            IsStartCell = true;
        }

        public void End()
        {
            IsEndCell = true;
        }

        public void ChangeToPath()
        {
            if (State == AStarCellState.Closed)
            {
                State = AStarCellState.Path;
            }
        }

        public void UpdateCost(AStarCell previousCell, int ex, int ey)
        {
            var tempGCost = GetGCost(previousCell);
            var tempHCost = GetHCost(ex, ey);
            var tempFCost = tempGCost + tempHCost;

            if (ShouldUpdateValue(tempFCost))
            {
                UpdateFCost(previousCell, tempGCost, tempHCost);
            }
        }

        private void UpdateFCost(AStarCell previousCell, int tempGCost, int tempHCost)
        {
            GCost = tempGCost;
            HCost = tempHCost;
            FCost = GCost + HCost;
            DiscovedFrom(previousCell.XPos, previousCell.YPos);
        }

        private bool ShouldUpdateValue(int tempFCost)
        {
            return FCost == 0 || tempFCost < FCost;
        }

        private int GetGCost(AStarCell previousCell)
        {
            return previousCell.GCost + ComputeCostToPoint(previousCell.XPos, previousCell.YPos);
        }

        private int GetHCost(int ex, int ey)
        {
            return ComputeCostToPoint(ex, ey);
        }

        private int ComputeCostToPoint(int x, int y)
        {
            var xDiff = Math.Abs(x - XPos);
            var yDiff = Math.Abs(y - YPos);

            return xDiff + yDiff;
        }
    }
}