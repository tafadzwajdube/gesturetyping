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
    public class zonesGesture : MonoBehaviour {



    //for patterns

    List<Point> setPattern = new List<Point>();
    Point p;

    //imported
    string lastcollision;
    private GameObject myText;
    bool startTyping = false;
    DateTime startTime;
    DateTime endTime;
    int textLength;
    string presentedText;
    double WPM;
    double WPMAll;
    double S;
    public InputField toEnterField;
    public InputField enteredField;
    public InputField wpm;
    double ER;
    double INF;
    double totalER;
    double MSD;
    double MSDER;
    string transcribedText;
    string s;
    double IF;
    double CER;
    double UCER;
    static string textFile = @"C:\Users\tjdub\Documents\phrases50.txt";
    string[] myStrings = File.ReadAllLines(textFile);

    int backspace_counter;
    int phraseNumber;

    string IS;

    static string f;// = string.Format("metrics-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
    static string g;// = string.Format("events-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
    string filename;//= @"C:\Users\tjdub\Documents\" + f + ".tsv";
    string eventfile;// = @"C:\Users\tjdub\Documents\" + g + ".tsv";

    string keyboard;
    int x = 0;

    string[] mystring; //= text.Split(' ');



    /* word options*/
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public GameObject option4;


    //ttext and ptext
    public GameObject ptext; //presented text 
    public GameObject ttext; //trascribed text

    string ttSoFar;





    SerialPort stream = new SerialPort("COM7", 38400);
    static string myf;
    string myfilename;

    Vector3 displacement;
    Vector3 v = Vector3.zero;
    string[] stringDelimitersFSR = new string[] { ":", };

    float force;
    float lastforce;

    bool firstType = false;

    string mypath;
    string dir;
    string thecurrentword;

        float totalnavigation=0;
        float averagenavigation = 0;

    //original


    //string zone1 = "qweas";
    //string zone2 = "ertdf";
    //string zone3 = "yuihj";
    //string zone4 = "iopkl";
    //string zone5 = "asozx";
    //string zone6 = "dfgxcv";
    //string zone7 = "hjkbn";
    //string zone8 = "klm";
    //string zone9= "klm";
    //string zone10 = "klm";

    string zone1 = "qwa";
    string zone2 = "wersd";
    string zone3 = "rtyfg";
    string zone4 = "yuihj";
    string zone5 = "iopkl";
    string zone6 = "asz";
    string zone7 = "sdfzxc";
    string zone8 = "fghcvb";
    string zone9 = "jbnmk";
    string zone10 = "mkl";

        string path;

        List<int> zones = new List<int>();
    List<int> pattern = new List<int>();
    List<Node> root = new List<Node>();

    bool typing;
    bool positioning;

    float minX = -4.39f;//;
    float maxX = 1.87f;//5.5f;
    float minY = -2.06f;//-2.4f;
    float maxY = 1.57f;//2.4f;

    float xpos;
    float ypos;
    int i;

    Vector3 displac;

    string currentword;
    int currentw;

        //
        bool mystartTyping;

        //navigation staff

        double totalNavTime = 0;
        DateTime posStartTime;
        DateTime posEndTime;
        bool startedPositioning;
        double averageNavTime = 0;


    // Use this for initialization
    void Start () {

        //    getwordzones("the");

        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        positioning = false;
        typing = false;
            mystartTyping = false;
            startedPositioning = false;

        /* metrics files*/

        myf = string.Format("metrics"+unixTimestamp); //change date format
       // g = string.Format(keyboard + "-events-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        myfilename = @"C:\Users\tjdub\Documents\" + myf + ".tsv";
        //eventfile = @"C:\Users\tjdub\Documents\" + g + ".tsv";

        dir = @"C:\Users\tjdub\Documents\";
            string myjsonfile = string.Format("pattern" + unixTimestamp + ".json");
          //  dir = @"C:\Users\tjdub\Documents\";//Directory.GetCurrentDirectory();
            path = Path.Combine(dir, myjsonfile);

            //Directory.GetCurrentDirectory();
            //      mypath = Path.Combine(dir, "File.json");
            // mypath = @"C:\Users\tjdub\Documents\";
            //myText = GameObject.Find("pText");
            //myText.GetComponent<TextMesh>().text = "";



            phraseNumber = 0;
        backspace_counter = 0;
        ttext.GetComponent<Text>().text = "";
        showTextToTranscribe();

        //create file with headings




         string headings = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\r\n",
                           "TOTALPHRASES", "PHRASENUMBER", "PT", "TT", "PTLENGTH", "TTLENGHT", "WPM", "WPM-all","WER","TotalNavigation", "AverageNavigation");

          using (FileStream fs = File.Create(myfilename))
          {
              Byte[] info = new UTF8Encoding(true).GetBytes(headings);
              // Add some information to the file.
              fs.Write(info, 0, info.Length);
          }



        //create eventfile


        /* string intro = "[]";

         using (FileStream fs1 = File.Create(eventfile))
         {
             Byte[] info = new UTF8Encoding(true).GetBytes(intro);
             // Add some information to the file.
             fs1.Write(info, 0, info.Length);
         }

         */





        // for the fsr

       // Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        f = string.Format("fsr_" + unixTimestamp);
        filename = @"C:\Users\tjdub\Documents\FSR_MPU9250\" + f + ".tsv";



        string headings2 = String.Format("{0}\t{1}\t{2}\t\r\n", "FSR", "X", "Y");

        using (FileStream fs = File.Create(filename))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(headings2);
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }






        stream.ReadTimeout = 50;
        stream.Open();








    }

    // Update is called once per frame
    void Update () {


        if (lastcollision=="enterzone"&&force>300) { enterpressed(); }



        if (!typing && pattern.Count() > 0)
        {

                //change list to array




                Debug.Log("evaluating...");
        if(currentw<mystring.Length)    currentword = mystring[currentw];

            savePattern(currentword);
            getwordzones(currentword);
            compareZones();
           // currentw++;
        }

        //positioning

        if (positioning)
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            this.GetComponent<TrailRenderer>().widthMultiplier = 0.0f;
            this.GetComponent<TrailRenderer>().time = 0.01f;

            ypos = Input.GetAxis("Mouse Y");// - Input.GetAxis("Mouse X");
            xpos = Input.GetAxis("Mouse X");// Input.GetAxis("Mouse Y");

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



        if (typing)
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            this.GetComponent<TrailRenderer>().widthMultiplier = 0.1f;
            this.GetComponent<TrailRenderer>().time = 30f;
            ypos = Input.GetAxis("Mouse Y");//-Input.GetAxis("Mouse X");
            xpos = Input.GetAxis("Mouse X");//Input.GetAxis("Mouse Y");

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
            else if (this.transform.position.y + displac.y > maxX)//2.4)
                position.y = maxX;
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
                p.X = this.transform.position.x; //xpos;// mycursor.transform.position.x;
                p.Y = this.transform.position.y;// ypos;// mycursor.transform.position.y;

                setPattern.Add(p);

                // drawingPattern[i] = p;

                //i = i + 1;
                // Debug.Log(i);





            }





        //fsr

        string cmd = CheckForRecievedData();


        try
        {
            // print(stream.ReadLine());

            if (cmd.StartsWith("F")) //Got a force
            {
                //print("hey");
                force = ParseFSRData(cmd);
    // print(force);

                if (force > 10 && force < 300)  //position soft touch
                {
                      
                        

                    positioning = true;
                    typing = false;

                        if (!startedPositioning)
                        {
                            posStartTime = DateTime.Now;
                            startedPositioning = true;

                        }
                    }

                else if (force > 300) //typing
                {
                    typing = true;
                    positioning = false;
                        if (currentw == 0 && !mystartTyping)
                        {
                           // startedTyping = true;
                            startTime = DateTime.Now;

                            mystartTyping = true;
                        }


                        if (startedPositioning)
                        {
                            double navTime = 0;
                            posEndTime = DateTime.Now;
                            navTime = (posEndTime - posStartTime).TotalSeconds;
                            totalNavTime = totalNavTime + navTime;
                            startedPositioning = false;
                            
                        }
                        //navtime = DateTime.Now - navstart;
                        //totalnavigation = totalnavigation + navtime;
                    //while start
                    //while (typing)
                    //{

                    //p.X = mycursor.transform.position.x;
                    //p.Y = mycursor.transform.position.y;


                    //  }//end while

                }

                else { typing = false; positioning = false; }

            }

            //   else { force = 0; typing = false; positioning = false; }
        }
        catch (System.Exception)
        {
        }







        //movenmouse

       


    }

        //

            void savePattern(string word)
        {


            Point[] mypoints = new Point[setPattern.Count];

            mypoints = setPattern.ToArray();

            PointPattern pa = new PointPattern();

            pa.Name = word;
            pa.Points = mypoints;


            setPattern.Clear();
            PointManager.AddInput(path, pa);
            Debug.Log("shoooo");
        }



    void getwordzones(string word)
    {
        //int index = 0;
        // get all possible zones
        zones.Clear();
        root.Clear();
        thecurrentword = word;
        //create root node
        //   var root = new Node<string>('r');
        int t = 0;
        foreach(char c in word)
        {

            var parent = new Node(t);

            if (zone1.Contains(c.ToString())) {
                parent.add(1);
             //   zones.Add(1);
               // continue;
            }
            if (zone2.Contains(c.ToString()))
            {
                parent.add(2);
                //zones.Add(2);
                //continue;
            }
            if (zone3.Contains(c.ToString()))
            {
                parent.add(3);
                //zones.Add(3);
                //continue;
            }
            if (zone4.Contains(c.ToString()))
            {
                parent.add(4);
                //zones.Add(4);
                //continue;
            }
            if (zone5.Contains(c.ToString()))
            {
                parent.add(5);
                //zones.Add(5);
                //continue;
            }
            if (zone6.Contains(c.ToString()))
            {
                parent.add(6);
                //zones.Add(6);
                //continue;
            }
            if (zone7.Contains(c.ToString()))
            {
                parent.add(7);
                //zones.Add(7);
                //continue;
            }
            if (zone8.Contains(c.ToString()))
            {
                parent.add(8);
                //zones.Add(8);
                //continue;
            }

            if (zone9.Contains(c.ToString()))
            {
                parent.add(9);
                //zones.Add(9);
                //continue;
            }

            if (zone10.Contains(c.ToString()))
            {
                parent.add(10);
                //zones.Add(10);
                //continue;
            }

            root.Add(parent);
        }


        //add node to root node

       

        t++;
        foreach (int i in zones)
        {

            Debug.Log(i);
        }

    }

    void compareZones()
    {
        string wordzones=zones.ToString();
        string drawnpattern = pattern.ToString();
        int j = 0;
        int insidex = 0;
        int x = 0;
        bool found = false;
        
        //foreach(int i in zones)
        //{

        //    while (x < pattern.Count())
        //    {
        //        if (i == pattern[x])
        //        {
        //            j++;
        //            insidex++;
        //            x = insidex-1;
        //            break;
                    
        //        }
        //        x++;
                
        //    }

        //}

        foreach(Node letter in root)
        {
            foreach (Node zone in letter)
            {

                while (x < pattern.Count())
                {
                
                    if (zone.Value == pattern[x])
                    {
                       
                        insidex++;
                        x = insidex - 1;
                        found = true;
                        break;
                    }

                    x++;
                }


                if (found)
                {
                    j++;
                    found = false;
                    break;
                }
                else x = 0;
            }
        }


        int myzones = 0;
        foreach (Node g in root)
        {
         //  myzones = 0;

            foreach (Node h in g)
            {
                myzones++;
            
            }
        }
        Debug.Log("zones in word " + myzones);//zones.Count());
        Debug.Log("zones matched " + j);
        //foreach (char c in drawnpattern)
        //{

        //    if (wordzones.Contains(c.ToString()))
        //    {
        //        //displayword

        //      //  string wo = myTopResults[0].Name + ' ' + myTopResults[0].Probability.ToString();
        //        ttSoFar = ttext.GetComponent<Text>().text;
        //        ttext.GetComponent<Text>().text = ttSoFar + " " + currentword;


        //        j++;
        //    }

        //}

        if (j >= thecurrentword.Length-2)//)zones.Count()-1)
        {
            Debug.Log("matched pattern");
            ttSoFar = ttext.GetComponent<Text>().text;
            ttext.GetComponent<Text>().text = ttSoFar + " " + currentword;
            currentw++;
        }
        else
        {
            backspace_counter++;
        }
            pattern.Clear();
    }

    void getpattern()
    {
        
    }


    //void OnCollisionStay(Collision collision)
    //{
    //    // Debug-draw all contact points and normals
    //    //foreach (ContactPoint contact in collisionInfo.contacts)
    //    //{
    //    //    Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    //}
    //    string zone = collision.gameObject.name;

    //    if(zone=="enterzone" && force > 300)
    //    {
    //        showTextToTranscribe();
    //    }
    //}


    void OnCollisionEnter(Collision collision)
    {
        //string zone= collider.gameobject.nmae

        lastcollision = collision.gameObject.name;
        Debug.Log(collision.gameObject.name);

        if (typing)
        {
            //   Debug.Log("hrllo");
            string zone = collision.gameObject.name;
            if (zone == "zone1") pattern.Add(1);
            else if (zone == "zone2") pattern.Add(2);
            else if (zone == "zone3") pattern.Add(3);
            else if(zone == "zone4") pattern.Add(4);
            else if(zone == "zone5") pattern.Add(5);
            else if(zone == "zone6") pattern.Add(6);
            else if(zone == "zone7") pattern.Add(7);
            else if(zone == "zone8") pattern.Add(8);
            else if (zone == "zone9") pattern.Add(9);

            else if (zone == "zone10") pattern.Add(10);

        }

        //  pattern.ForEach(Debug.Log(pattern));

        foreach (int i in pattern)
        {

            Debug.Log(i);
        }
        //Debug.Log(pattern.ToString());
        // ContactPoint contact = collision.contacts[0];
        // Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //Vector3 position = contact.point;
        // Instantiate(explosionPrefab, position, rotation);
        //Destroy(gameObject);
    }





    //imported functions


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
    void writeLog3()
    {

        string metrics;

        using (StreamWriter w = File.AppendText(filename))
        {
            metrics = String.Format("{0}\t{1}\t{2}\t\r\n",
                          force, xpos, ypos);
            Log3(metrics, w);
            // Log("Test2", w);
            //Console.WriteLine("Hello World!");
        }

    }

    public static void Log3(string logMessage, TextWriter w)
    {
        //w.Write("\r\nLog Entry : ");
        //w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
        //w.WriteLine("  :");
        //w.WriteLine($"  :{logMessage}");
        //w.WriteLine("-------------------------------");
        w.Write(logMessage);
        w.Write("\r\n");

    }



    /*text entry metrics*/

    //private void keypressed(System.Object o, KeyPressEventArgs e)
    //{
    //    // The keypressed method uses the KeyChar property to check 
    //    // whether the ENTER key is pressed. 

    //    // If the ENTER key is pressed, the Handled property is set to true, 
    //    // to indicate the event is handled.
    //    if (e.KeyChar == (char)Keys.Return)
    //    {
    //        e.Handled = true;
    //    }
    //}

    void enterpressed()
    {

        //if (typing)
        //{
            //Debug.Log("i got here0");
            endTime = DateTime.Now;
            //typing = false;
            //Debug.Log("i got here");
            calculateMetrics();
            //Debug.Log("i got here2");
            writeLog();
            //Debug.Log("i got here3");
            //Debug.Log(WPM);
            //Debug.Log(x);
            if (x < 50)
            {
                showTextToTranscribe();
                lastcollision = "";

            }
            else
            {
                enteredField.text = "";
                //showTextToTranscribe(); ;
                myText.GetComponent<TextMesh>().text = "SESSION COMPLETE";
            }
        //}

    }



    void showTextToTranscribe()
    {

        //show text
        backspace_counter = 0;
        getString();
        phraseNumber++;
        currentw = 0;
        



        presentedText = s;// "HELLO TAFIE";
                          //  toEnterField.text = presentedText;
        mystring= presentedText.Split(' ');
        Debug.Log(mystring[1].ToString());
        ptext.GetComponent<Text>().text = presentedText;

            totalNavTime = 0;
            mystartTyping = false;  //not started typing
            startTyping = true;
        ttext.GetComponent<Text>().text = "";
        // enteredField.ActivateInputField();
        //Debug.Log(enteredField.isFocused);
        x++;
    }

    int numberOfWords(string s)
    {
        int word_count=0;
     //   char[] mys=

             foreach (char c in s)
        {

            if (c.Equals(' '))
            {
                word_count++;
            }
        }
     
        return word_count+1;
    }

    void calculateMetrics()
    {


        transcribedText = ttext.GetComponent<Text>().text;
        INF = Levenshteindistance(presentedText.ToLower(), transcribedText.ToLower());
        MSD = INF;
        //Debug.Log("INF");
        //Debug.Log(INF);
        //transcribedText = enteredField.text;
        Debug.Log("backspaces");
        Debug.Log(backspace_counter);


        int word_count = numberOfWords(transcribedText);

        //CALCULATE WPM
        S = (endTime - startTime).TotalSeconds;
        //Debug.Log(S);
        double R = (transcribedText.Length - 2) / (S);
        double Rall = (transcribedText.Length-1) / (S);
        //        Debug.Log(R);

        WPM = (R * 60) / 5;
        WPMAll= (Rall * 60) / 5;
        //   wpm.text = WPM.ToString();

        double error1 = (double)backspace_counter / (word_count+backspace_counter);

         //CALCULATE ER
         ER = error1*100;

       // ER = (backspace_counter / transcribedText.Length) * 100;

        Debug.Log("backspaces");
        Debug.Log(backspace_counter);
        Debug.Log("words");
        Debug.Log(word_count);
        Debug.Log("ER");
        Debug.Log(ER);

        //ER = (INF / transcribedText.Length) * 100;

        //Debug.Log(transcribedText.Length);
        Debug.Log("word count");
        Debug.Log(word_count);

        // Total Error Rate  //text lenght words or characters

        double C = (Mathf.Max(presentedText.Length, transcribedText.Length) - MSD) * 1;

        IF = backspace_counter;

        totalER = (((INF + IF) * 1.0) / (C + INF + IF)) * 100.0;


        // Corrected ER

        CER = IF / (C + MSD + IF) * 100;

        //uncorrected ER

        UCER = MSD / (C + MSD + IF) * 100;



        //MSD error rate



        MSDER = (MSD / Mathf.Max(presentedText.Length, transcribedText.Length)) * 100;

            //keystrokes per character

            //average nav time

            int presentedWords = numberOfWords(presentedText);

            averageNavTime = totalNavTime / presentedWords;





    }





    public static int Levenshteindistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Step 2
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Step 3
        for (int i = 1; i <= n; i++)
        {
            //Step 4
            for (int j = 1; j <= m; j++)
            {
                // Step 5
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Step 6
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Step 7
        return d[n, m];
    }


    void writeLog()
    {

        string metrics;

        //
        int ttwords = numberOfWords(transcribedText);
        int ptwords = numberOfWords(presentedText);

        using (StreamWriter w = File.AppendText(myfilename))
        {
            metrics = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\r\n",
                         "TOTALPHRASES", phraseNumber, presentedText, transcribedText, ptwords,ttwords, WPM, WPMAll, ER,totalNavTime, averageNavTime);
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


    void logThisEvent(string c, DateTime dt)
    {
        string t;
        // double seconds;


        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        // seconds=(dt - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        DateTime xx = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
        TimeSpan result = dt.Subtract(xx);

        int seconds = Convert.ToInt32(result.TotalSeconds);

        //   int seconds = Convert.ToInt32(result.TotalSeconds);

        seconds = dt.Second;
        t = seconds.ToString();




        string u = "[" + unixTimestamp + " ," + enteredField.text + ", " + c + ",]";

        using (StreamWriter w1 = File.AppendText(eventfile))
        {
            w1.Write(u);
            w1.Write("\t");
            //2(metrics, w);
            // Log("Test2", w);
            //Console.WriteLine("Hello World!");
        }

    }

    void getString()
    {


        string[] sy = new String[5];
        sy[0] = "we are having spaghetti";
        sy[1] = "time to go shopping";
        sy[2] = "the world is a stage";
        sy[3] = "one heck of a question";
        sy[4] = "I agree with you";

        Random i = new Random();
            //  int x = i.Next(499);
            int x = i.Next(50);

            //  i=random.
            s = myStrings[x];// sy[x];



    }





    void prepareLogs(string keyboard)
    {


        f = string.Format(keyboard + "-metrics-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        g = string.Format(keyboard + "-events-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        filename = @"C:\Users\tjdub\Documents\" + f + ".tsv";
        eventfile = @"C:\Users\tjdub\Documents\" + g + ".tsv";

        myText = GameObject.Find("pText");
        myText.GetComponent<TextMesh>().text = "";


        phraseNumber = 0;
        x = 0;
        showTextToTranscribe();

        //create log file

        Console.WriteLine(filename);



        //create file with headings


        string headings = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\r\n",
                         "TOTALPHRASES", "PHRASENUMBER", "PT", "TT", "PPLENGTH", "TTLENGHT", "WPM", "ER", "MSDER", "CER", "UCER", "TER", "BSPACE", "KSPC", "EKSER", "KEYBOARD", "HANDS");

        using (FileStream fs = File.Create(filename))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(headings);
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }



        //create eventfile


        string intro = "[]";

        using (FileStream fs1 = File.Create(eventfile))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(intro);
            // Add some information to the file.
            fs1.Write(info, 0, info.Length);
        }


    }
}


}
