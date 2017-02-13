using UnityEngine;

public static class Trigonometrics {
	// Angle counts from twelve o'clock and clock wise
	public static Vector2 GetCircunferencePoint(Vector2 center,float radius,float angleRad){
		return new Vector2(center.x + (radius * Mathf.Sin(angleRad)) ,center.y + ( radius * Mathf.Cos(angleRad)));
	}

	public static Vector2 localPointToGlobal(GameObject referent,Vector2 point) {
		
		return referent.transform.TransformPoint (point);
	}

	public static Vector2 globalPointToLocal(GameObject referent, Vector2 point) {
		return referent.transform.InverseTransformPoint(point);

	}

	public static Vector2 getPosition2D(GameObject referent){
		return new Vector2 (referent.transform.position.x, referent.transform.position.y);
	}

	public static Vector2 rotatePointFromCenter (Vector2 point , float angle){
		float newX = Mathf.Cos(angle*Mathf.Deg2Rad) * (point.x) - Mathf.Sin(angle*Mathf.Deg2Rad) * (point.y);   
		float newY = Mathf.Sin(angle*Mathf.Deg2Rad) * (point.x) + Mathf.Cos(angle*Mathf.Deg2Rad) * (point.y); 

		return new Vector2 (newX, newY);

	}

	public static Vector2 linesIntersection (Vector2 point1 ,Vector2 vector1,Vector2 point2, Vector2 vector2){
		float A1 = vector1.y;
		float B1 = -1 * vector1.x;
		float C1 = point1.x* A1 + point1.y * B1;

		float A2 = vector2.y;
		float B2 = -1 * vector2.x;
		float C2 = point2.x* A2 + point2.y * B2;

		float delta = A1 * B2 - A2 * B1;

		float x = (B2 * C1 - B1 * C2) / delta;
		float y = (A1 * C2 - A2 * C1) / delta;

		return new Vector2 (x, y);




	}
}