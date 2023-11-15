using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField]
  private PlayerControllerServices services;

  protected virtual void Start()
  {
    // Build the path and sample points along it.
    var path = services.PathBuilder.Build();
    var points = new List<Vector3>();
    for (var i = 0; i <= 200; i++)
    {
      var t = i / 200f;
      points.Add(path.Sample(t));
    }

    // Make a direct equidistant path from the built path and provide that to the guide.
    services.ForceTowardsGuide.TargetTransform.GetComponent<MoveAlongPath>().Path = new DirectPath(MathUtils.SpreadEqually(points)); ;
  }

  protected virtual void Update()
  {
    var cameraDistance = Mathf.Abs(transform.position.z - services.Camera.transform.position.z);

    var cursorPosition = services.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
    cursorPosition.z = transform.position.z;

    services.ForceTowardsCursor.TargetPosition = cursorPosition;
  }
}

[Serializable]
public struct PlayerControllerServices
{
  public Camera Camera;
  public ForceTowardsTarget ForceTowardsCursor;
  public ForceTowardsTarget ForceTowardsGuide;
  public BuilderBehaviour<IPath> PathBuilder;
}
