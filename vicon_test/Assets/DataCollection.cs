using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataCollection
{

    public class VisualHapticPair
    {
        public int v;
        public int h;

        public VisualHapticPair(int _v, int _h)
        {
            v = _v;
            h = _h;
        }

        public override string ToString()
        {
            return v.ToString()+" "+h.ToString();
        }
    }

    private VisualHapticPair[] dataArray;
    private int[] resArray;
    private int varV, varH, repeat;
    private int testNum;
    private int idx;
    private string filename;
    private StreamWriter streamWriter;

    public DataCollection()
    {
        varV = 5;
        varH = 5;
        repeat = 10;
        testNum = varV * varH * repeat;
        dataArray = new VisualHapticPair[testNum];
        resArray = new int[testNum];
        idx = 0;
        // create array
        for(int i = 0; i < varV; i++) 
        {
            for (int j = 0; j < varH; j++)
            {
                for (int k = 0; k < repeat; k++)
                {
                    dataArray[idx] = new VisualHapticPair(i, j);
                    idx++;
                }
            }
        }
        idx = 0;
        //shuffle
        for (int i = 0; i < testNum-1; i++)
        {
            int temp = Random.Range(i, testNum);
            (dataArray[i], dataArray[temp]) = (dataArray[temp], dataArray[i]);
        }

        filename = "Data/" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        streamWriter = new StreamWriter(filename);
    }

    public VisualHapticPair GetNextTrial()
    {
        if (idx >= testNum)
        {
            Debug.Log("Test Finished!");
            return null;
        }
        else
        {
            Debug.Log("Trial "+idx.ToString());
            return dataArray[idx];
        }
    }

    public void ResultUpdate(int res)
    {
        resArray[idx] = res;
        streamWriter.WriteLine(dataArray[idx].ToString() + " " + resArray[idx].ToString());
        idx++;
    }

    public void SaveToFile()
    {
        //string filename = "Data/"+ System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        Debug.Log("save data to file = " + filename);
        /*
        StreamWriter streamWriter = new StreamWriter(filename);
        for (int i = 0; i < testNum; i++)
        {
            streamWriter.WriteLine(dataArray[i].ToString()+" "+resArray[i].ToString());
        }
        */
        streamWriter.Close();
    }
}
