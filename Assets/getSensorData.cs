using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports;
using System;
using System.IO;
using System.Text;

public class getSensorData : MonoBehaviour {

   SerialPort stream = new SerialPort("COM7", 38400);
    static string f;
    string filename;

    Vector3 acceleration;
    Vector3 acceleration2;
    Vector3 gyro;
    Vector3 angles;

    Vector3 ac;
    Vector3 gy;
    Vector3 an;
    Quaternion qn; 

    Quaternion q;

    Vector3 newac;

    float[] rotationMatrix2=new float[9];
    float[] rotationResult= new float[3];
    float[] accA= new float[3];
    float[] qA;

    public GameObject sphere;
    Vector3 displacement;
    Vector3 v=  Vector3.zero;

    float prevt;
    float firstt;
    


    string[] stringDelimitersFSR = new string[] {":",};
    string[] stringDelimitersAC = new string[] { ":",",", };
    string[] stringDelimitersGY = new string[] { ":", ",", };
    string[] stringDelimitersAN = new string[] { ":", ",", };
    float force;
    float lastforce;
    // Use this for initialization
    void Start () {
        
        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        f = string.Format("fsr_"+unixTimestamp);
        filename = @"C:\Users\tjdub\Documents\FSR_MPU9250\" + f + ".tsv";



        string headings = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t\r\n", "FSR", "Force", "AC", "GYRO", "ANGLES", "P0S","QUAT");

        using (FileStream fs = File.Create(filename))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(headings);
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }






