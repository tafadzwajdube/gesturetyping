using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accelerationToVel : MonoBehaviour {

    public GameObject sphere;
    Vector3 displacement;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        sphere.transform.Translate(displacement);   
	}



    void calculateVel(Vector3 acc, float t)
    {
        Vector3 v;
        float deltat = 0.5f;//Time.   t0.5f;
        Vector3 g =new  Vector3(9.8f, 9.8f, 9.8f);
        acc *= 9.8f; //convert to m/s2

        Vector3 initV = Vector3.zero;

        v.x = acc.x * deltat;
        v.y = acc.y * deltat;
        v.z = 0f;// acc.x * deltat;

        displacement = calculatePos(v, 0.5f);
        //     acc = acc * 9.8f;
        //  acc = acc.Scale(g); 


    }

    Vector3 calculatePos(Vector3 v, float t)
    {
        float deltat = 0.5f;
        Vector3 pos;

        pos.x = v.x * deltat;

        pos.y = v.y * deltat;

        pos.z = v.z * deltat;
        return pos;
    }
}
