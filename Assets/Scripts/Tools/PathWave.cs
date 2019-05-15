using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWave {
	private List<SubPathWave> subPaths = new List<SubPathWave> ();

	public PathWave (Vector2 initialPosition,Vector2 velocity){
		SubPathWave currentSubPath = new SubPathWave (initialPosition, velocity, 0);
		subPaths.Add (currentSubPath );

		while (currentSubPath != null && currentSubPath.startTime < 10) {
			SubPathWave nextSubPath = World.findNextSubPath (currentSubPath);
			if (nextSubPath != null) {
				subPaths.Add (nextSubPath);
			}
			currentSubPath = nextSubPath;
		}
	}

	/*
	private SubPathWave findNextSubPath (SubPathWave subPath){
		return World.findNextSubPath (subPath);
	}
	*/
	public void setInfluenceInPosition(float time, float timeStep){
         while (time > 0) {
			SubPathWave currentSubPath = getCurrentSubPath (time);
			Vector2 position = currentSubPath.getPosition (time);
            
			if (position.y < 4.2f && position.y > -4.2f && position.x < 9 && position.x > -9) {
				World.addInfluence (position,  currentSubPath.velocity * ((10 - time)/10));
			}
        	time -= timeStep;
		}
	}

	private SubPathWave getCurrentSubPath(float time){
        //perfoermance optimizar las llamadas, no es necesario llamar tantas veces a esta funcion ya que se puede ir acumulando
		int currentSubPath = 0;
		int subPathCuantity = subPaths.Count;
		for (int i = 1; i < subPathCuantity; i++)
		{
			if (subPaths [i].startTime < time) {
				currentSubPath = i;
			} else {
				break;
			}
		}
		return subPaths[currentSubPath];
	}

}
