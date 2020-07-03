using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;
using System.Text;
using System.IO.Ports;
using UnityEngine.UI;
using Random = System.Random;
using System.Linq;



namespace SwipeType.Example
{
    public class GetPatterns : MonoBehaviour
    {


        List<Point> setPattern = new List<Point>();
        Point p;

        // bool typing;
        bool gettingPoints;// 
        bool positioning;

        float xpos;
        float ypos;
        int i;

        Vector3 displac;
        string dir;
        string path;

        /*force*/
        SerialPort stream1 = new SerialPort("COM7", 38400);
        static string myf;
        string myfilename;

        Vector3 displacement;
        Vector3 v = Vector3.zero;
        string[] stringDelimitersFSR = new string[] { ":", };

        float force;
        float lastforce;

        // Use this for initialization
        void Start()
        {
            positioning = false;
            gettingPoints = false;
            // Creating the file name and path
             dir = @"C:\Users\tjdub\Documents\";//Directory.GetCurrentDirectory();
             path = Path.Combine(dir, "File.json");
            stream1.ReadTimeout = 50;
            stream1.Open();

        }

        // Update is called once per frame
        void Update()
        {


            /*check force*/

            string cmd = CheckForRecievedData();

            try
            {
                 print(cmd);
               // print("tafe");
                if (cmd.StartsWith("F")) //Got a force
                {
                 //   print("hey");
                    force = ParseFSRData(cmd);
                    print(force);

                    if (force > 300 && force < 490)  //positoon soft touch
                    {
                        positioning = true;
                        gettingPoints = false;
                    }

                    else if (force > 500)
                    {

                        positioning = false;
                        gettingPoints = true;

                        //while start
                        //while (typing)
                        //{

                        //p.X = mycursor.transform.position.x;
                        //p.Y = mycursor.transform.position.y;


                        //  }//end while

                    }

                    else { gettingPoints = false; positioning = false; }

                }

                //   else { force = 0; typing = false; positioning = false; }
            }
            catch (System.Exception)
            {
                Debug.Log("hie");
            }


            /*position*/
            if (positioning)
            {
                this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                this.GetComponent<TrailRenderer>().widthMultiplier = 0.0f;
                this.GetComponent<TrailRenderer>().time = 0.01f;

                xpos = Input.GetAxis("Mouse X");
                ypos = Input.GetAxis("Mouse Y");

                displac.x = xpos*0.3f;
                displac.y = ypos*0.3f;
                displac.z = 0f;

                //float minX = -5.5f;
                //float maxX = 5.5f;
                //float minY = -2.4f;
                //float maxY = 2.4f;

                // displac.x = Mathf.Clamp(displac.x, minX, maxX);
                //displac.y = Mathf.Clamp(displac.y, minY, maxY);

                Vector3 position = this.transform.position;

                if (this.transform.position.x + displac.x < -5.5)
                    position.x = -5.5f;
                else if (this.transform.position.x + displac.x > 5.5)
                    position.x = 5.5f;
                else
                    position.x += displac.x;

                if (this.transform.position.y + displac.y < -2.4)
                    position.y = -2.4f;
                else if (this.transform.position.y + displac.y > 2.4)
                    position.y = 2.4f;
                else
                    position.y += displac.y;

                //  position.y += displac.y;
                position.z += displac.z;


                try
                {
                    this.transform.position = position;
                }
                catch (System.Exception)
                {

                }
            }



            /*get points */


            if (gettingPoints)
            {


                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.GetComponent<TrailRenderer>().widthMultiplier = 0.1f;
                this.GetComponent<TrailRenderer>().time = 2f;
                xpos = Input.GetAxis("Mouse X");
                ypos = Input.GetAxis("Mouse Y");

                displac.x = xpos*0.3f;
                displac.y = ypos*0.3f;
                displac.z = 0f;



                Vector3 position = this.transform.position;

                if (this.transform.position.x + displac.x < -5.5)
                    position.x = -5.5f;
                else if (this.transform.position.x + displac.x > 5.5)
                    position.x = 5.5f;
                else
                    position.x += displac.x;

                if (this.transform.position.y + displac.y < -2.4)
                    position.y = -2.4f;
                else if (this.transform.position.y + displac.y > 2.4)
                    position.y = 2.4f;
                else
                    position.y += displac.y;

                //  position.y += displac.y;
                position.z += displac.z;


                try
                {
                    this.transform.position = position;
                }
                catch (System.Exception)
                {
                }

                p.X = this.transform.position.x; //xpos;// mycursor.transform.position.x;
                p.Y = this.transform.position.y;// ypos;// mycursor.transform.position.y;

                setPattern.Add(p);

                // drawingPattern[i] = p;

                i = i + 1;
                Debug.Log(i);


            }

            /*write word to json file*/


            if (!gettingPoints && setPattern.Count > 0)
            {

                //change list to array

                Point[] mypoints = new Point[setPattern.Count];

                mypoints = setPattern.ToArray();

                PointPattern pa = new PointPattern();

                pa.Name = "myword";
                pa.Points = mypoints;


                setPattern.Clear();
                PointManager.AddInput(path, pa);
                Debug.Log("shoooo");
            }



            // Create a Point Pattern object
            // This is dependent on your inputs
            // Im using a default constructor in this case,

            Debug.Log("yes yebo");
            //var pattern = new PointPattern();
            //pattern.Name = "Falesto";
            //pattern.Points = new Point[] { new Point { X = 100, Y = 300 }, new Point { X = 400, Y = 300 } };




            //PointManager.AddInput(path, pattern);


            // Point Mannager is a static class with static methods.
            //PointManager.AddInput(path, pattern);

//            PointManager.ReadFile(path);

        }



        public string CheckForRecievedData()
        {
            try //Sometimes malformed serial commands come through. We can ignore these with a try/catch.
            {
                string inData = stream1.ReadLine();
                int inSize = inData.Length;
                if (inSize > 0)
                {
                    int a = 1 + 1;
                     Debug.Log("ARDUINO->|| " + inData + " ||MSG SIZE:" + inSize.ToString());
                }
                //Got the data. Flush the in-buffer to speed reads up.
                inSize = 0;
                stream1.BaseStream.Flush();
                stream1.DiscardInBuffer();
                return inData;
            }
            catch { Debug.Log("ngapha");  return string.Empty; }
        }

        float ParseFSRData(string data) //Read the rotation command string and return a proper Vector3
        {
            int count = 2;

            try
            {
                string[] splitResult = data.Split(stringDelimitersFSR, count, StringSplitOptions.RemoveEmptyEntries);
                float f = float.Parse(splitResult[1]);
                //int y = int.Parse(splitResult[1]);
                //int z = int.Parse(splitResult[2]);
                //force = f; new Vector3(x, y, z);
                lastforce = f;
                return lastforce;
            }
            catch { Debug.Log("Malformed Serial Transmisison"); return lastforce; }
        }
    }
}
