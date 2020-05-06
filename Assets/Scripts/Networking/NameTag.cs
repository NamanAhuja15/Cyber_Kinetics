using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviourPunCallbacks {

  
    [SerializeField]
    private Text nameText;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Awake()
    {
        
    }
    void Start() {

        if (photonView.IsMine) {
           photonView.RPC("SetName", RpcTarget.All, PhotonNetwork.NickName);
        } else {
            SetName(photonView.Owner.NickName);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update() {
    }

    /// <summary>
    /// RPC function to set player name tag.
    /// </summary>
    [PunRPC]
    void SetName(string name) {
        nameText.text = name;
    }

}
