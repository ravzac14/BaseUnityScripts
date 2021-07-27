namespace map {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MapPiece : MonoBehaviour {
		
		public List<Transform> warpTargets;
		public List<Transform> warpExits;
		public List<Transform> playerSpawns;
		public List<Transform> npcSpawns;
		public List<Transform> breakableSpawns;
	}
}