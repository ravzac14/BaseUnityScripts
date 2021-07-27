namespace io {
	using generic;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class FileReader {
		private static string PrefabFolderPrefix = "Prefabs/";
	
		/* The filePath is relative to the folder called Prefabs in the Assets folder */
		private static List<GameObject> loadPrefabsWithPredicate(
			string filePath, 
			Func<GameObject, Boolean> predicate) {
			List<GameObject> result = new List<GameObject>();
			UnityEngine.Object[] allFiles = Resources.LoadAll<UnityEngine.Object>(PrefabFolderPrefix + filePath);
			foreach (UnityEngine.Object obj in allFiles) {
				if (obj is GameObject) {
					GameObject gameObject = obj as GameObject;
					if (predicate(gameObject))
						result.Add(gameObject);
				}
			}
			return result;
		}
		
		public static List<GameObject> loadPrefabsWithTag(string filePath, string tag) {
			return loadPrefabsWithPredicate(
				filePath, 
				(gameObject) => { return gameObject.tag == tag; });
		}
		
		public static List<GameObject> loadPrefabsWithTags(string filePath, List<string> tags) {
			return loadPrefabsWithPredicate(
				filePath, 
				(gameObject) => { return tags.Contains(gameObject.tag); });
		}
		
		public static List<GameObject> loadPrefabsWithComponent<T>(string filePath) where T: Component {
			return loadPrefabsWithPredicate(
				filePath, 
				(gameObject) => { return gameObject.GetComponent<T>() != null; });
		}
	
		/* The filePath is relative to any folder names Resources in the Assets folder */
		public static TextAsset getTextAsset(string filePath) {
			TextAsset foundTextAsset = Resources.Load<TextAsset>(filePath);
			if (foundTextAsset == null)
				Debug.LogError("Could not load TextAsset at [" + filePath + "]");
			return foundTextAsset;
		}
	}
}