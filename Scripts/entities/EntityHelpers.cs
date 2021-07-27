namespace entities {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class EntityHelpers {
	
		private static HashSet<string> entityTags = new HashSet<string> { "Player", "NPC", "Breakable" };
	
		private static bool isEntity(GameObject o) {
			return (o != null) && (entityTags.Contains(o.tag));
		}
		
		private static bool isRoot(GameObject o) {
			return (o != null) && (o.transform.root == o.transform);
		}
		
		public static GameObject findEntityParent(GameObject gameObject) {
			GameObject entityObject = null;
			GameObject currentObject = gameObject;
			
			while (!isRoot(currentObject) && !isEntity(currentObject)) {
				currentObject = currentObject.transform.parent.gameObject;
			}

			if (isEntity(currentObject)) {
				entityObject = currentObject;
			}
			return entityObject;
		}
	
		public static GameObject findEntityParent(Component component) {
			return findEntityParent(component.gameObject);
		}
		
		public static GameObject findEntityParent(Transform transform) {
			return findEntityParent(transform.gameObject);
		}
	}
}