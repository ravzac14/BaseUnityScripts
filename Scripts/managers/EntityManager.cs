namespace managers {
	using entities;
	using generic;
	using hud;
	using EntityFactory = factories.EntityFactory;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class EntityManager : SingletonComponent<EntityManager> {
		
		private EntityFactory entityFactory;
		private List<Entity> entities;
		
		GameManager gameManager;
		EventLog eventLog;

		void Awake() {
			gameManager = GetComponent<GameManager>();
			eventLog = GetComponent<EventLog>();
			initEntityManager();
			entities = new List<Entity>();
		}

		void Start() {
		}

		void Update() {
		}
		
		void initEntityManager() {
			gameObject.AddComponent<EntityFactory>();
			entityFactory = GetComponent<EntityFactory>();
		}

		public Entity createEntity(
			EntityType entityType, 
			Vector2 entityPos, 
			Quaternion entityRot) {
			if (entityFactory == null) {
				initEntityManager();
			}

			Entity newEntity = entityFactory.createEntity(entityType, entityPos, entityRot, gameObject.transform);
			newEntity.gameObject.name = entityType.ToString() + (count(entityType) + 1);
			entities.Add(newEntity);
			return newEntity;
		}
		
		public Entity getFirstEntity(EntityType entityType) {
			return entities.Find((Entity e) => { return e.entityType == entityType; });
		}
		
		public List<Entity> getAllEntities(EntityType entityType) {
			return entities.FindAll((Entity e) => { return e.entityType == entityType; });
		}
		
		public int count(EntityType entityType) {
			return getAllEntities(entityType).Count;
		}
	}
}