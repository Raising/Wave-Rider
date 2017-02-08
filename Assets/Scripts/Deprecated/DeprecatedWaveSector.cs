using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeprecatedWaveSector : MonoBehaviour {

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/*
	//deprecated
	private void ShrinkSector(Vector2 collisionPoint){
		Vector2[] newEdgePoints = getPreShrinkPoints (collisionPoint);

		refactorSectorToNewEdges (newEdgePoints[0],newEdgePoints[1]);
	}
	//deprecated
	private  Vector2 collisionAtEdges(Vector2 collisionPoint){
		if ((collisionPoint - Trigonometrics.localPointToGlobal(gameObject,pointA)).magnitude <= diferenceFactorToSplitOrBounce) {
			return pointA;
		}else if((collisionPoint - Trigonometrics.localPointToGlobal(gameObject,pointB)).magnitude <= diferenceFactorToSplitOrBounce){
			return pointB;
		}else{
			return Vector2.zero;
		}
	}
	//deprecated
	private Vector2[] getPreShrinkPoints(Vector2 collisionPoint){
		Vector2[] newPointsAB = new Vector2[2];
		if (collisionPoint == pointA) {
			newPointsAB[0] =  Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,( minimumCollisionAngle / circunferenceRadius ) - pointsDispersionAngle);
			newPointsAB[1] = pointB;
		}
		else {
			newPointsAB[0] = pointA;
			newPointsAB[1] = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius, pointsDispersionAngle -  ( minimumCollisionAngle / circunferenceRadius ) );
		}
		return newPointsAB;
	}
	//deprecated
	private GameObject createWaveSector(Vector2 newPointA,Vector2 newPointB) {
		Vector2 newCenter = new Vector2 ((newPointA.x + newPointB.x) / 2, (newPointA.y + newPointB.y) / 2);

		Vector2 globalCenter = Trigonometrics.localPointToGlobal(gameObject,newCenter);
		Vector3 newTransform = new Vector3 (globalCenter.x ,globalCenter.y, 0);

		float angleFromParent = Vector2.Angle(newCenter - circunferenceCenter , Vector2.up  );
		Vector3 cross = Vector3.Cross(newCenter - circunferenceCenter, Vector2.up);
		if (cross.z > 0) {
			angleFromParent = -1 * angleFromParent;
		}


		float globalAngle = angleFromParent + transform.rotation.eulerAngles.z; 
		float angleFromSelf = Mathf.Deg2Rad * Vector2.Angle(newPointA - circunferenceCenter , newCenter - circunferenceCenter  );


		GameObject newWaveSector = Instantiate(waveSectorPrefab,newTransform,  Quaternion.Euler(0, 0, globalAngle));

		WaveSector waveSectorInterface = newWaveSector.GetComponent<WaveSector> ();
		waveSectorInterface.constructor ((newPointA-newPointB).magnitude,angleFromSelf,traslationSpeed,remainingAliveTime);
		return newWaveSector;
	}

	//deprecated
	private void SplitSector (Vector2 point, GameObject obstacle){
		Vector2 localPoint = Trigonometrics.globalPointToLocal (gameObject,point);
		Debug.DrawLine (point,Trigonometrics.localPointToGlobal(gameObject,circunferenceCenter));
		//Debug.Break();
		GameObject waveSector1 = createWaveSector (pointA,localPoint);
		GameObject waveSector2 = createWaveSector (localPoint,pointB);

		Physics2D.IgnoreCollision (waveSector1.GetComponent<EdgeCollider2D>(),obstacle.GetComponent<PolygonCollider2D>());		
		Physics2D.IgnoreCollision (waveSector2.GetComponent<EdgeCollider2D>(),obstacle.GetComponent<PolygonCollider2D>());		

		Destroy(gameObject);
	}


	//deprecated
	private void refactorSectorToNewEdges(Vector2 newPointA,Vector2 newPointB){
		Debug.DrawLine (Trigonometrics.localPointToGlobal(gameObject,newPointA),Trigonometrics.localPointToGlobal(gameObject,newPointB),Color.red);
		Debug.Break ();

		float pointsDistance = (newPointA - newPointB).magnitude;
		if (pointsDistance < diferenceFactorToSplitOrBounce) {
			Destroy (gameObject);
		}
		Vector2 newCenter = new Vector2 ((newPointA.x + newPointB.x) / 2, (newPointA.y + newPointB.y) / 2);
		Vector2 globalCenter = Trigonometrics.localPointToGlobal(gameObject,newCenter);


		Vector3 newTransform = new Vector3 (globalCenter.x ,globalCenter.y, 0);
		float angleFromParent = getCircunferenceAngle (newCenter);

		float globalAngle = angleFromParent + transform.rotation.eulerAngles.z; 
		float angleFromSelf = Mathf.Deg2Rad * Vector2.Angle(newPointA - circunferenceCenter , newCenter - circunferenceCenter  );

		transform.position = newTransform;
		transform.rotation = Quaternion.Euler (0, 0, globalAngle);
		constructor (pointsDistance,angleFromSelf,traslationSpeed,remainingAliveTime);
	}
	*/
}
