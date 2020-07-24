using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathWave {
	private List<SubPathWave> subPaths = new List<SubPathWave> ();
    private int limiteRebotes = 4;
	public PathWave (Vector2 initialPosition,Vector2 velocity){
		SubPathWave currentSubPath = new SubPathWave (initialPosition, velocity, 0);
		subPaths.Add (currentSubPath );

		while (currentSubPath != null && currentSubPath.startTime < 10 && subPaths.Count < limiteRebotes) {
			SubPathWave nextSubPath = World.findNextSubPath (currentSubPath);
          
            if (nextSubPath != null) {
                if (subPaths.Count == limiteRebotes - 1)
                {
                    nextSubPath.velocity = Vector2.zero;
                }
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
	public void setInfluenceInPosition(float time,  float timeStep, float minTime = 0)
    {
        minTime = Mathf.Max(minTime, 0);
         while (time >= minTime ) {
			SubPathWave currentSubPath = getCurrentSubPath (time);
            if(currentSubPath.velocity == Vector2.zero)
            {
                time -= timeStep;
                continue;
            }
			Vector2 position = currentSubPath.getPosition (time);
		//	if (position.y < 5f && position.y > -5f && position.x < 9.6f && position.x > -9.6) {
                //se está usando indistintamente intensidad como velocidad
				World.addInfluence (position,  currentSubPath.velocity * ((20 - time)*0.01f));
		//	}
        	time -= timeStep;
		}
	}

	private SubPathWave getCurrentSubPath(float time){
        //perfoermance optimizar las llamadas, no es necesario llamar tantas veces a esta funcion ya que se puede ir acumulando
		int currentSubPath = 0;
		int subPathCuantity = subPaths.Count;
		for (int i = 1; i < subPathCuantity; i++)
		{
            if (subPaths[i].startTime < time) {
				currentSubPath = i;
			} else {
				break;
			}
		}
		return subPaths[currentSubPath];
	}

}
