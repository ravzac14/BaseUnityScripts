namespace map {
	using entities;
	using hud;
	using managers;
	using ScreenFader = hud.ScreenFader;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class WarpBehavior : MonoBehaviour {

		EventLog eventLog;
		ScreenFader screenFader;
		GameManager gameManager;
	
		public Transform warpTarget;

		public WarpBehaviorObjectType objectType;
		public CompassDirection direction;
		
		void Awake() {
			screenFader = GameObject.FindGameObjectWithTag("fader").GetComponent<ScreenFader>();
			eventLog = GetComponentInParent<EventLog>();
			gameManager = GetComponentInParent<GameManager>();
		}
	
		// OnTrigger set Player/Camera to target
		IEnumerator OnTriggerEnter2D(Collider2D collision) {
			GameObject entityParent = EntityHelpers.findEntityParent(collision);
			eventLog.logWarpStart(entityParent.name, name, warpTarget.name);
			gameManager.isInputEnabled = false;
			yield return StartCoroutine(screenFader.FadeToBlack());

			entityParent.transform.position = warpTarget.position;
			Camera.main.transform.position = warpTarget.position;

			yield return StartCoroutine(screenFader.FadeToClear());
			gameManager.isInputEnabled = true ;
			eventLog.logWarpEnd(entityParent.name);
		}
	}
}
