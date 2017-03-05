using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPathWave
{
	public Vector2 velocity;
	public Vector2 startPosition ;
	public float startTime = 0;

	public SubPathWave (Vector2 position,Vector2 velocityVector,float time)
	{
		velocity = velocityVector;
		startPosition = position;
		startTime = time;
	}

	public Vector2 getPosition(float time){
		return startPosition + (velocity * (time - startTime));
	}
	 
}