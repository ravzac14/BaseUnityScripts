namespace map {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	
	// TODO(zack): Add to these when you have a better idea of what goes in these
	[Serializable]
	public class Stage {

		// Values are key'd by it's X,Y position on a grid
		public Dictionary<Position, GameObject> mapPieceDict;
		public GameObject entrance;
		
		public Stage() {
			mapPieceDict = new Dictionary<Position, GameObject>();
		}
		
		public Stage(Dictionary<Position, GameObject> newMapPieceDict) {
			mapPieceDict = newMapPieceDict;
			entrance = mapPieceDict.Values.First<GameObject>((GameObject o) => { return o.tag == "MapEntrance"; });
		}
		
		public List<GameObject> mapPieces() {
			return mapPieceDict.Values.ToList();
		}
		
		public List<Transform> playerSpawns() {
			return entrance.GetComponent<MapPiece>().playerSpawns;
		}
		
		public List<Transform> npcSpawns() {
			List<Transform> result = new List<Transform>();
			foreach(GameObject stagePiece in mapPieces()) {
				foreach(Transform spawn in stagePiece.GetComponent<MapPiece>().npcSpawns) {
					result.Add(spawn);
				}
			}
			return result;
		}
				
		public List<Transform> breakableSpawns() {
			List<Transform> result = new List<Transform>();
			foreach(GameObject stagePiece in mapPieces()) {
				foreach(Transform spawn in stagePiece.GetComponent<MapPiece>().breakableSpawns) {
					result.Add(spawn);
				}
			}
			return result;
		}
	}
}