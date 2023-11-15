using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MathUtils : UnityEditor.MathUtils
{
    /// <summary>
    /// Takes points and returns an equivalent set of points where each is equidistant from the next,
    /// roughly preserving the shape of the original path and making sure the first and last points are unchanged.
    /// </summary>
    public static IEnumerable<Vector3> SpreadEqually(IEnumerable<Vector3> points)
    {
        var equidistantPoints = points.ToArray();
        if (equidistantPoints.Length < 3)
        {
            return equidistantPoints;
        }

        // Find the average distance between points.
        var totalDistance = 0f;
        for (var i = 1; i < equidistantPoints.Length; i++)
        {
            totalDistance += Vector3.Distance(equidistantPoints[i - 1], equidistantPoints[i]);
        }
        var averageDistance = totalDistance / (equidistantPoints.Length - 1);

        // Start at the second point to make sure the first point is unchanged.
        var index = 1;

        // If there are an even number of points, cheat by moving the second point away from the first point through its current vector.
        if (equidistantPoints.Length % 2 == 0)
        {
            Debug.Log("Even number of points, cheating by moving the second point away from the first point through its current vector.");
            equidistantPoints[1] = equidistantPoints[0] + (equidistantPoints[1] - equidistantPoints[0]).normalized * averageDistance;
            ++index;
        }

        // For each other point, find the vectors to the previous and next points and move the point along the average of those vectors until it is the average distance from both points.
        while (index < equidistantPoints.Length - 1)
        {
            var previous = equidistantPoints[index - 1];
            var current = equidistantPoints[index];
            var next = equidistantPoints[index + 1];
            var previousVector = current - previous;
            var nextVector = next - current;

            // Move the current point along the normal of those vectors until it is the average distance from both points.
            var normal = Vector3.Cross(previousVector, nextVector).normalized;
            equidistantPoints[index] = current + normal * (averageDistance - Mathf.Max(Vector3.Distance(previous, current), Vector3.Distance(current, next)));

            index += 2;
        }

        return equidistantPoints;
    }
}
