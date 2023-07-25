using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Controls a spline.
/// </summary>
[ExecuteInEditMode]
public class SplineController : MonoBehaviour, ISpline
{
  public Spline Spline;

  public SplineTuning Tuning = new SplineTuning() { Tension = 0, Alpha = 0.5f };

  public bool Closed = false;

  private void Start()
  {
    this.Recalculate();
  }

#if UNITY_EDITOR
  private void Update()
  {
    this.Recalculate();
  }
#endif

  private void OnValidate()
  {
    this.Recalculate();
  }

  private void OnDrawGizmos()
  {
    var previous = this.Sample(0f);
    for (var i = 1; i <= 100; i++)
    {
      var t = i / 100f;
      var current = this.Sample(t);
      Gizmos.DrawLine(previous, current);
      previous = current;
    }
  }

  public void Recalculate()
  {
    this.Spline = ScriptableObject.CreateInstance<Spline>();
    this.Spline.Fit(this.transform.Cast<Transform>().Select(child => child.position), this.Tuning, this.Closed);
  }

  public Vector3 Sample(float t)
  {
    return this.Spline.Sample(t);
  }
}