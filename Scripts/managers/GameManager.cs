namespace managers {
	using generic;
	using hud;
	using io;
	using Random = UnityEngine.Random;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// NOTE(zack): Any Singletons (including this) should be added to the GameManager prefab
	public class GameManager : SingletonComponent<GameManager> {

		SceneManager sceneManager;
    PlayerInputManager inputManager;
		EventLog eventLog;
	
		/* Related to seed'ing the Random */
		public string wordListFileName = "word_list";
		public string overrideSeed;
		/* Related to setting difficulty*/
		public int difficulty = 1;
	
		void Awake() {
			sceneManager = GetComponent<SceneManager>();
      inputManager = GetComponent<PlayerInputManager>();
			eventLog = GetComponent<EventLog>();
      inputManager.setInputMode(Disabled);
		}
		
		void Start() {
			initGame();
		}
	
		void Update() {
		}
		
		void OnApplicationPause () {
		}
		
		void OnApplicationQuit () {
		}
		
		string randomWord() {
			TextAsset textAsset = FileReader.getTextAsset(wordListFileName);
			string[] wordList = textAsset.text.Split("\n"[0]);
			return wordList[Random.Range(0, wordList.Length)];
		}
		
		void setRandomSeed(string newSeed) {
			int hashedSeed = Helpers.hashUtf8Seed(newSeed);
			Random.InitState(hashedSeed);
			eventLog.log("Updating Random.seed [" + newSeed + "], hashed as [" + hashedSeed + "]");
		}
		
		void initGame() {
			eventLog.log("Initializing Game...");
			/* Set Random's seed randomly or based on user's input */
			eventLog.log("Setting initial Random Seed...");
			string seed;
			if (!string.IsNullOrEmpty(overrideSeed))
				seed = overrideSeed;
			else
				seed = randomWord();
			setRandomSeed(seed);

			/* Init scene through scene manager*/
			eventLog.log("Initializing scene...");
			sceneManager.setupScene();
		}
	}
}
