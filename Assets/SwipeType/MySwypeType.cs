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

namespace SwipeType.Example
{

    public class MySwypeType : MonoBehaviour {

        //word option objects
       /*static*/ public GameObject option1;
        /*static*/ public GameObject option2;
        /*static*/ public GameObject option3;

        string pattern;
        StringBuilder ptn = new StringBuilder(100);


        SwipeType simpleSwipeType = new MatchSwipeType(File.ReadAllLines("EnglishDictionary.txt"));
        SwipeType distanceSwipeType = new DistanceSwipeType(File.ReadAllLines("EnglishDictionary.txt"));

        bool typing;
        bool startedPattern;

        SerialPort stream = new SerialPort("COM7", 38400);
        static string f;
        string filename;

        Vector3 displacement;
        Vector3 v = Vector3.zero;
        string[] stringDelimitersFSR = new string[] { ":", };

        float force;
        float lastforce;

        float xpos;
        float ypos;
        Vector3 displac;

        string[] wordOptions = new String[3];

        // Use this for initialization
        void Start() {


            // for the fsr

            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            f = string.Format("fsr_" + unixTimestamp);
            filename = @"C:\Users\tjdub\Documents\FSR_MPU9250\" + f + ".tsv";



            string headings = String.Format("{0}\t{1}\t{2}\t\r\n", "FSR", "X", "Y");

            using (FileStream fs = File.Create(filename))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(headings);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }






            stream.ReadTimeout = 50;
            stream.Open();


            //end fsr

            typing = false;
            Debug.Log("started");
            pattern = "heqerqllo";

            startedPattern = false;

           // SampleUsingSwipeType(simpleSwipeType,pattern);
           // Console.ReadKey(true);

            SampleUsingSwipeType(distanceSwipeType, pattern);
           // Console.ReadKey(true);

        }

        

        // Update is called once per frame
        void Update() {


            if (!startedPattern)
            {
                if (ptn.ToString() != "")
                {
                    //get  words
                 wordOptions=SampleUsingSwipeType(simpleSwipeType,ptn.ToString());

                    option1.GetComponent<Text>().text = wordOptions[0];
                    option2.GetComponent<Text>().text = wordOptions[1];
                    option3.GetComponent<Text>().text = wordOptions[2];

                    ptn.Length = 0;
                    ptn.Capacity=0;
                }

            }

            string cmd = CheckForRecievedData();

            try
            {
                // print(stream.ReadLine());

                if (cmd.StartsWith("F")) //Got a force
                {
                    //print("hey");
                    force = ParseFSRData(cmd);
                   // print(force);

                    if (force > 70)
                    {
                        typing = true;

                        //newac 

                        //            prevt = ct;
                        //          ct =  DateTime.Now;



                        //qA[0] = q.x;
                        //qA[1] = q.y;
                        //qA[2] = q.z;
                        //qA[3] = q.w;


                        //get mouse                    


                        //  sphere.transform.Translate(displacement);
                        Debug.Log("ediaaaaaaaaaaa");


                        xpos = Input.GetAxis("Mouse X");
                        ypos = Input.GetAxis("Mouse Y");

                        displac.x = xpos;
                        displac.y = ypos;
                        displac.z = 0f;

                        writeLog();
                        try
                        {
                            this.transform.Translate(displac);
                        }
                        catch (System.Exception)
                        {
                        }

                    }

                    else { typing = false; startedPattern = false; }
                    //Vector3 accl = ParseFSRData(cmd);
                    ////Smoothly rotate to the new rotation position.
                    //target.transform.rotation = Quaternion.Slerp(target.transform.rotation, Quaternion.Euler(accl), Time.deltaTime * 2f);
                }

            }
            catch (System.Exception)
            {
            }





            //end
            if (force > 10) { typing = true; }

            else typing = false;

          //  if(=1)
        }




        void OnCollisionEnter(Collision collision)
        {

            Debug.Log("collided");
            if (typing)
            {
                startedPattern = true;
                if (collision.gameObject.tag == "key")
                {
                    ptn.Append(collision.gameObject.name);
                }
            }


            Debug.Log(ptn);

        }





            public static string[] SampleUsingSwipeType(SwipeType swipeType, string word_pattern)
        {


            Stopwatch stopwatch = new Stopwatch();
            string[] myOptions= new String[3];

            stopwatch.Start();
            var result = swipeType.GetSuggestion(word_pattern, 10);
            stopwatch.Stop();
            stopwatch.Reset();

            int length = result.Length;

            if (length < 1)
            {

                myOptions[0] = "zero";

                myOptions[1] = "zero";

                myOptions[2] = "zero";
            }

            else if(length == 1){


                myOptions[0] = result[0];

                myOptions[1] = "zero";

                myOptions[2] = "zero";
            }

            else if (length == 2)
            {


                myOptions[0] = result[0];

                myOptions[1] = result[1];

                myOptions[2] = "zero";
            }

            else
            {

                myOptions[0] = result[0];

                myOptions[1] = result[1];

                myOptions[2] = result[2];
            }

            return myOptions;

            //option1.GetComponent<Text>().text = result[0];
            //option2.GetComponent<Text>().text = result[1];
            //option3.GetComponent<Text>().text = result[2];

            for (int i = 0; i < 3; ++i)
            {

                Debug.Log(result[i]);
                //Console.WriteLine(result[i]);
            }

            //string[] testCases =
            //{
            //    "heqerqllo",
            //    "qwertyuihgfcvbnjk",
            //    "wertyuioiuytrtghjklkjhgfd",
            //    "dfghjioijhgvcftyuioiuytr",
            //    "aserfcvghjiuytedcftyuytre",
            //    "asdfgrtyuijhvcvghuiklkjuytyuytre",
            //    "mjuytfdsdftyuiuhgvc",
            //    "vghjioiuhgvcxsasdvbhuiklkjhgfdsaserty"
            //};

            //foreach (var s in testCases)
            //{


            //    stopwatch.Start();
            //    var result = swipeType.GetSuggestion(s, 10);
            //    stopwatch.Stop();
            //    stopwatch.Reset();

            //    int length = result.Length;
            //    for (int i = 0; i < length; ++i)
            //    {
            //        i = i;
            //        //Console.WriteLine($"match {i + 1}: {result[i]}");
            //    }
            //}
        }



        //functions

        public string CheckForRecievedData()
        {
            try //Sometimes malformed serial commands come through. We can ignore these with a try/catch.
            {
                string inData = stream.ReadLine();
                int inSize = inData.Length;
                if (inSize > 0)
                {
                    int a = 1 + 1;
                    //  Debug.Log("ARDUINO->|| " + inData + " ||MSG SIZE:" + inSize.ToString());
                }
                //Got the data. Flush the in-buffer to speed reads up.
                inSize = 0;
                stream.BaseStream.Flush();
                stream.DiscardInBuffer();
                return inData;
            }
            catch { return string.Empty; }
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


        //log file
        void writeLog()
        {

            string metrics;

            using (StreamWriter w = File.AppendText(filename))
            {
                metrics = String.Format("{0}\t{1}\t{2}\t\r\n",
                              force, xpos, ypos);
                Log2(metrics, w);
                // Log("Test2", w);
                //Console.WriteLine("Hello World!");
            }

        }

        public static void Log2(string logMessage, TextWriter w)
        {
            //w.Write("\r\nLog Entry : ");
            //w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            //w.WriteLine("  :");
            //w.WriteLine($"  :{logMessage}");
            //w.WriteLine("-------------------------------");
            w.Write(logMessage);
            w.Write("\r\n");

        }





    }
}