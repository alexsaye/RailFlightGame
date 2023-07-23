using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Camera cam;

  [SerializeField]
  private RigidbodyMoveToTarget moveToCursor;

  private void Start()
  {
    this.cam ??= Camera.main;
  }

  private void Update()
  {
    var cameraDistance = Mathf.Abs(this.transform.position.z - this.cam.transform.position.z);

    var cursorPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
    cursorPosition.z = this.transform.position.z;

    this.moveToCursor.Target = cursorPosition;
  }
}
