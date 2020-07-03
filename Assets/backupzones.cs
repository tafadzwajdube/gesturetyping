using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupzones : MonoBehaviour {


    string zone1 = "qweas";
    string zone2 = "ertdf";
    string zone3 = "yuihj";
    string zone4 = "iopkl";
    string zone5 = "asozx";
    string zone6 = "dfgxcv";
    string zone7 = "hjkbn";
    string zone8 = "klm";

    List<int> zones = new List<int>();
    List<int> pattern = new List<int>();

    bool typing = false;

    float minX = -4.39f;//;
    float maxX = 1.87f;//5.5f;
    float minY = -2.06f;//-2.4f;
    float maxY = 1.57f;//2.4f;

    float xpos;
    float ypos;
    int i;

    Vector3 displac;


    // Use this for initialization
    void Start()
    {

        getwordzones("the");

    }

    // Update is called once per frame
    void Update()
    {



        //movenmouse

        xpos = Input.GetAxis("Mouse X");
        ypos = Input.GetAxis("Mouse Y");

        displac.x = xpos;
        displac.y = ypos;
        displac.z = 0f;

        displac.x = xpos * 0.3F;
        displac.y = ypos * 0.3F;
        displac.z = 0f;



        Vector3 position = this.transform.position;

        if (this.transform.position.x + displac.x < minX)//-5.5)
            position.x = minX;//-5.5f;
        else if (this.transform.position.x + displac.x > maxX)// 5.5)
            position.x = maxX;//5.5f;
        else
            position.x += displac.x;

        if (this.transform.position.y + displac.y < minY)//-2.4)
            position.y = minY;
        else if (this.transform.position.y + displac.y > maxY)//2.4)
            position.y = maxY;
        else
            position.y += displac.y;

        //  position.y += displac.y;
        position.z += displac.z;
        //  myTransform.position = position;


        //   displac.x = Mathf.Clamp(displac.x, minX, maxX);
        //  displac.y = Mathf.Clamp(displac.y, minY, maxY);


        try
        {
            this.transform.position = position;
            // this.transform.Translate(displac);
        }
        catch (System.Exception)
        {
        }


    }

    void getwordzones(string word)
    {
        // get all possible zones
        zones.Clear();

        foreach (char c in word)
        {
            if (zone1.Contains(c.ToString()))
            {
                zones.Add(1);
                continue;
            }
            if (zone2.Contains(c.ToString()))
            {
                zones.Add(2);
                continue;
            }
            if (zone3.Contains(c.ToString()))
            {
                zones.Add(3);
                continue;
            }
            if (zone4.Contains(c.ToString()))
            {
                zones.Add(4);
                continue;
            }
            if (zone5.Contains(c.ToString()))
            {
                zones.Add(5);
                continue;
            }
            if (zone6.Contains(c.ToString()))
            {
                zones.Add(6);
                continue;
            }
            if (zone7.Contains(c.ToString()))
            {
                zones.Add(7);
                continue;
            }
            if (zone8.Contains(c.ToString()))
            {
                zones.Add(8);
                continue;
            }


        }


        foreach (int i in zones)
        {

            Debug.Log(i);
        }

    }

    void compareZones()
    {
        string wordzones = zones.ToString();
        string drawnpattern = pattern.ToString();
        int j = 0;

        foreach (char c in drawnpattern)
        {

            if (wordzones.Contains(c.ToString()))
            {
                //displayword
                j++;
            }

        }

        if (j >= wordzones.Length)
        {
            Debug.Log("matched pattern");
        }
        pattern.Clear();
    }

    void getpattern()
    {

    }


    //void OnCollisionEnter(Collision collision)
    //{
    //    //string zone= collider.gameobject.nmae

    //    Debug.Log(collision.gameObject.name);

    //    if (true)
    //    {
    //     //   Debug.Log("hrllo");
    //        string zone = collision.gameObject.name;
    //        if (zone == "zone1") pattern.Add(1);
    //        if (zone == "zone2") pattern.Add(2);
    //        if (zone == "zone3") pattern.Add(3);
    //        if (zone == "zone4") pattern.Add(4);
    //        if (zone == "zone5") pattern.Add(5);
    //        if (zone == "zone6") pattern.Add(6);
    //        if (zone == "zone7") pattern.Add(7);
    //        if (zone == "zone8") pattern.Add(8);
    //    }

    //    //  pattern.ForEach(Debug.Log(pattern));

    //   foreach (int i in pattern)
    //    {

    //        Debug.Log(i);
    //    }
    //    //Debug.Log(pattern.ToString());
    //   // ContactPoint contact = collision.contacts[0];
    //   // Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //    //Vector3 position = contact.point;
    //   // Instantiate(explosionPrefab, position, rotation);
    //    //Destroy(gameObject);
    //}
}
