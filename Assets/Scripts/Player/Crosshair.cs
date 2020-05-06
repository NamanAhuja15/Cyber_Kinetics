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
    private Vector3 localPosition;
    private Vector3 position;
    void Start()
    {
        player = GetComponentInParent<PlayerHealth>();
      //  cam = player.gameObject.GetComponentInChildren<PlayerCamera>().gameObject;
        if (photonView.IsMine)
        {
            crosshair = player.crosshair;
        }

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
        else if(!photonView.IsMine)
        {
            transform.localPosition = localPosition;
            transform.position = position;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.position);
        }
        else
        {
            localPosition = (Vector3)stream.ReceiveNext();
            position = (Vector3)stream.ReceiveNext();
        }
    }
}
