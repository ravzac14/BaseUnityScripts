namespace map {
	using UnityEngine;

	public class Position {

		public int x;
		public int y;
		
		public Position(int newX, int newY) {
			x = newX;
			y = newY;
		}
		
		public Vector2 toVector2(int scalar = 1) {
			return new Vector2(x * scalar, y * scalar);
		}
	}
}