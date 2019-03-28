
using System.Collections;
using System.Collections.Generic;
//using System.IO.Ports;
using UnityEngine;

public class Block : MonoBehaviour
{

    //SerialPort sp = new SerialPort("COM7", 115200);
    public event System.Action<Block> OnBlockPressed;
    public Vector2Int coord;


    private void Start()
    {
        //sp.Open();

        //if (sp.IsOpen)
            //Debug.Log("Connected1");
        //else
            //Debug.Log("Not Connected");
    }

    public void Init(Vector2Int startingCoord, Texture2D image)
    {
        coord = startingCoord;

        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    void OnMouseDown()
    {
             
        OnBlockPressed(this);
        
    }

    void OnTriggerEnter(Collider col)
    {
        //if (!sp.IsOpen)
        //{
            //sp.Open();
        //}
        //Debug.Log(col.gameObject.name);
        //sp.Write(col.gameObject.name);

        OnBlockPressed(this);
        //sp.Close();
    }

}

