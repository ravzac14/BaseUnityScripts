namespace factories {
	using entities;
	using generic;
	using io;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class EntityFactory : MonoBehaviour {

		public string playersPath = "entities/player";
		public string playerTag = "Player";
		public string npcsPath = "entities/npc";
		public string npcTag = "NPC";
		public string breakablesPath = "entities/static_objects/breakable";
		public string breakableTag = "Breakable";
	
		private List<GameObject> playerPrefabList;
		private List<GameObject> npcPrefabList;
		private List<GameObject> breakablePrefabList;
	
		void Awake() {
			populateEntityLists();
		}
		
		private void populateEntityLists() {
			playerPrefabList = FileReader.loadPrefabsWithTag(playersPath, playerTag);
			npcPrefabList = FileReader.loadPrefabsWithTag(npcsPath, npcTag);
			breakablePrefabList = FileReader.loadPrefabsWithTag(breakablesPath, breakableTag);
			Debug.Assert(playerPrefabList.Count > 0, "Could not find player entities!");
			Debug.Assert(npcPrefabList.Count > 0, "Could not find npc entities!");
			Debug.Assert(breakablePrefabList.Count > 0, "Could not find breakable entities!");
		}
		
		public Entity createEntity(
			EntityType entityType, 
			Vector2 entityPos, 
			Quaternion entityRot,
			Transform parent) {
			GameObject prefabObject = null;
			switch (entityType) {
				case EntityType.Player:
					prefabObject = Helpers.randomListValue(playerPrefabList);
					break;
				
				case EntityType.NPC:
					prefabObject = Helpers.randomListValue(npcPrefabList);
					break;
					
				case EntityType.Breakable:
					prefabObject = Helpers.randomListValue(breakablePrefabList);
					break;
				
				default:
					break;
			}
			
			GameObject entityObject = (GameObject) Instantiate(prefabObject, entityPos, entityRot, parent);
			return new Entity(entityType, entityObject);
		}
	}
}