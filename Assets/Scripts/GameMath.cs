using UnityEngine;
using System.Collections;

public class GameMath : MonoBehaviour {

	public static Vector2 IntersectionTwoLines(Vector2 startPoint1, Vector2 endPoint1, Vector2 startPoint2, Vector2 endPoint2){
		float a1 = endPoint1.y - startPoint1.y;
		float b1 = startPoint1.x - endPoint1.x;
		float c1 = startPoint1.x * a1 + startPoint1.y * b1;

		float a2 = endPoint2.y - startPoint2.y;
		float b2 = startPoint2.x - endPoint2.x;
		float c2 = startPoint2.x * a2 + startPoint2.y * b2;

		float x = 0;
		float y = 0;

		float det = a1 * b2 - a2 * b1;
		if (det == 0) {
		} else {
			x = (b2 * c1 - b1 * c2) / det;
			y = (a1 * c2 - a2 * c1) / det;
		}
		return new Vector2 (x, y);
	}
}
