using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class WaveSector : MonoBehaviour {
	[SerializeField]
	private float traslationSpeed = 1; 
	[SerializeField]
	private float colliderPointDensityFactor = 2; 
	[SerializeField]
	private float spritesDensityFactor = 2; 
	[SerializeField]
	private GameObject waveSectorPrefab = null; 
	[SerializeField]
	private float diferenceFactorToSplitOrBounce = 0.1f; 
	[SerializeField]
	private float minimumCollisionAngle = 0.2f; 
	[SerializeField]
	private float reshapeFactor = 0.1f; 



	private bool noCollision = false;
	public Vector2 pointA = new Vector2(0,0); 
	public Vector2 pointB = new Vector2(0,0); 
	public Vector2 circunferenceCenter = new Vector2(0,0);
	public float circunferenceRadius = 0;

	//Angulos en radianes
	public float pointsDispersionAngle = Mathf.PI/4; 

	public float remainingAliveTime = 5f;

	private EdgeCollider2D edgeCollider;
	private GameObject inmunityObstacle = null; 
	private Color selfColor;

	[SerializeField]
	private string reshapeMode = "undefined";
	private float currentReshapeTime = 0;
	private float totalRechapeTime = 0.4f;
	// Use this for initialization


	void Start () {
		edgeCollider = gameObject.GetComponent<EdgeCollider2D> ();
		selfColor = Random.ColorHSV (0f, 1f, 1f, 1f, 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		controlRemainingTime ();
		Advance ();

	}

	private void Advance(){
		recalculatePointsABC ();
		recalculateCollider ();
		moveForward ();
	}
		
	public void HandleObstacleCollision(Collision2D collision){
		PolygonCollider2D wallCollider = (PolygonCollider2D)collision.contacts [0].otherCollider;
		if (isInmune (wallCollider.gameObject)) {
			return;
		}
			
		ObstacleWall wall = new ObstacleWall(wallCollider);
		wall.defineAproachigWave (this);
		float angleA = -1 * pointsDispersionAngle;
		float angleB = pointsDispersionAngle;
		float firstImpactAngle = wall.firstImpactAngle ();

		if (wall.getLeftBound () >= wall.getRightBound ()) {
			return;
		}

		if (wall.isLeftExceedByWave ()) {
			float wallLeftBound = wall.getLeftBound ();
			WaveSector leftExceed = createSplitedSector (angleA,wallLeftBound - minimumCollisionAngle/ circunferenceRadius );
			angleA = wallLeftBound;
		}

		if (wall.isRightExceedByWave ()) {
			float wallRightBound = wall.getRightBound ();
			WaveSector rightExceed = createSplitedSector (wallRightBound  + minimumCollisionAngle/ circunferenceRadius,angleB);
			angleB = wallRightBound;
		}
			
		if (firstImpactAngle - angleA > minimumCollisionAngle) {


			float distanceToEndShape = wall.getDistanceFromCircunferenceToWall(firstImpactAngle);

			float timeToEndShape = distanceToEndShape  * reshapeFactor / traslationSpeed;
			//Debug.Log ("distance = " + distanceToEndShape + "  tiempo = " + timeToEndShape);
			WaveSector leftBounce = createSplitedSector (angleA,firstImpactAngle);	
			leftBounce.setTotalInmunity ();
			leftBounce.setReshape ("DestroyFromLeft",timeToEndShape);

			WaveSector leftBounceMirror = leftBounce.createClone ();
			leftBounceMirror.gameObject.transform.position = wall.getMirrorPosition (leftBounce.gameObject.transform.position); 
			leftBounceMirror.gameObject.transform.rotation = wall.getMirrorRotation (leftBounce.gameObject.transform.rotation); 
			leftBounceMirror.setInmunityToObstacle (wallCollider.gameObject);

			leftBounceMirror.setReshape ("CreateFromLeft",timeToEndShape);
		}

		if ( angleB - firstImpactAngle  > minimumCollisionAngle) {
			float distanceToEndShape = wall.getDistanceFromCircunferenceToWall(firstImpactAngle); 
			float timeToEndShape = distanceToEndShape *  reshapeFactor / traslationSpeed;

			WaveSector rightBounce = createSplitedSector (firstImpactAngle,angleB);	
			rightBounce.setTotalInmunity();
			rightBounce.setReshape ("DestroyFromRight",timeToEndShape);

			WaveSector rightBounceMirror = rightBounce.createClone ();
			rightBounceMirror.gameObject.transform.position = wall.getMirrorPosition (rightBounce.gameObject.transform.position); 
			rightBounceMirror.gameObject.transform.rotation = wall.getMirrorRotation (rightBounce.gameObject.transform.rotation); 
			rightBounceMirror.setInmunityToObstacle (wallCollider.gameObject);


			rightBounceMirror.setReshape ("CreateFromRight",timeToEndShape);
		}
		//Debug.Break();
		Destroy (gameObject);
	}

	public void constructor(float pointsDistance,float newPointsDispersionAngle, float newSpeed, float newRemainingTime){
		remainingAliveTime = newRemainingTime;
		traslationSpeed = newSpeed;
		pointsDispersionAngle = newPointsDispersionAngle;

		pointA = new Vector2 (-1 * pointsDistance / 2, 0);
		pointB = new Vector2 ( pointsDistance / 2, 0);

		//reshapeMode = "";
	}




	private WaveSector createSplitedSector(float angleA,float angleB){
		Vector2 newPointA = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,angleA); 
		Vector2 newPointB = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,angleB); 
		Vector2 newCenter = new Vector2 ((newPointA.x + newPointB.x) / 2, (newPointA.y + newPointB.y) / 2);

		Vector2 globalCenter = Trigonometrics.localPointToGlobal(gameObject,newCenter);
		Vector3 newPosition = new Vector3 (globalCenter.x ,globalCenter.y, 0);

		float angleFromParent = getCircunferenceAngle (newCenter);
		float globalAngle = angleFromParent + transform.rotation.eulerAngles.z; 
		float angleFromSelf = Mathf.Deg2Rad * Vector2.Angle(newPointA - circunferenceCenter , newCenter - circunferenceCenter  );

		GameObject newWaveSector = Instantiate(waveSectorPrefab,newPosition,  Quaternion.Euler(0, 0, globalAngle));
		WaveSector waveSectorInterface = newWaveSector.GetComponent<WaveSector> ();
		waveSectorInterface.constructor ((newPointA-newPointB).magnitude,angleFromSelf,traslationSpeed,remainingAliveTime);
		return waveSectorInterface;
	}

	private WaveSector createClone(){
		GameObject newWaveSector = Instantiate(waveSectorPrefab,transform.position, transform.rotation);
		WaveSector waveSectorInterface = newWaveSector.GetComponent<WaveSector> ();
		waveSectorInterface.constructor ((pointA-pointB).magnitude,pointsDispersionAngle,traslationSpeed,remainingAliveTime);
		return waveSectorInterface;
	}

	private void moveForward(){
		float distancia = Time.deltaTime * traslationSpeed * Mathf.Cos(pointsDispersionAngle);
		float directionAngle = Mathf.Deg2Rad * transform.rotation.eulerAngles.z;
		Vector2 MovementVector = distancia * new Vector2(-1 * Mathf.Sin(directionAngle),Mathf.Cos(directionAngle));
		gameObject.transform.position += new Vector3(MovementVector.x,MovementVector.y,0);
	}

	private void setReshape(string newReshapeMode,float endTime){
		reshapeMode = newReshapeMode;
		totalRechapeTime = endTime;
		currentReshapeTime = 0;
	}

	private void recalculateCollider(){
		float leftLimit = 0;
		float rightLimit = 1;

		float reshapePercent = getReshapePercent ();
		switch (reshapeMode) {
			case "DestroyFromLeft":
				if (reshapePercent == 1) {
				Destroy (gameObject);
				} 
				leftLimit = reshapePercent;
				break;
			case "DestroyFromRight":
				if (reshapePercent == 1) {
					Destroy (gameObject);
				} 
				rightLimit = 1 - reshapePercent;
				
				break;
			case "CreateFromLeft":
				leftLimit = 1 - reshapePercent;
				break;
			case "CreateFromRight":
				rightLimit =  reshapePercent;
				break;
			case "undefined":
				leftLimit = 0;
				rightLimit = 0;
				reshapeMode = "";
				break;
			case "":
				reshapeMode = "";
				break;

		}


		edgeCollider.points = calculateCirucnferencePoints (leftLimit,rightLimit);
	}

	private float getReshapePercent (){
		float percent = Mathf.Min(1, Mathf.Max(0, currentReshapeTime / totalRechapeTime));
		currentReshapeTime += Time.deltaTime;
		//Debug.Log ("percent:" + percent + "   cosPercent : " + Mathf.Cos ((1 - percent) * Mathf.PI / 2));
		return  Mathf.Cos ((1 - percent) * Mathf.PI / 2);
	}
	private void recalculatePointsABC(){
		float distancia = Time.deltaTime * traslationSpeed * Mathf.Cos(pointsDispersionAngle);
		pointA += Vector2.left * Mathf.Tan (pointsDispersionAngle) * distancia;
		pointB += Vector2.right * Mathf.Tan (pointsDispersionAngle) * distancia;
		circunferenceCenter = getCircunferenceCenterPoint();
		circunferenceRadius = (pointA - circunferenceCenter).magnitude;
	}

	private Vector2 getCircunferenceCenterPoint(){
		float DistanceCenterToCircunferenceCenter =  0 ;
		if (pointsDispersionAngle > 0) {
			DistanceCenterToCircunferenceCenter = Mathf.Abs (pointA.x) / Mathf.Tan (pointsDispersionAngle);
		} 
		return new Vector2 (0, -1 * DistanceCenterToCircunferenceCenter);
	}

	private void controlRemainingTime(){
		if (remainingAliveTime < 0) {
			Destroy(gameObject);
		} else {
			remainingAliveTime = remainingAliveTime - Time.deltaTime;
		}
	}

	private Vector2[] calculateCirucnferencePoints (float startAnglePercent , float endAnglePercent){
		int numberOfPoints = 2 + (int)Mathf.Floor (colliderPointDensityFactor * Mathf.Abs (pointA.x * 2));
		Vector2[] circunferencePoints = new Vector2[numberOfPoints + 1];

		float angleStep = (pointsDispersionAngle * 2) / (float)numberOfPoints;
		angleStep = angleStep * (endAnglePercent - startAnglePercent);
		float  startAngle =  ((startAnglePercent * 2) - 1)  * pointsDispersionAngle;
		for (int i = 0; i <= numberOfPoints; i++) {
			float sectorProportionAngle = startAngle +  i * angleStep ;
			Vector2 pointInSector = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,sectorProportionAngle);
			circunferencePoints [i] = pointInSector;

			if (i > 0) {
				DrawSegment (circunferencePoints [i - 1], circunferencePoints [i]);
			}
		}

		return circunferencePoints;
	}

	private void DrawSegment(Vector2 point1,Vector2 point2){
		Debug.DrawLine(Trigonometrics.localPointToGlobal(gameObject,point1), Trigonometrics.localPointToGlobal(gameObject,point2),selfColor);
	}

	private Vector2 getPosition2D (){
		return new Vector2 (transform.position.x, transform.position.y);
	}







	private bool isInmune(GameObject obstacle){
		if (noCollision == true) {
			return true;
		}
		if (inmunityObstacle == null) {
			return false;
		}
		return inmunityObstacle.Equals(obstacle); 
	}

	private void setInmunityToObstacle(GameObject obstacle){
		inmunityObstacle = obstacle; 
	}

	public float getCircunferenceAngle(Vector2 point){
		float angleFromCircunferenceCenter = Vector2.Angle(point - circunferenceCenter , Vector2.up  );
		Vector3 cross = Vector3.Cross(point - circunferenceCenter, Vector2.up);
		if (cross.z > 0) {
			angleFromCircunferenceCenter = -1 * angleFromCircunferenceCenter;
		}
		return angleFromCircunferenceCenter;
	}

	private void setTotalInmunity(){
		noCollision = true;
	}
}
