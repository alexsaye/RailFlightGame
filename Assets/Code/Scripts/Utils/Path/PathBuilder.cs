using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A behaviour which builds a path from a collection of nodes which represent point trees.
/// </summary>
public abstract class PathBuilder : BuilderBehaviour<IPath>
{
  [Header("Guide")]

  [Tooltip("The nodes to use as control points.")]
  public List<Transform> Nodes;

  /// <summary>
  /// The points which make up the path, derived from the nodes.
  /// </summary>
  public IEnumerable<Vector3> Points => DerivePoints(Nodes ?? new List<Transform> { transform });

  /// <summary>
  /// Recursively reduce a collection of transforms into a collection of positions, treating each transform as a tree and returning the positions of its leaves.
  /// </summary>
  private static IEnumerable<Vector3> DerivePoints(IEnumerable<Transform> transform)
  {
    return transform.SelectMany(t => t.childCount > 0 ? DerivePoints(t.Cast<Transform>()) : new[] { t.position });
  }

  protected virtual void OnDrawGizmos()
  {
    Gizmos.color = Color.white;

    // If the selection doesn't contain this transform or any of its nodes or their children, dim the gizmos.
    if (!(UnityEditor.Selection.Contains(gameObject) || Nodes != null && Nodes.Any(n => n.GetComponentsInChildren<Transform>().Any(t => UnityEditor.Selection.Contains(t.gameObject)))))
    {
      Gizmos.color *= 0.75f;
    }

    var path = Build();
    var samples = Points.Count() * 100;
    var previous = path.Sample(0f);
    for (var i = 1; i <= samples; i++)
    {
      var t = (float)i / samples;
      var current = path.Sample(t);
      Gizmos.DrawLine(previous, current);
      previous = current;
    }
  }
}
