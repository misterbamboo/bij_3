using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathFinding
{
    public class AStarPathFinding
    {
        Stack<AStarCell> pathStack;
        public IEnumerable<Vector2> Path { get; private set; }

        List<AStarCell> nextCandidates;

        private AStarCellsHandler cells;
        private int sx;
        private int sy;
        private int ex;
        private int ey;

        public Vector2 CurrentPos => currentCandidate == null ? new Vector2(-1, -1) : new Vector2(currentCandidate.XPos, currentCandidate.YPos);

        private AStarCell currentCandidate;
        public bool PathFound { get; private set; }
        public bool NoDiagonals { get; set; }
        public AStarCell[][] AStarGrid { get; private set; }
        private IAStarGridInfo GridInfo { get; }

        public AStarPathFinding(IAStarGridInfo gridInfo)
        {
            GridInfo = gridInfo;
            GenerateAStarGrid(GridInfo);

            this.cells = new AStarCellsHandler(AStarGrid);
            nextCandidates = new List<AStarCell>(gridInfo.Width * gridInfo.Height);
            pathStack = new Stack<AStarCell>();
            Path = Enumerable.Empty<Vector2>();
        }

        private void GenerateAStarGrid(IAStarGridInfo gridInfo)
        {
            AStarGrid = new AStarCell[gridInfo.Width][];
            for (int x = 0; x < gridInfo.Width; x++)
            {
                AStarGrid[x] = new AStarCell[gridInfo.Height];
                for (int y = 0; y < gridInfo.Height; y++)
                {
                    AStarGrid[x][y] = new AStarCell(x, y);
                }
            }
        }

        public void SearchPath(Vector2 start, Vector2 end)
        {
            nextCandidates.Clear();
            pathStack.Clear();
            Path = Enumerable.Empty<Vector2>();
            PathFound = false;
            cells.ResetTouchedCells();

            sx = (int)start.x;
            sy = (int)start.y;
            ex = (int)end.x;
            ey = (int)end.y;

            cells.TouchStart(sx, sy);
            cells.TouchEnd(ex, ey);

            currentCandidate = cells.Touch(sx, sy);
            nextCandidates.Add(currentCandidate);
        }

        public void StartSearching(int maxIterations)
        {
            for (int i = 0; DiscoverNext() && i < maxIterations; i++) ;
        }

        public bool DiscoverNext()
        {
            if (!CanContinue())
            {
                return false;
            }

            currentCandidate = PullNextCandidate();

            if (ArrivedAtEnd())
            {
                FinishPathFinding(currentCandidate);
                return false;
            }

            UpdateCurrentNeighbors();
            currentCandidate.Close();
            return true;
        }

        private bool CanContinue()
        {
            return !PathFound && HaveNextCandidate();
        }

        private bool HaveNextCandidate()
        {
            var unclosedCandidates = nextCandidates.Where(c => c.State != AStarCellState.Closed);
            return unclosedCandidates.Any();
        }

        private AStarCell PullNextCandidate()
        {
            var candidate = nextCandidates
                .Where(c => c.State != AStarCellState.Closed)
                .OrderBy(c => c.FCost)
                .ThenBy(c => c.HCost)
                .First();

            nextCandidates.Remove(candidate);
            return candidate;
        }

        private bool ArrivedAtEnd()
        {
            return currentCandidate.IsEndCell;
        }

        private void FinishPathFinding(AStarCell currentCandidate)
        {
            nextCandidates.Clear();
            PathFound = true;
            BuildPathToEnd(currentCandidate);
        }

        private void BuildPathToEnd(AStarCell endPath)
        {
            var pathCell = endPath;
            while (!pathCell.IsStartCell)
            {
                pathCell.ChangeToPath();
                pathStack.Push(pathCell);
                pathCell = cells.Touch(pathCell.ParentX, pathCell.ParentY);
            }
            Path = pathStack.Select(p => new Vector2(p.XPos, p.YPos)).ToList();
        }

        private void UpdateCurrentNeighbors()
        {
            var arroundCells = cells.TouchArroundPoint(currentCandidate.XPos, currentCandidate.YPos, NoDiagonals);
            foreach (var arroundCell in arroundCells)
            {
                if (NeighborCanBeUpdated(arroundCell))
                {
                    UpdateNeighbor(arroundCell);
                }
            }
        }

        private bool NeighborCanBeUpdated(AStarCell n)
        {
            return n.State != AStarCellState.Closed && GridInfo.IsWalkable(n.XPos, n.YPos);
        }

        private void UpdateNeighbor(AStarCell n)
        {
            n.UpdateCost(currentCandidate, ex, ey);
            nextCandidates.Add(n);
        }
    }
}