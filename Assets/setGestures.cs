using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

namespace SwipeType.Example
{
    public class setGestures : MonoBehaviour
    {
        List<Point> setPattern = new List<Point>();
        Point point;

        string dir;
        string path;


        Vector3 q = new Vector3(-3.44f, 1.11f, 0.29f);
        Vector3 w = new Vector3(-2.81f, 1.11f, 0.29f);
        Vector3 e = new Vector3(-2.19f, 1.11f, 0.29f);
        Vector3 r = new Vector3(-1.58f, 1.11f, 0.29f);
        Vector3 t = new Vector3(-0.96f, 1.11f, 0.29f);
        Vector3 y = new Vector3(-0.36f, 1.11f, 0.29f);
        Vector3 u = new Vector3(0.29f, 1.11f, 0.29f);
        Vector3 i = new Vector3(0.89f, 1.11f, 0.29f);
        Vector3 o = new Vector3(1.53f, 1.11f, 0.29f);
        Vector3 p = new Vector3(2.09f, 1.11f, 0.29f);
        Vector3 a = new Vector3(-3.11f, 0.18f, 0.29f);
        Vector3 s = new Vector3(-2.48f, 0.18f, 0.29f);
        Vector3 d = new Vector3(-1.84f, 0.18f, 0.29f);
        Vector3 f = new Vector3(-1.25f, 0.18f, 0.29f);
        Vector3 g = new Vector3(-0.61f, 0.18f, 0.29f);
        Vector3 h = new Vector3(-0.02f, 0.18f, 0.29f);
        Vector3 j = new Vector3(0.63f, 0.18f, 0.29f);
        Vector3 k = new Vector3(1.21f, 0.18f, 0.29f);
        Vector3 l = new Vector3(1.84f, 0.18f, 0.29f);
        Vector3 z = new Vector3(-2.50f, -0.75f, 0.29f);
        Vector3 x = new Vector3(-1.87f, -0.75f, 0.29f);
        Vector3 c = new Vector3(-1.20f, -0.75f, 0.29f);
        Vector3 v = new Vector3(-0.63f, -0.75f, 0.29f);
        Vector3 b = new Vector3(-0.04f, -0.75f, 0.29f);
        Vector3 n = new Vector3(0.61f, -0.75f, 0.29f);
        Vector3 m = new Vector3(1.22f, -0.75f, 0.29f);
        Vector3 target;

        static string textFile = @"C:\Users\tjdub\Documents\wordlist.txt";
        string[] myStrings = File.ReadAllLines(textFile);

        char[] currentWord;
        string mycurrentWord;
        int currentl;
        int count = 0;
        int wordCounter = 0;
        bool start = false;
        //   int count = 0;
        private IEnumerator mycoroutine;
        // Use this for initialization
        void Start()
        {
            dir = @"C:\Users\tjdub\Documents\";//Directory.GetCurrentDirectory();
            path = Path.Combine(dir, "WordsFileTest.json");
            //  currentWord = null;
            //  target = transform.position;

            foreach(string s in myStrings)
            {
                Debug.Log(s);
            }
        }

        // Update is called once per frame
        void Update()
        {
            float step = 2.0f * Time.deltaTime;
            //   mycoroutine = move(p);

            //move(p);
            //    move2(p);


            // transform.position = Vector3.MoveTowards(transform.position, p, step);
            //transform.position = Vector3.MoveTowards(transform.position, t, step);
            // string mystring = currentWord;


            mycoroutine = move2();
            //char[] chars = mystring.ToCharArray();
            // int l = chars.Length;


            //  transform.position = h;
            if (!start)
            {
                getWord();
                //target = p;
                start = true;
                count = 0;
                findTarget(count);
                //  yield return new WaitForSeconds(5);
                // StopAllCoroutines();
                // StartCoroutine(mycoroutine);
                transform.position = target;

            }

            // Debug.Log("Current word is " + currentWord[count]+ target);

            transform.position = Vector3.MoveTowards(transform.position, target, step);

            Debug.Log(transform.position + " and " + target);
            Debug.Log(Vector3.Distance(transform.position, target));


            //set points
            point.X = transform.position.x; //xpos;// mycursor.transform.position.x;
            point.Y = transform.position.y;// 
            setPattern.Add(point);


            if ((Vector3.Distance(transform.position, target) < 0.2f))
            {
                // Swap the position of the cylinder.
                //x.transform.position *= -1.0f;
                //   yield return new WaitForSeconds(5);
                Debug.Log("h");
                //switch (mystring[count])
                //{
                //    case 'h':
                //        target = h;
                //        //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                //        Debug.Log("h");
                //        break;
                //    case 'i':
                //        target = i;
                //        Debug.Log("i");
                //        break;
                //    case 'e':
                //        target = e;
                //        Debug.Log("e");
                //        break;
                //    default:
                //        break;
                //}


                if (count > currentWord.Length - 1)
                {
                    start = false;
                    Point[] mypoints = new Point[setPattern.Count];

                    mypoints = setPattern.ToArray();

                    PointPattern pa = new PointPattern();

                    pa.Name = mycurrentWord;
                    pa.Points = mypoints;


                    setPattern.Clear();
                    PointManager.AddInput(path, pa);

                }
                if (start)
                    findTarget(count);
                count++;
            }

            //if (count >= currentWord.Length)
            //{
            //    start = false;
            //}
            // mycoroutine = move(chars, count, l);
            //StartCoroutine(mycoroutine);

            //   
            //foreach (char c in mystring)
            //{
            //    switch (c) {
            //        case 'h':
            //            target = h;
            //            //  transform.position = Vector3.MoveTowards(transform.position, h, step);
            //            Debug.Log("h");
            //            break;
            //        case 'i':
            //            target = i;
            //            Debug.Log("i");
            //            break;
            //        case 'e':
            //            target = e;
            //            Debug.Log("e");
            //            break;
            //        default:
            //            break;

            //                }
            //    move(target);

            //}
        }

