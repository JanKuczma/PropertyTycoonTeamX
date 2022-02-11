using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorchange : MonoBehaviour
{
    // Start is called before the first frame update
    bool selected;
    bool mouseOver;
    Color start_color;
    Renderer rend;
    void Start()
    {
        selected = false;
        mouseOver = false;
        rend = GetComponent<Renderer>();
        start_color = rend.materials[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("r") && selected)
        {
            rend.materials[1].SetColor("_Color",Color.red);
        }
        if(Input.GetKeyDown("t") && selected)
        {
            rend.materials[1].SetColor("_Color",Color.green);
        }
        if(Input.GetKeyDown("y") && selected)
        {
            rend.materials[1].SetColor("_Color",Color.blue);
        }
        if(Input.GetMouseButtonDown(0))
        {
            if(!selected && mouseOver)
            {
                selected = true;
                rend.materials[0].SetColor("_Color",new Color(start_color.r,start_color.g*2,start_color.b*2,start_color.a));
            }
            else if (selected && mouseOver)
            {
                selected = false;
                rend.materials[0].SetColor("_Color",start_color);
            }
            else if (selected && !mouseOver)
            {
                selected = false;
                rend.materials[0].SetColor("_Color",start_color);
            }
        } 
    }

    void OnMouseOver()
    {
        mouseOver = true;
    }
    void OnMouseExit()
    {
        mouseOver = false;
    }
}
