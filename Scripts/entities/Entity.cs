namespace entities {
	using entities.gear;
	using generic;
	using System;
	using System.Collections.Generic;
	using UnityEngine;
	
	public enum EntityType { Unknown, Player, NPC, Breakable }

	[Serializable]
	public class Entity {

		public EntityType entityType;
		public GameObject gameObject;
		
		public Entity() {
			entityType = EntityType.Unknown;
			gameObject = new GameObject();
		}
		
		public Entity(EntityType newEntityType, GameObject newGameObject) {
			entityType = newEntityType;
			gameObject = newGameObject;
		}
	}
}
