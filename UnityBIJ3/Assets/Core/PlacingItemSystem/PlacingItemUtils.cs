using System.Collections.Generic;
using UnityEngine;

public class PlacingItemUtils
{
    public PlacingItemUtils(float hexHeightPerIndex, float hexWidth)
    {
        HexHeightPerIndex = hexHeightPerIndex;
        HexWidth = hexWidth;
    }

    public float HexHeightPerIndex { get; set; }
    public float HexWidth { get; set; }

    public SnapPosition FindClosestSnapPosition(Vector3 point)
    {
        var approx = GetApproximativeXZIndex(point);

        float closedDistance = float.MaxValue;
        SnapPosition closestAnchor = new SnapPosition();
        foreach (var anchor in GetNeerbySnapPositions(approx))
        {
            var distance = (anchor.Position - point).magnitude;
            if (distance < closedDistance)
            {
                closedDistance = distance;
                closestAnchor = anchor;
            }
        }

        return closestAnchor;
    }

    public ApproximatedXZIndex GetApproximativeXZIndex(Vector3 point)
    {
        var approxZIndex = (int)(point.z / HexHeightPerIndex);

        var xOff = ((approxZIndex - 1) % 2) * (HexWidth / 2);
        var approxXIndex = (int)((point.x - xOff) / HexWidth);

        return new ApproximatedXZIndex(approxXIndex, approxZIndex);
    }

    public IEnumerable<SnapPosition> GetNeerbySnapPositions(ApproximatedXZIndex approximated)
    {
        for (int xInd = approximated.XIndex - 1; xInd <= approximated.XIndex + 1; xInd++)
        {
            for (int zInd = approximated.ZIndex - 1; zInd <= approximated.ZIndex + 1; zInd++)
            {
                var zPos = zInd * HexHeightPerIndex;

                var xOff = (zInd % 2) * HexWidth / 2;
                var xPos = xInd * HexWidth + xOff;

                yield return new SnapPosition(xInd, zInd, new Vector3(xPos, 0, zPos));
            }
        }
    }
}
