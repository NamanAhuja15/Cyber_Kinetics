using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
   public GameObject crosshair;
    void Start()
    {
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    // Update is called once per frame
    void Update()
    {
        if(crosshair)
        crosshair.transform.position = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
    }
}
