namespace entities {
  using UnityEngine;

  public enum Direction {Up, Down, Left, Right};

  public static class DirectionHelpers {

    public static Direction vectorToDirection(Vector2 v) {
      if (Mathf.Abs(v.x) >= Mathf.Abs(v.y)) {
        if (v.x >= 0f) {
          return Direction.Right;
        } else {
          return Direction.Left;
        }
      } else {
        if (v.y >= 0f) {
          return Direction.Up;
        } else {
          return Direction.Down;
        }
      }
    }
  }
}
