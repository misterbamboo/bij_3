using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    public interface IAStarGridInfo
    {
        int Width { get; }
        int Height { get; }

        bool IsWalkable(int x, int y);
    }
}
