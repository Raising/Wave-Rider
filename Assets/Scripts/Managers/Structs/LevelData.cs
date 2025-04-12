using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelData
{
    public string name;
    public string world;
    public string levelId;

    public int waveTarget;
    public float timeTarget;
    public ElementData[] levelElements;

    //public ObstacleData[] obstacleList = new ObstacleData[0];
    //public BallData[] ballList = new BallData[0];
    //public ExitData[] exitList = new ExitData[0];
    //public FlowData[] flowList = new FlowData[0];
    //public BlockData[] blockList = new BlockData[0];

}

