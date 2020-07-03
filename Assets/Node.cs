using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IEnumerable
{

    //	// Use this for initialization
    //	void Start () {

    //	}

    //	// Update is called once per frame
    //	void Update () {

    //	}
    //}

        public Node parent { get; set; }
         public int Value { get; set; }

    public Node(int value)
    {
        Value = value;
    }

    List<Node> children = new List<Node>();

    public void add(int zone)
    {
        var childNode = new Node(zone);
        children.Add(childNode);


    }

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)children).GetEnumerator();
    }





    //public IEnumerator<T> GetEnumerator()
    //{
    //    throw new System.NotImplementedException();
    //}

    //IEnumerator IEnumerable.GetEnumerator()
    //{
    //    throw new System.NotImplementedException();
    //}
}
