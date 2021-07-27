namespace hud {
	using entities;
	using generic;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class EventLog : SingletonComponent<EventLog> {

    Queue<string> logQueue = new Queue<string>();
    string text = "";

    public int maxLines = 10;
    public int textAreaHeight = 300;
    public int textAreaWidth = 150;

    void OnGUI() {
		GUI.Label(
			new Rect(
				Screen.width - textAreaWidth,
				0f, 
				textAreaWidth, 
				textAreaHeight), 
			text, 
			GUI.skin.textArea);
    }

    private void addEvent(
		string eventString, 
		int alertLevel, 
		bool hideFromHUD = false, 
		bool hideFromConsole = false) {
		if (!hideFromConsole) {
			if (alertLevel == 3)
				print(eventString);
			else if (alertLevel == 2)
				Debug.LogWarning(eventString);
			else if (alertLevel == 1)
				Debug.LogError(eventString);
		}
		
		if (!hideFromHUD) {
			if (logQueue.Count >= maxLines) logQueue.Dequeue();
			logQueue.Enqueue(eventString);

			text = "";
			foreach (string logEvent in logQueue)
				text += (logEvent + "\n");
		}
    }

	/* General logs */
	public void log(string message) {
		this.addEvent(message, 3);
	}
	
    public void logWarning(string message) {
		this.addEvent(message, 2);
    }
	
	public void logError(string message) {
		this.addEvent(message, 1);
	}
	
	/* Entity logs */
    public void logAttack(
        string attackerName, 
        string targetName, 
        string animationName) {
		log("Entity [" + attackerName + "] attacked [" + targetName + "] with animation [" + animationName + "]");
    }

    public void logMovement(
        string name,
        Direction direction) {
		log("Entity [" + name + "] moved [" + direction + "]");
    }

	/* Warp logs */
	public void logWarpStart(
		string warpee, 
		string from,
		string to) {
		log("Warping object [" + warpee + "] from [" + from + "] to [" + to + "]");
	}
	
	public void logWarpEnd(string warpee) {
		log("Done warping object [" + warpee + "]");
	}
  }
}