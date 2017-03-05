using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruta {
	private ArrayList subPathInitialPositions = new ArrayList ();
	private ArrayList subPathVelocityVector = new ArrayList ();
	private ArrayList subPathInitialTime = new ArrayList ();

	public Ruta (){
		subPathInitialPositions.Add (new Vector2 (0, 0));
		subPathVelocityVector.Add (new Vector2 (1, 0));
		subPathInitialTime.Add (0f);

		subPathInitialPositions.Add (new Vector2 (5, 0));
		subPathVelocityVector.Add (new Vector2 (0, 1));
		subPathInitialTime.Add (5f);

		subPathInitialPositions.Add (new Vector2 (5, 5));
		subPathVelocityVector.Add (new Vector2 (-1, 0));
		subPathInitialTime.Add (10f);

		subPathInitialPositions.Add (new Vector2 (0, 5));
		subPathVelocityVector.Add (new Vector2 (0, -1));
		subPathInitialTime.Add (15f);
	}

	public Ruta (Vector2 velocity){
		subPathInitialPositions.Add (new Vector2 (0, 0));
		subPathVelocityVector.Add (velocity);

		subPathInitialTime.Add (0f);

	}

	public Vector2 getPositionInGivenTime(float time){
		int currentSubPath = getCurrentSubPath (time);
		float inSubPathTime = time - (float)subPathInitialTime [currentSubPath];
		return (Vector2)subPathInitialPositions [currentSubPath] + ((Vector2)subPathVelocityVector [currentSubPath] * inSubPathTime);
	}

	private int getCurrentSubPath(float time){
		int currentSubPath = 0;
		for (int i = 1; i < subPathInitialTime.Count; i++)
		{
			if ((float)subPathInitialTime [i] < time) {
				currentSubPath = i;
			} else {
				break;
			}
		}
		return currentSubPath;
	}

}
