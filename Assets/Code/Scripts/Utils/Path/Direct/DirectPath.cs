using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A direct path of joined line segments.
/// </summary>
public class DirectPath : IPath
{
    private Vector3[] points;

    public float Length { get; private set; }

    public bool Closed { get; private set; }

    public DirectPath(IEnumerable<Vector3> points)
    {
        this.points = points.ToArray();

        Length = 0f;
        for (var i = 1; i < this.points.Length; i++)
        {
            Length += Vector3.Distance(this.points[i - 1], this.points[i]);
        }

        Closed = this.points.Length == 0 || this.points.First().Equals(this.points.Last());
    }

    public Vector3 Sample(float progress)
    {
        var distance = Mathf.Clamp01(progress) * Length;
        var currentDistance = 0f;
        for (var i = 1; i < points.Length; i++)
        {
            var previous = points[i - 1];
            var current = points[i];
            var segmentLength = Vector3.Distance(previous, current);
            if (currentDistance + segmentLength >= distance)
            {
                return Vector3.Lerp(previous, current, (distance - currentDistance) / segmentLength);
            }
            currentDistance += segmentLength;
        }
        return points.Last();
    }
}
