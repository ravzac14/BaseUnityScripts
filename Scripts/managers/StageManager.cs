namespace managers {
	using generic;
	using map;
	using Stage = map.Stage;
	using StageFactory = factories.StageFactory;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class StageManager : SingletonComponent<StageManager> {

		GameManager gameManager;
	
		// TODO(zack): eventually get these from config per difficulty
		public int numMapPieces = 3;
	
		private StageFactory stageFactory;
		private Stage currentStage;		
		
		void Awake() {
			gameManager = GetComponent<GameManager>();
			initStageManager();
		}
		
		void Start() {
		}

		void Update() {
		}
		
		void initStageManager() {
			gameObject.AddComponent<StageFactory>();
			stageFactory = GetComponent<StageFactory>();
			stageFactory.difficulty = gameManager.difficulty;
		}
		
		public void setupStage() {
			if (stageFactory == null) {
				initStageManager();
			}
			currentStage = stageFactory.createStage(numMapPieces, gameObject.transform);
		}
		
		public Vector2 generatePlayerSpawnPoint() {
			return Helpers.randomListValue(currentStage.playerSpawns()).position;
		}
		
		public List<Vector2> generateNpcSpawnPoints(int numNpcs) {
			List<Vector2> result = new List<Vector2>();
			List<Transform> spawnsToChooseFrom = currentStage.npcSpawns();
			if (numNpcs > 0) {
				for (int i = 1; i <= numNpcs; i++) {
					if (spawnsToChooseFrom.Count > 0) {
						Transform choice = Helpers.randomListValue(spawnsToChooseFrom);
						result.Add(choice.position);
						spawnsToChooseFrom.Remove(choice);
					}
				}
			}
			return result;
		}
		
		public List<Vector2> generateBreakableSpawnPoints(int numBreakables) {
			List<Vector2> result = new List<Vector2>();
			List<Transform> spawnsToChooseFrom = currentStage.breakableSpawns();
			if (numBreakables > 0) {
				for (int i = 1; i <= numBreakables; i++) {
					if (spawnsToChooseFrom.Count > 0) {
						Transform choice = Helpers.randomListValue(spawnsToChooseFrom);
						result.Add(choice.position);
						spawnsToChooseFrom.Remove(choice);
					}
				}
			}
			return result;
		}
	}
}