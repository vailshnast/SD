using UnityEngine;
using System.Collections;

public class Game_Scene : MonoBehaviour {

    protected bool shallBeActive;

	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    protected void init()
    {
        shallBeActive = false;
    }

    public bool isSceneActive()
    {
        return shallBeActive;
    }
}
