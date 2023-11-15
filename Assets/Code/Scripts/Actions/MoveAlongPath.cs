using System;
using UnityEngine;

/// <summary>
/// A behaviour which moves along a spline.
/// </summary>
[ExecuteAlways]
public class MoveAlongPath : MonoBehaviour
{
  [SerializeField]
  private MoveAlongPathServices services;

  /// <summary>
  /// The path to move along.
  /// </summary>
  public IPath Path;

  /// <summary>
  /// The progress along the path, where 0 is the start and 1 is the end.
  /// </summary>
  [Tooltip("The progress along the path, where 0 is the start and 1 is the end.")]
  [Range(0f, 1f)]
  public float Progress = 0f;

  /// <summary>
  /// The speed at which to move along the path.
  /// </summary>
  [Tooltip("The rate at which to move along the path, where 0 is stationary, 1 will complete the path in one second, 2 will complete the path in half a second, etc.")]
  public float Rate = 0.1f;

  protected virtual void Start()
  {
    Path = services.PathBuilder?.Build();
  }

  protected virtual void Update()
  {
    if (Application.isPlaying)
    {
      Progress += Rate * Time.deltaTime;

      if (Path.Closed)
      {
        Progress = Mathf.Repeat(Progress, 1f);
      }
      else
      {
        Progress = Mathf.Clamp01(Progress);
      }
    }

    if (Path != null)
    {
      transform.position = Path.Sample(Progress);
    }
  }

  protected virtual void OnDrawGizmos()
  {
    if (Path == null)
    {
      return;
    }

    Gizmos.color = Color.white;

    for (var i = 0; i <= 100; i++)
    {
      Gizmos.DrawSphere(Path.Sample(i / 100f), 0.2f);
    }
  }

  protected virtual void OnValidate()
  {
    Start();
  }
}

[Serializable]
public struct MoveAlongPathServices
{
  [Tooltip("Builds the path to move along.")]
  [SerializeField]
  public BuilderBehaviour<IPath> PathBuilder;
}