        IEnumerator move(char[] mystring, int count, int l)
        {



            switch (mystring[count])
            {

                case 'a':
                    target = a;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("a");
                    break;
                case 'h':
                    target = h;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("h");
                    break;
                case 'i':
                    target = i;
                    Debug.Log("i");
                    break;
                case 'e':
                    target = e;
                    Debug.Log("e");
                    break;
                default:
                    break;
            }


            float step = 1.0f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (count < l - 1)
            {
                count = count + 1;
                yield return new WaitForSeconds(2);
                move(mystring, count, l);
            }


            //if (count <l-1)
            //{
            //    mycoroutine = move(mystring,count+1,l);
            //    StartCoroutine(mycoroutine);
            //}
            //if (Vector3.Distance(transform.position, target) < 0.001f)
            ////{
            ////    // Swap the position of the cylinder.
            ////    //x.transform.position *= -1.0f;
            ////    yield return new WaitForSeconds(5);
            ////    Debug.Log("h");
            ////}
        }

        IEnumerator move2()
        {
            //Debug.Log("eee");

            yield return new WaitForSeconds(10);
        }

        void findTarget(int count)
        {


            switch (currentWord[count])
            {

                case 'a':
                    target = a;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("a");
                    break;
                case 'b':
                    target = b;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("b");
                    break;

                case 'c':
                    target = a;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("c");
                    break;
                case 'd':
                    target = d;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("d");
                    break;
                case 'e':
                    target = e;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("e");
                    break;
                case 'f':
                    target = f;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("f");
                    break;
                case 'g':
                    target = g;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("g");
                    break;
                case 'h':
                    target = h;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("h");
                    break;
                case 'i':
                    target = i;
                    Debug.Log("i");
                    break;
                case 'j':
                    target = j;
                    Debug.Log("j");
                    break;
                case 'k':
                    target = k;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("k");
                    break;
                case 'l':
                    target = l;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("l");
                    break;
                case 'm':
                    target = m;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("m");
                    break;
                case 'n':
                    target = n;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("n");
                    break;
                case 'o':
                    target = o;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("o");
                    break;
                case 'p':
                    target = p;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("p");
                    break;
                case 'q':
                    target = q;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("q");
                    break;
                case 'r':
                    target = r;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("r");
                    break;
                case 's':
                    target = s;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("s");
                    break;
                case 't':
                    target = t;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("t");
                    break;
                case 'u':
                    target = u;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("u");
                    break;
                case 'v':
                    target = v;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("v");
                    break;
                case 'w':
                    target = w;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("w");
                    break;
                case 'x':
                    target = x;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("x");
                    break;
                case 'y':
                    target = y;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("y");
                    break;
                case 'z':
                    target = z;
                    //  transform.position = Vector3.MoveTowards(transform.position, h, step);
                    Debug.Log("z");
                    break;
                default:
                    break;
            }

        }

        void getWord()
        {

            string[] words = { "the", "of", "and", "to", "a", "in", "for" };
            string word = myStrings[wordCounter];//words[wordCounter]; 
            mycurrentWord = word;
            currentWord = word.ToCharArray();
            //char[] chars = mystring.ToCharArray();
            //currentl = chars.Length;
            wordCounter++;

            //char[] myword

        }
    }
}