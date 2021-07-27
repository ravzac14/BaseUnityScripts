namespace factories {
	using io;
	using map;
	using MathF = UnityEngine.Mathf;
	using Random = UnityEngine.Random;
	using Stage = map.Stage;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	// TODO(zack): 
	// 		* Will I ever actually need to add to the bodyMap (graph whatever)?
	// 		* Do I need to have more "difficulty" or "level" data on each map body that I would filter on? As tags?
	//		** Or will it always just be # of map pieces?
	//		* Are entrances really just body pieces with another script that lets them be special? Would have the transform for the player start...
	//		** Therefore would need to refactor the map data here..but if it's graphs it should be simpler..ie make graph, choose starting pos randomly, then choose paths as below
	public class StageFactory : MonoBehaviour {
		
		public int difficulty;
		public string entrancesPath = "map/map_pieces/entrances";
		public string entranceTag = "MapEntrance";
		public string bodiesPath = "map/map_pieces/bodies";
		public string bodyTag = "MapBody";
		public string interiorsPath = "map/map_pieces/interiors";
		public List<string> interiorTags = new List<string> { "HouseInterior", "TentInterior" };
		
		private Dictionary<GameObject, StageBuilderHelpers.SupportedExits> entrancePrefabMap;
		private Dictionary<GameObject, StageBuilderHelpers.SupportedExits> bodyPrefabMap;
		private Dictionary<GameObject, StageBuilderHelpers.SupportedExits> interiorPrefabMap;
		
		void Awake() {
			populateMapLists();
		}
		
		private void populateMapLists() {
			List<GameObject> allMapEntrances = FileReader.loadPrefabsWithTag(entrancesPath, entranceTag);
			List<GameObject> allMapBodies = FileReader.loadPrefabsWithTag(bodiesPath, bodyTag);
			List<GameObject> allMapInteriors = FileReader.loadPrefabsWithTags(interiorsPath, interiorTags);
			entrancePrefabMap = StageBuilderHelpers.createExitMap(allMapEntrances);
			bodyPrefabMap = StageBuilderHelpers.createExitMap(allMapBodies);
			Debug.Assert(allMapEntrances.Count > 0, "Could not find map entrances!");
			Debug.Assert(allMapBodies.Count > 0, "Could not find map bodies!");
		}
		
		// TODO(zack): Flesh this out
		// GameObject entrance = StageBuilderHelpers.chooseN(1, entrancePrefabMap.Keys.ToList())[0];
		// List<GameObject> bodies = StageBuilderHelpers.chooseBodies(numPieces, bodyPrefabMap);
		// Dictionary<Position, MapPiece> pieceDictionary = StageBuilderHelpers.toMapPieceDictionary(entrance, bodies);
		// return new Stage(pieceDictionary);
		public Stage createStage(
			int numPieces,
			Transform parent) {
			Dictionary<Position, GameObject> pieceDictionary = new Dictionary<Position, GameObject>();
			// Init entrance1
			Position entrance1Position = new Position(3, 0);
			GameObject entrance1Prefab = entrancePrefabMap.Keys.ToList().Find((GameObject o) => { return o.name == "entrance1"; });
			GameObject entrance1 = (GameObject) Instantiate(entrance1Prefab, entrance1Position.toVector2(100), Quaternion.identity, parent);
			// Init exit1
			Position exit1Position = new Position(0, 0);
			GameObject exit1Prefab = bodyPrefabMap.Keys.ToList().Find((GameObject o) => { return o.name == "exit1"; });
			GameObject exit1 = (GameObject) Instantiate(exit1Prefab, exit1Position.toVector2(100), Quaternion.identity, parent);
			// Init village2
			Position village2Position = new Position(1, 0);
			GameObject village2Prefab = bodyPrefabMap.Keys.ToList().Find((GameObject o) => { return o.name == "village2"; });
			GameObject village2 = (GameObject) Instantiate(village2Prefab, village2Position.toVector2(100), Quaternion.identity, parent);
			// Init village3
			Position village3Position = new Position(2, 0);
			GameObject village3Prefab = bodyPrefabMap.Keys.ToList().Find((GameObject o) => { return o.name == "village3"; });
			GameObject village3 = (GameObject) Instantiate(village3Prefab, village3Position.toVector2(100), Quaternion.identity, parent);
			
			/* Connect all the warps */
			// entrance1 self1 -> self1
			entrance1.GetComponent<MapPiece>().warpExits[1].gameObject.GetComponent<WarpBehavior>().warpTarget = entrance1.GetComponent<MapPiece>().warpTargets[1];
			// entrance1 westExit <-> village3 eastEntrance
			entrance1.GetComponent<MapPiece>().warpExits[0].gameObject.GetComponent<WarpBehavior>().warpTarget = village3.GetComponent<MapPiece>().warpTargets[1];
			village3.GetComponent<MapPiece>().warpExits[1].gameObject.GetComponent<WarpBehavior>().warpTarget = entrance1.GetComponent<MapPiece>().warpTargets[0];
			// village3 westExit <-> village2 eastEntrance
			village3.GetComponent<MapPiece>().warpExits[0].gameObject.GetComponent<WarpBehavior>().warpTarget = village2.GetComponent<MapPiece>().warpTargets[1];
			village2.GetComponent<MapPiece>().warpExits[1].gameObject.GetComponent<WarpBehavior>().warpTarget = village3.GetComponent<MapPiece>().warpTargets[0];
			// village2 westExit <-> exit1 eastEntrance
			village2.GetComponent<MapPiece>().warpExits[0].gameObject.GetComponent<WarpBehavior>().warpTarget = exit1.GetComponent<MapPiece>().warpTargets[0];
			exit1.GetComponent<MapPiece>().warpExits[0].gameObject.GetComponent<WarpBehavior>().warpTarget = village2.GetComponent<MapPiece>().warpTargets[0];
			
			// Add to dict and return
			pieceDictionary.Add(entrance1Position, entrance1);
			pieceDictionary.Add(exit1Position, exit1);
			pieceDictionary.Add(village2Position, village2);
			pieceDictionary.Add(village3Position, village3);
			return new Stage(pieceDictionary);
		}
	}
	
	public static class StageBuilderHelpers {

		public enum SupportedExits {
			NESW,
			NES,
			NEW,
			NSW,
			ESW,
			NE,
			NS,
			NW,
			ES,
			EW,
			SW,
			N,
			E,
			S,
			W
		};

		private static SupportedExits supportedExitsForExitDirections(List<CompassDirection> directions) {
			List<CompassDirection> _nesw = new List<CompassDirection> { CompassDirection.North, CompassDirection.East, CompassDirection.South, CompassDirection.West };
			List<CompassDirection> _nes = new List<CompassDirection> { CompassDirection.North, CompassDirection.East, CompassDirection.South };
			List<CompassDirection> _new = new List<CompassDirection> { CompassDirection.North, CompassDirection.East, CompassDirection.West };
			List<CompassDirection> _nsw = new List<CompassDirection> { CompassDirection.North, CompassDirection.South, CompassDirection.West };
			List<CompassDirection> _esw = new List<CompassDirection> { CompassDirection.East, CompassDirection.South, CompassDirection.West };
			List<CompassDirection> _ne = new List<CompassDirection> { CompassDirection.North, CompassDirection.East };
			List<CompassDirection> _ns = new List<CompassDirection> { CompassDirection.North, CompassDirection.South };
			List<CompassDirection> _nw = new List<CompassDirection> { CompassDirection.North, CompassDirection.West };
			List<CompassDirection> _es = new List<CompassDirection> { CompassDirection.East, CompassDirection.South };
			List<CompassDirection> _ew = new List<CompassDirection> { CompassDirection.East, CompassDirection.West };
			List<CompassDirection> _sw = new List<CompassDirection> { CompassDirection.South, CompassDirection.West };
			List<CompassDirection> _n = new List<CompassDirection> { CompassDirection.North };
			List<CompassDirection> _e = new List<CompassDirection> { CompassDirection.East };
			List<CompassDirection> _s = new List<CompassDirection> { CompassDirection.South };
			List<CompassDirection> _w = new List<CompassDirection> { CompassDirection.West };
			
			if (directions.Count == _nesw.Count && directions.All(_nesw.Contains)) return SupportedExits.NESW;
			if (directions.Count == _nes.Count && directions.All(_nes.Contains)) return SupportedExits.NES;
			if (directions.Count == _new.Count && directions.All(_new.Contains)) return SupportedExits.NEW;
			if (directions.Count == _nsw.Count && directions.All(_nsw.Contains)) return SupportedExits.NSW;
			if (directions.Count == _esw.Count && directions.All(_esw.Contains)) return SupportedExits.ESW;
			if (directions.Count == _ne.Count && directions.All(_ne.Contains)) return SupportedExits.NE;
			if (directions.Count == _ns.Count && directions.All(_ns.Contains)) return SupportedExits.NS;
			if (directions.Count == _nw.Count && directions.All(_nw.Contains)) return SupportedExits.NW;
			if (directions.Count == _es.Count && directions.All(_es.Contains)) return SupportedExits.ES;
			if (directions.Count == _ew.Count && directions.All(_ew.Contains)) return SupportedExits.EW;
			if (directions.Count == _sw.Count && directions.All(_sw.Contains)) return SupportedExits.SW;
			if (directions.Count == _n.Count && directions.All(_n.Contains)) return SupportedExits.N;
			if (directions.Count == _e.Count && directions.All(_e.Contains)) return SupportedExits.E;
			if (directions.Count == _s.Count && directions.All(_s.Contains)) return SupportedExits.S;
			if (directions.Count == _w.Count && directions.All(_w.Contains)) return SupportedExits.W;
			throw new Exception("StageBuilderHelpers#supportedExitsForExitDirections: Unsupported directions list");
		}

		public static List<CompassDirection> directionsForMapPiece(GameObject mapPiece) {
			List<CompassDirection> directionList = new List<CompassDirection>();
			foreach (Transform warpExit in mapPiece.transform.Find("WarpExits")) {
				if (warpExit.gameObject.GetComponent<WarpBehavior>().objectType == WarpBehaviorObjectType.MapPieceExit) {
					// If it's the warpExit type we care about then add warp direction to directionList
					directionList.Add(warpExit.gameObject.GetComponent<WarpBehavior>().direction);
				}
			}
			return directionList;
		}

		public static void addToMapPieceMap(
			GameObject newMapPiece, 
			Dictionary<GameObject, SupportedExits> mapPieceMap) {
			mapPieceMap.Add(newMapPiece, supportedExitsForExitDirections(directionsForMapPiece(newMapPiece)));
		}

		public static Dictionary<GameObject, SupportedExits> createExitMap(List<GameObject> allMapBodies) {
			Dictionary<GameObject, SupportedExits> result = new Dictionary<GameObject, SupportedExits>();
			foreach (GameObject body in allMapBodies){
				result.Add(body, supportedExitsForExitDirections(directionsForMapPiece(body)));
			}
			return result;
		}

		public static List<GameObject> chooseN(int n, List<GameObject> allOptions) {
			int optionLength = allOptions.Count;
			int lastIndex = optionLength - 1;
			List<GameObject> result = new List<GameObject>();
			if (optionLength > 0) {
				for (int i = 0; i < n; i++) {
					result.Add(allOptions[(int)Mathf.Round(Random.Range(0f, (float)lastIndex))]);
				}
			}
			return result;
		}
		
		private static List<SupportedExits> supportedExitsForEntranceDirection(CompassDirection d) {
			switch (d) {
				case CompassDirection.North:
					return new List<SupportedExits> { 
						SupportedExits.NESW,
						SupportedExits.NES,
						SupportedExits.NEW,
						SupportedExits.NSW,
						SupportedExits.NE,
						SupportedExits.NS,
						SupportedExits.NW,
						SupportedExits.N
					};
				
				case CompassDirection.East:
					return new List<SupportedExits> {
						SupportedExits.NESW,
						SupportedExits.NES,
						SupportedExits.NEW,
						SupportedExits.ESW,
						SupportedExits.NE,
						SupportedExits.ES,
						SupportedExits.EW,
						SupportedExits.E
					};
					
				case CompassDirection.South:
					return new List<SupportedExits> {
						SupportedExits.NESW,
						SupportedExits.NES,
						SupportedExits.NSW,
						SupportedExits.ESW,
						SupportedExits.NS,
						SupportedExits.ES,
						SupportedExits.SW,
						SupportedExits.S
					};
					
				case CompassDirection.West:
					return new List<SupportedExits> {
						SupportedExits.NESW,
						SupportedExits.NEW,
						SupportedExits.NSW,
						SupportedExits.ESW,
						SupportedExits.NW,
						SupportedExits.EW,
						SupportedExits.SW,
						SupportedExits.W
					};
					
				default:
					throw new Exception("StageBuilderHelpers#supportedExitsForExitDirection: Unsupported CompassDirection!");
			}
		}

		public static List<GameObject> chooseBodies(
			int numPieces,
			CompassDirection entranceDirection,
			Dictionary<GameObject, StageBuilderHelpers.SupportedExits> bodyMap) {
			List<GameObject> result = new List<GameObject>();
			// Need to build a tree of paths, where each node is a new body and the root is the entrance. Then choose a random one of the correct length (or take up to the correct length of a longer one)
			// ^ This doesnt work bc of how duplicate paths would behave
			// WHAT ACTUALLY NEEDS TO GET DONE:
			// 1) Need to build a graph, where edges are the directions that two bodies can connect on. 
			// 2) Get all paths that have <s>length</s> the size of the set of nodes == to numPieces and where the path:
			//		a) starts with the direction (start edge) given
			//		b) It never returns back to a node more times than it has exits
			//		c) Never duplicates a dead ended path fully
			// 3) Choose randomly from these paths
			// ... somewhere else ...
			// 4) Connect the bodies warpBeh's -> warpTargets
			// 5) Place in MapPos dictionary
			// ... somewhere further else ...
			// 6) init the Stage to scene, set player, set items, set npcs, etc
			
			//for (int = 0; i < numPieces; i++) {	
			//}
			return result;
		}
	}
}