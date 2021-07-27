namespace camera {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

  /** A behavior for 2d cameras to follow an element w/ a transform around the scrren gracefully
   *
   *  @target - the element to follow
   *  @moveSpeed - how fast to recenter on target (default: 0.1f)
   *  @zoomOut - how high in Z to keep camera from target (default: 10f)
   * */
	public class 2DCameraFollow : MonoBehaviour {

		public Transform target;
		public float moveSpeed = 0.1f;
		public float zoomOut = -10f;
	
		void Update () {
			if (target) {
				Vector3 cameraDefaultZ = new Vector3(0, 0, zoomOut);
				transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed) + cameraDefaultZ;
			}
		}
	}
}