        stream.ReadTimeout = 50;
        stream.Open();

    }
	
	// Update is called once per frame
	void Update () {

        string cmd = CheckForRecievedData();

        try
        {
           // print(stream.ReadLine());

            if (cmd.StartsWith("F")) //Got a force
            {
                //print("hey");
                force = ParseFSRData(cmd);
                print(force);

                if (force > 0)
                {


                    //newac 

                    //            prevt = ct;
                    //          ct =  DateTime.Now;

                    accA[0] = acceleration.x;
                    accA[1] = acceleration.y;
                    accA[2] = acceleration.z;



                    //qA[0] = q.x;
                    //qA[1] = q.y;
                    //qA[2] = q.z;
                    //qA[3] = q.w;



                    rotationMatrix2 = getRotationMatrixFromQuaternion(q);
                    rotationResult = matrixMultiplication(accA, rotationMatrix2);
                    acceleration.x = rotationResult[0];
                    acceleration.y = rotationResult[1];
                    acceleration.z = rotationResult[2];

                    Debug.Log(acceleration.ToString());

                    calculateVel(acceleration,0.01f);


                  //  sphere.transform.Translate(displacement);
                  Debug.Log("ediaaaaaaaaaaa");
                    writeLog();
                    try { 
                        sphere.transform.Translate(displacement);
                }
        catch (System.Exception)
        {
        }

    }
                //Vector3 accl = ParseFSRData(cmd);
                ////Smoothly rotate to the new rotation position.
                //target.transform.rotation = Quaternion.Slerp(target.transform.rotation, Quaternion.Euler(accl), Time.deltaTime * 2f);
            }
            else if (cmd.StartsWith("AC")){ acceleration = ParseACData(cmd); }

            else if (cmd.StartsWith("GY")){ gyro = ParseGyroData(cmd); }

            else if (cmd.StartsWith("AN")){ angles = ParseAngleData(cmd); }

            else if (cmd.StartsWith("QU")) { q = ParseQuatData(cmd); }
        }
        catch (System.Exception)
        {
        }


        
       

    }


    public string CheckForRecievedData()
    {
        try //Sometimes malformed serial commands come through. We can ignore these with a try/catch.
        {
            string inData = stream.ReadLine();
            int inSize = inData.Length;
            if (inSize > 0)
            {
                int a =1 + 1;
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


    Vector3 ParseACData(string data) {

        int count = 5;
        //string[] splitResult = data.Split(stringDelimitersFSR, count, StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("hebooo");
     //   Debug.Log(splitResult[1]);
     //   Debug.Log(splitResult[2]);

        try
        {
         string[] splitResult = data.Split(stringDelimitersFSR, count, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log(splitResult[1]);
            // Debug.Log(splitResult[2]);
            //Debug.Log(splitResult[3]);
            //   Debug.Log(splitResult[2]);
            //  ac = splitResult[1];
            float x = float.Parse(splitResult[1]);
           float y = float.Parse(splitResult[2]);
           float z = float.Parse(splitResult[3]);
            ac = new Vector3(x,y,z);

            Debug.Log("heiiiita");
      
            //Debug.Log(ac.x);
            //Debug.Log(ac.y);
            //Debug.Log(ac.z);
            //int y = int.Parse(splitResult[1]);
            //int z = int.Parse(splitResult[2]);
            //force = f; new Vector3(x, y, z);
            //   lastforce = f;
            return ac;
        }
        catch { Debug.Log("Malformed Serial Transmisison"); return ac; }

        }


     Vector3 ParseGyroData(string data)
    {
        Debug.Log("gyro");

        int count = 5;
        try
        {
            string[] splitResult = data.Split(stringDelimitersGY, count, StringSplitOptions.RemoveEmptyEntries);
            //gy = splitResult[1];

            float x = float.Parse(splitResult[1]);
            float y = float.Parse(splitResult[2]);
            float z = float.Parse(splitResult[3]);
            gy = new Vector3(x, y, z);

            //int y = int.Parse(splitResult[1]);
            //int z = int.Parse(splitResult[2]);
            //force = f; new Vector3(x, y, z);
            //   lastforce = f;
            return gy;
        }
        catch { Debug.Log("Malformed Serial Transmisison"); return gy; }

    }


   Vector3 ParseAngleData(string data)
    {

        Debug.Log("angle");

        int count = 5;
        try
        {
            string[] splitResult = data.Split(stringDelimitersAN, count, StringSplitOptions.RemoveEmptyEntries);
            // an = splitResult[1];

            float x = float.Parse(splitResult[1]);
            float y = float.Parse(splitResult[2]);
            float z = float.Parse(splitResult[3]);
            an = new Vector3(x, y, z);
            //int y = int.Parse(splitResult[1]);
            //int z = int.Parse(splitResult[2]);
            //force = f; new Vector3(x, y, z);
            //   lastforce = f;
            return an;
        }
        catch { Debug.Log("Malformed Serial Transmisison"); return an; }

    }


    Quaternion ParseQuatData(string data)
    {

        Debug.Log("qua");

        int count = 6;
        try
        {
            string[] splitResult = data.Split(stringDelimitersAN, count, StringSplitOptions.RemoveEmptyEntries);
            // an = splitResult[1];

            float x = float.Parse(splitResult[1]);
            float y = float.Parse(splitResult[2]);
            float z = float.Parse(splitResult[3]);
            float w = float.Parse(splitResult[4]);
            qn = new Quaternion(x, y, z, w);
            //int y = int.Parse(splitResult[1]);
            //int z = int.Parse(splitResult[2]);
            //force = f; new Vector3(x, y, z);
            //   lastforce = f;

          //  newac = qn.Euler(ac);
            return qn;

            
        }
        catch { Debug.Log("Malformed Serial Transmisison"); return qn; }

    }


    //log file
    void writeLog()
    {

        string metrics;

        using (StreamWriter w = File.AppendText(filename))
        {
            metrics = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t\r\n",
                         "fsr", force,acceleration2.ToString("F4"), gyro.ToString(), angles.ToString(), displacement.ToString(), q.ToString());
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



    void calculateVel(Vector3 acc, float t)
    {
       
        float deltat = 0.01f;//Time.   t0.5f;
        Vector3 g = new Vector3(9.8f, 9.8f, 9.8f);
        acc *= 9.8f; //convert to m/s2

        acc.x -= 9.8f;
        acc.y -= 9.8f;

        acceleration2 = ac;

        Vector3 initV = Vector3.zero;

        v.x += (acc.x) * deltat;
        v.y += (acc.y) * deltat;
        v.z += 0f;// acc.x * deltat;

        displacement = calculatePos(v, 0.5f);
        //     acc = acc * 9.8f;
        //  acc = acc.Scale(g); 


    }

    Vector3 calculatePos(Vector3 v, float t)
    {
        float deltat = 0.01f;
        Vector3 pos;

        pos.x = v.x * deltat;

        pos.y = v.y * deltat;

        pos.z = v.z * deltat;
        return pos;
    }


    private float[] getRotationMatrixFromQuaternion(Quaternion q22)
    {
        //
        float[] q = new float[4];
        float[] result = new float[9];
        q[0] = q22.w;
        q[1] = q22.x;
        q[2] = q22.y;
        q[3] = q22.z;

        result[0] = q[0] * q[0] + q[1] * q[1] - q[2] * q[2] - q[3] * q[3];
        result[1] = 2 * (q[1] * q[2] - q[0] * q[3]);
        result[2] = 2 * (q[1] * q[3] + q[0] * q[2]);

        result[3] = 2 * (q[1] * q[2] + q[0] * q[3]);
        result[4] = q[0] * q[0] - q[1] * q[1] + q[2] * q[2] - q[3] * q[3];
        result[5] = 2 * (q[2] * q[3] - q[0] * q[1]);

        result[7] = 2 * (q[2] * q[3] + q[0] * q[1]);
        result[6] = 2 * (q[1] * q[3] - q[0] * q[2]);
        result[8] = q[0] * q[0] - q[1] * q[1] - q[2] * q[2] + q[3] * q[3];

        return result;
    }

    private float[] matrixMultiplication(float[] A, float[] B)
    {
        float[] result = new float[3];

        result[0] = A[0] * B[0] + A[1] * B[1] + A[2] * B[2];
        result[1] = A[0] * B[3] + A[1] * B[4] + A[2] * B[5];
        result[2] = A[0] * B[6] + A[1] * B[7] + A[2] * B[8];

        return result;
    }


}
