using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
   public Image crosshair;
    private Ray ray;
    private RaycastHit hit;
    void Start()
    {
        crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crosshair)
        {
            ray = Camera.main.ScreenPointToRay(crosshair.transform.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject)
                {
                    transform.position = hit.point;
                }
            }
        }
    }
}
