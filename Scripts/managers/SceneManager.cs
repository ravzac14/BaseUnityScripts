namespace managers {
	using camera;
	using entities;
	using generic;
	using hud;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SceneManager : SingletonComponent<SceneManager> {
	
		public int numBreakables;
		public int numEnemies;
		public int numNeutrals;
	
		private Vector2 playerSpawnPoint;
		private List<Vector2> npcSpawnPoints;
		private List<Vector2> breakableSpawnPoints;
	
		GameManager gameManager;
    PlayerInputManager inputManager;
		StageManager stageManager;
		EntityManager entityManager;
		EventLog eventLog;
		GameObject mainCamera;
		
		void Awake() {
			gameManager = GetComponent<GameManager>();
      inputManager = GetComponent<PlayerInputManager>();
			stageManager = GetComponent<StageManager>();
			entityManager = GetComponent<EntityManager>();
			eventLog = GetComponent<EventLog>();
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
		
		void Start() {}

		void Update() {}
				
		public void setupScene() {
			eventLog.log("Initializing stage...");
			stageManager.setupStage();
			playerSpawnPoint = stageManager.generatePlayerSpawnPoint();
			// TODO(zack): Split up enemies and neutrals eventually
			npcSpawnPoints = stageManager.generateNpcSpawnPoints(numEnemies + numNeutrals);
			breakableSpawnPoints = stageManager.generateBreakableSpawnPoints(numBreakables);
			
			eventLog.log("Initializing breakables...");
			foreach(Vector2 pos in breakableSpawnPoints) {
				entityManager.createEntity(EntityType.Breakable, pos, Quaternion.identity);
			}
			
			eventLog.log("Initializing npcs...");
			foreach(Vector2 pos in npcSpawnPoints) {
				entityManager.createEntity(EntityType.NPC, pos, Quaternion.identity);
			}
			
			eventLog.log("Initializing player...");
			Entity playerEntity = entityManager.createEntity(EntityType.Player, playerSpawnPoint, Quaternion.identity);
			mainCamera.GetComponent<CameraFollow>().target = playerEntity.gameObject.transform;
			inputManager.setInputMode(EnabledFull);

			eventLog.log("Done initializing scene");
		}
	}
}
