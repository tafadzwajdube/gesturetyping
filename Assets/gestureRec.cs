using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SwipeType.Example
{
    public class gestureRec : MonoBehaviour
    {

        private readonly PointPatternAnalyzer myAnalyzer = new PointPatternAnalyzer();

        List<PointPatternMatchResult> myTopResults = new List<PointPatternMatchResult>();

        PointPatternMatchResult myempty = new PointPatternMatchResult();

        PointPatternAnalyzer myPointAnalyzer = new PointPatternAnalyzer();

        Point[] drawingPattern = new Point[] { };

        List<Point> myPattren = new List<Point>();
        List<PointPattern> mypt = new List<PointPattern>();

        string mypath;
        string dir;
        Point p;
        bool typing = false;


        float minX = -3.78f;//;
        float maxX = 2.46f;//5.5f;
        float minY = -2.18f;//-2.4f;1.61
        float maxY = 1.61f;//2.4f;

        float xpos;
        float ypos;
        int i;

        Vector3 displac;
        /* word options*/
        public GameObject option1;
        public GameObject option2;
        public GameObject option3;
        public GameObject option4;
        public GameObject trascribedText;
        string firstLetter;
        string lastCollision;
        bool first = true;

        // Use this for initialization
        void Start()
        {

            dir = @"C:\Users\tjdub\Documents\";//Directory.GetCurrentDirectory();
            mypath = Path.Combine(dir, "WordsFileTest.json");
            // mypath = @"C:\Users\tjdub\Documents\";
            //myText = GameObject.Find("pText");
            //myText.GetComponent<TextMesh>().text = "";

            myAnalyzer.PointPatternSet = PointManager.ReadFile(mypath);
            option1.GetComponent<Text>().text = "";
            option2.GetComponent<Text>().text = "";
            option3.GetComponent<Text>().text = "";
            option4.GetComponent<Text>().text = "";
            trascribedText.GetComponent<Text>().text = "";

        }

        // Update is called once per frame
        void Update()
        {


            if (Input.GetMouseButtonDown(0)) {
                typing = true;
                firstLetter = lastCollision;
                // Debug.Log("first is " + firstLetter);

            }// Debug.Log("Pressed left click.");
            if (Input.GetMouseButtonDown(1)) typing = false;// Debug.Log("Pressed right click.");



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
            else if (this.transform.position.y + displac.y > maxX)//2.4)
                position.y = maxX;
            else
                position.y += displac.y;

            //  position.y += displac.y;
            position.z += displac.z;




            try
            {
                this.transform.position = position;
                // this.transform.Translate(displac);
            }
            catch (System.Exception)
            {
            }


            if (typing)
            {

                this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                this.GetComponent<TrailRenderer>().widthMultiplier = 0.1f;
                this.GetComponent<TrailRenderer>().time = 6f;
                xpos = Input.GetAxis("Mouse X");
                ypos = Input.GetAxis("Mouse Y");

                displac.x = xpos;
                displac.y = ypos;
                displac.z = 0f;

                displac.x = xpos * 0.3F;
                displac.y = ypos * 0.3F;
                displac.z = 0f;

                position = this.transform.position;

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


              

                try
                {
                    this.transform.position = position;
                    // this.transform.Translate(displac);
                }
                catch (System.Exception)
                {
                }

                p.X = this.transform.position.x;//xpos;// mycursor.transform.position.x;
                p.Y = this.transform.position.y; ;//ypos;// mycursor.transform.position.y;

                myPattren.Add(p);

                // drawingPattern[i] = p;

                i = i + 1;
            }


            if(!typing&& myPattren.Count > 0)
            {

                this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

                Point[] mypointsN = new Point[myPattren.Count];

                 mypointsN = myPattren.ToArray();

                var interPolatedArray = PointPatternMath.GetInterpolatedPointArray(mypointsN, 20);


                myPattren.Clear();

                //   compare with all patterns

                RecognizeGesture(mypointsN);

                Debug.Log(myTopResults);
            }

            /* when user finishes entering a word*/
            //if (!typing && myPattren.Count > 0)
            //{
            //    Debug.Log("list debug");
            //    foreach (Point p in myPattren)
            //    {

            //        Debug.Log(p.X + " " + p.Y);
            //    }


            //    //interpolate and print

            //    //change list to array

            //    Point[] mypointsN = new Point[myPattren.Count];

            //    mypointsN = myPattren.ToArray();

            //    var interPolatedArray = PointPatternMath.GetInterpolatedPointArray(mypointsN, 20);
            //    Debug.Log(interPolatedArray.Length);

            //    Debug.Log(interPolatedArray[2].X);

            //    for (int t = 0; t < interPolatedArray.Length; t++)
            //    {
            //        Debug.Log("interpolated array");
            //        Debug.Log(interPolatedArray[t].X + " " + interPolatedArray[t].Y);

            //    }


            //    //empty list

            //    myPattren.Clear();

            //    //compare with all patterns

            //    RecognizeGesture(mypointsN);

            //    //list of patterns

            //    //mypt.

            //    // myPointAnalyzer.GetPointPatternMatchResult(interPolatedArray,interPolatedArray);


            //    //add word with highest probability to the text input area, and show all 3 options closest

            //    //showpresentedText();

            //    string wo = myTopResults[0].Name + ' ' + myTopResults[0].Probability.ToString();
            //    ttSoFar = ttext.GetComponent<Text>().text;
            //    ttext.GetComponent<Text>().text = ttSoFar + " " + wo;// " " + " transcribed text";// wordResult[1];

            //    option1.GetComponent<Text>().text = myTopResults[0].Name;
            //    option2.GetComponent<Text>().text = myTopResults[1].Name;
            //    option3.GetComponent<Text>().text = myTopResults[2].Name;



            //    //int j = 0;
            //    //Debug.Log("kutough");
            //    //Debug.Log(drawingPattern.Length.ToString()+" nanzo");

            //    //while (j<drawingPattern.Length) {
            //    //   Debug.Log(drawingPattern[i].X.ToString() + " " + drawingPattern[i].Y.ToString());
            //    //    j++;
            //    //}
            //}




        }


        //void OnCollisionEnter(Collision collision)
        //{
        //    //string zone= collider.gameobject.nmae
        //    Debug.Log(collision.gameObject.name);
        // //   firstLetter = collision.gameObject.name;
        //    //if (typing && first){
        //    //    firstLetter= collision.gameObject.name;
        //    //    first = false;
        //    //}

        //    // ContactPoint contact = collision.contacts[0];
        //    // Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //    //Vector3 position = contact.point;
        //    // Instantiate(explosionPrefab, position, rotation);
        //    //Destroy(gameObject);
        //    Debug.Log("first is " + firstLetter);

        //}


        private void OnTriggerEnter(Collider collision)
        {

            Debug.Log(collision.gameObject.name);
            lastCollision = collision.gameObject.name;

            //speed = speed * -1;
            //colorPicker = Random.Range(0, 10);
        }


        private void RecognizeGesture(Point[] points)
        {

            //empty 
            Debug.Log("first is " + firstLetter);

            first = true;
            myTopResults.Clear();

            var results = myAnalyzer.GetPointPatternMatchResults(points,firstLetter);

            if (!results.Any())
                // {
                //   myTopResults.Add(myempty);
                return;

            //}

            var topResult = results.First();

            myTopResults.Add(topResult);
            if (results.Length > 1)
                myTopResults.Add(results[1]);
            else
                myTopResults.Add(myempty);
            if (results.Length > 2)
                myTopResults.Add(results[2]);
            else
                myTopResults.Add(myempty);

       //  var res2=results.OrderByDescending(a=>a.Probability).ThenBy(a => a.Distance);
        
            Debug.Log("top");
            Debug.Log(topResult.Name);
            Debug.Log("prob");
            Debug.Log(topResult.Probability);
            int i = 0;
            foreach(PointPatternMatchResult result in results)
            {
                Debug.Log(result.Name+" distance "+result.Distance+" averageDISTANCE "+result.AverageDistance+" prob "+ result.Probability);
                //option1.GetComponents<>
                switch (i) {
                      case 0:
                        trascribedText.GetComponent<Text>().text = trascribedText.GetComponent<Text>().text+result.Name+" ";
                        option1.GetComponent<Text>().text = result.Name;
                        break;
                        case 1:
                        option2.GetComponent<Text>().text = result.Name;
                        break;
                       case 2:
                        option3.GetComponent<Text>().text = result.Name;
                        break;
                        case 3:
                        option4.GetComponent<Text>().text = result.Name;
                        break;
                        default:
                        break;
                }
                i = i + 1;
                if (i > 3)
                    break;
            }
            // var matchResults = $"Best Match: {topResult.Name}, Probability: {topResult.Probability:F2}%";

            //  StatusMessage.Text = matchResults;
        }
    }
}