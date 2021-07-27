namespace generic {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class TestCollider : MonoBehaviour {

		void OnCollisionEnter2D(Collision2D other) {
			Debug.Log("Entered collider for [" + gameObject.name + "]");
		}
		
		void OnCollisionExit2D(Collision2D other) {
			Debug.Log("Exited collider for [" + gameObject.name + "]");
		}
	}
}