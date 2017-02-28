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
	private GameObject waveImage = null;
	[SerializeField]
	private float diferenceFactorToSplitOrBounce = 0.1f; 
	[SerializeField]
	private float minimumCollisionAngle = 0.2f; 
	[SerializeField]
	private float reshapeFactor = 0.1f; 

	private ArrayList dots = null;

	private bool noCollision = false;
	public Vector2 pointA = new Vector2(0,0); 
	public Vector2 pointB = new Vector2(0,0); 
	public Vector2 circunferenceCenter = new Vector2(0,0);
	public float circunferenceRadius = 0;


	private Rigidbody2D rigidBody = null;
	//Angulos en radianes
	public float pointsDispersionAngle = Mathf.PI/4; 

	public float remainingAliveTime = 5f;

	private EdgeCollider2D edgeCollider;
	private GameObject inmunityObstacle = null; 
	private Color selfColor;

	[SerializeField]
	private string reshapeMode = "undefined";
	private ObstacleWall wall = null;



	private float currentReshapeTime = 0;
	private float totalRechapeTime = 0.4f;
	// Use this for initialization


	void Start () {
		edgeCollider = gameObject.GetComponent<EdgeCollider2D> ();
		selfColor = Random.ColorHSV (0f, 1f, 1f, 1f, 1f, 1f);
		dots = new ArrayList ();
		rigidBody = GetComponent<Rigidbody2D> ();
		//rigidBody.velocity = Vector2.up;
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
		
	public void HandleObstacleCollision(PolygonCollider2D wallCollider){
		if (isInmune (wallCollider.gameObject)) {
			return;
		}
			
		wall = new ObstacleWall(wallCollider);
		wall.defineAproachigWave (this);
		float angleA = -1 * pointsDispersionAngle;
		float angleB = pointsDispersionAngle;
		float firstImpactAngle = wall.firstImpactAngle ();
		float wallLeftBound = wall.getLeftBound ();
		float wallRightBound = wall.getRightBound ();

		if (wallLeftBound >= wallRightBound) {
			return;
		}

		if (wall.isLeftExceedByWave ()) {
			WaveSector leftExceed = createSplitedSector (angleA,wallLeftBound - (minimumCollisionAngle/ circunferenceRadius) );
			angleA = wallLeftBound;
		}

		if (wall.isRightExceedByWave ()) {
			WaveSector rightExceed = createSplitedSector (wallRightBound  + minimumCollisionAngle/ circunferenceRadius,angleB);
			angleB = wallRightBound;
		}

		if (firstImpactAngle - angleA > minimumCollisionAngle) {
			generateBouncingColisioningPair (angleA, firstImpactAngle, "Right");
		}
		if ( angleB - firstImpactAngle  > minimumCollisionAngle) {
			generateBouncingColisioningPair (firstImpactAngle, angleB, "Left");

		}
		FullDestroy();
	}

	private void generateBouncingColisioningPair ( float leftAngle,float rightAngle,string direction){
		float distanceToEndShape = Mathf.Max(wall.getDistanceFromCircunferenceToWall(rightAngle),wall.getDistanceFromCircunferenceToWall(leftAngle));
		float timeToEndShape = distanceToEndShape  * reshapeFactor / traslationSpeed;

		WaveSector colisioner = createSplitedSector (leftAngle,rightAngle);	

		WaveSector bouncer = colisioner.createClone ();
		bouncer.gameObject.transform.position = wall.getMirrorPosition (colisioner.gameObject.transform.position); 
		bouncer.gameObject.transform.rotation = wall.getMirrorRotation (colisioner.gameObject.transform.rotation); 
		bouncer.setInmunityToObstacle (wall.gameObject);
		bouncer.remainingAliveTime = bouncer.remainingAliveTime - 1;

		if (direction == "Right") {
			colisioner.setReshape ("DestroyFromRight", timeToEndShape);
			bouncer.setReshape ("CreateFromLeft", timeToEndShape);
		} else if (direction == "Right") {
			colisioner.setReshape ("DestroyFromLeft", timeToEndShape);
			bouncer.setReshape ("CreateFromRight", timeToEndShape);
		}
		colisioner.setTotalInmunity ();
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
		waveSectorInterface.constructor ((newPointA-newPointB).magnitude,angleFromSelf,traslationSpeed,remainingAliveTime );
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
					FullDestroy ();
				} 
				leftLimit = reshapePercent;
				break;
			case "DestroyFromRight":
				if (reshapePercent == 1) {	
					FullDestroy ();
				} 
				rightLimit = 1 - reshapePercent;
				
				break;
			case "CreateFromRight":
				leftLimit = 1 - reshapePercent;
				break;
			case "CreateFromLeft":
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
		float percent = Mathf.Pow( Mathf.Min(1, Mathf.Max(0, currentReshapeTime / totalRechapeTime)),0.4f);
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
			FullDestroy ();
		} else {
			remainingAliveTime = remainingAliveTime - Time.deltaTime;
		}
	}

	private Vector2[] calculateCirucnferencePoints (float startAnglePercent , float endAnglePercent){
		float  startAngle =  ((startAnglePercent * 2) - 1)  * pointsDispersionAngle;


		int numberOfPoints = 2 + (int)Mathf.Floor (colliderPointDensityFactor * Mathf.Abs (pointA.x * 2) * (endAnglePercent - startAnglePercent));
		Vector2[] circunferencePoints = new Vector2[numberOfPoints + 1];

		float angleStep = (pointsDispersionAngle * 2) / (float)numberOfPoints;
		angleStep = angleStep * (endAnglePercent - startAnglePercent);

		for (int i = 0; i <= numberOfPoints; i++) {
			float sectorProportionAngle = startAngle +  i * angleStep ;
			Vector2 pointInSector = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,sectorProportionAngle);
			circunferencePoints [i] = pointInSector;

			if (i > 0) {
				DrawSegment (circunferencePoints [i - 1], circunferencePoints [i]);
			}
		}

		numberOfPoints = 2 + (int)Mathf.Floor (spritesDensityFactor * Mathf.Abs (pointA.x * 2)* (endAnglePercent - startAnglePercent));

		angleStep = (pointsDispersionAngle * 2) / (float)numberOfPoints;
		angleStep = angleStep * (endAnglePercent - startAnglePercent);

		for (int i = 0; i <= numberOfPoints; i++) {
			if (i >= dots.Count) {
				GameObject newDot = Instantiate (waveImage);
				dots.Add(newDot);
			}
			float sectorProportionAngle = startAngle +  i * angleStep ;
			Vector2 pointInSector = Trigonometrics.GetCircunferencePoint (circunferenceCenter,circunferenceRadius,sectorProportionAngle);
			Vector2 globalPoint = Trigonometrics.localPointToGlobal (gameObject,pointInSector);
			((GameObject)dots [i]).transform.position = new Vector3 (globalPoint.x, globalPoint.y, 0);


		}


		return circunferencePoints;
	}
	private void FullDestroy(){
		foreach (Object dot in dots) {
			Destroy ((GameObject)dot);
		}
		Destroy (gameObject);
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
		gameObject.layer = 8;
	}

	public float getImpulso (){
		return 5;
	}
	public Vector2 getDireccion(Vector2 position){
		Vector2 centroGlobal = Trigonometrics.localPointToGlobal (gameObject, circunferenceCenter);
		return (position - centroGlobal).normalized; 
	}
}
