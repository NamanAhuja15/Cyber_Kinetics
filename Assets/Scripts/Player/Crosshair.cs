using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Crosshair :  MonoBehaviourPunCallbacks,IPunObservable
{
    // Start is called before the first frame update
   public Image crosshair;
    private PlayerHealth player;
    private Ray ray;
    private RaycastHit hit;
    [SerializeField]
    private GameObject cam;
    private Vector3 offset;
    private Vector3 position;
    private Quaternion rotation;
    void Start()
    {
        player = GetComponentInParent<PlayerHealth>();
        cam = player.gameObject.GetComponentInChildren<PlayerCamera>().gameObject;
        if (photonView.IsMine)
        {
            crosshair = player.crosshair;
        }
        transform.parent = cam.transform;
        offset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
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
        else
        {
            transform.localRotation = rotation;
            transform.localPosition = position;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
