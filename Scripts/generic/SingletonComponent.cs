namespace generic {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SingletonComponent<T> : MonoBehaviour where T : Component {

		private static T instance;
		
		public static T Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					if (instance == null) {
						GameObject emptyGameObject = new GameObject();
						emptyGameObject.name = typeof(T).Name;
						instance = emptyGameObject.AddComponent<T>();
					}
				}
			return instance;
			}
		}
	
		void Awake() {
			if (instance == null) {
				instance = this as T;
				DontDestroyOnLoad(this.gameObject);
			} else {
				Destroy(gameObject);
			}
		}
	}
}