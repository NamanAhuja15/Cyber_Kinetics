using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class PlayerHealth : MonoBehaviourPunCallbacks, IPunObservable
{

    public delegate void Respawn(float time);
    public delegate void AddMessage(string Message);
    public event Respawn RespawnEvent;
    public event AddMessage AddMessageEvent;

    [SerializeField]
    private int startingHealth = 100;
    [SerializeField]
    private float respawnTime = 8.0f;
    private AudioManager playerAudio;
    [SerializeField]
    private float flashSpeed = 2f;
    [SerializeField]
    private Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [SerializeField]
    private NameTag nameTag;
 
    private Animator animator;
    private IKControl ikControl;
    private Slider healthSlider;
    private GameObject Screen,Controls;
    private Image damageImage;
    private int currentHealth;
    private bool isDead;
    private bool damaged;
    public Image crosshair,weapon_img;
    private Text gun_name, ammo, reloadtext,inGun,capacity;
    public GunScript gun;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentHealth = startingHealth;
        Screen = GameObject.FindGameObjectWithTag("Screen");
        SetUI();
        animator = GetComponent<Animator>();
        ikControl = GetComponent<IKControl>();
        playerAudio = GetComponent<AudioManager>();
        isDead = false;
        Cursor.visible = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void SetUI()
    {
        if (photonView.IsMine)
        {
            crosshair = Screen.transform.Find("Crosshair").GetComponent<Image>();
            healthSlider = Screen.GetComponentInChildren<Slider>();
            gun_name = Screen.transform.Find("GunName").GetComponent<Text>();
            ammo = Screen.transform.Find("Ammo").GetComponent<Text>();
            reloadtext = Screen.transform.Find("ReloadText").GetComponent<Text>();
            weapon_img = Screen.transform.Find("GunImage").GetComponent<Image>();
            inGun = Screen.transform.Find("InGun").GetComponent<Text>();
            capacity = Screen.transform.Find("Capacity").GetComponent<Text>();
            Controls = Screen.transform.Find("Controls").gameObject;
            healthSlider.value = currentHealth;
            healthSlider.GetComponentInChildren<Text>().text = currentHealth.ToString() + "/100";
        }
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Controls.activeInHierarchy)
                {
                    Controls.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Controls.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            if (gun != null && photonView.IsMine)
            {
                weapon_img.enabled = true;
                weapon_img.sprite = gun.Icon;
                gun_name.text = gun.Gun_Name;
                if (gun.weapon == GunScript.WeaponType.Rifle)
                {
                    ammo.text = gun.bulletsInTheGun.ToString() + " / " + gun.total_bullets.ToString();
                    inGun.text = "Bullets";
                    capacity.text = "Capacity";
                    reloadtext.text = "Reloading bullets";
                    if (gun.reloading)
                        reloadtext.enabled = true;
                    else
                        reloadtext.enabled = false;
                }
                else if (gun.weapon == GunScript.WeaponType.Beam)
                {
                    int heat = (int)gun.beamHeat;
                    ammo.text = heat.ToString() + " / " + gun.maxBeamHeat.ToString();
                    inGun.text = "Heat";
                    capacity.text = "MaxHeat";
                    reloadtext.text = "Cooling Down";
                    if (gun.coolingDown)
                        reloadtext.enabled = true;
                    else
                        reloadtext.enabled = false;
                }
            }
            else
                weapon_img.enabled = false;
        }
    }

    /// <summary>
    /// RPC function to let the player take damage.
    /// </summary>
    /// <param name="amount">Amount of damage dealt.</param>
    /// <param name="enemyName">Enemy's name who cause this player's death.</param>
    [PunRPC]
    public void TakeDamage(int amount, string enemyName)
    {
        if (isDead) return;
        if (photonView.IsMine&&gun.owner!=enemyName)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                photonView.RPC("Death", RpcTarget.All, enemyName);
            }
            healthSlider.value = currentHealth;
            healthSlider.GetComponentInChildren<Text>().text = currentHealth.ToString() + "/100";
            animator.SetTrigger("IsHurt");
        }
        playerAudio.Hurt();
    }

    /// <summary>
    /// RPC function to declare death of player.
    /// </summary>
    /// <param name="enemyName">Enemy's name who cause this player's death.</param>
    [PunRPC]
    void Death(string enemyName)
    {
        isDead = true;
        ikControl.enabled = false;
        nameTag.gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            animator.SetTrigger("IsDead");
            AddMessageEvent(PhotonNetwork.LocalPlayer.NickName + " was killed by " + enemyName + "!");
            RespawnEvent(respawnTime);
            StartCoroutine("DestoryPlayer", respawnTime);
        }
        playerAudio.Hurt();
    }

    /// <summary>
    /// Coroutine function to destory player game object.
    /// </summary>
    /// <param name="delayTime">Delay time before destory.</param>
    IEnumerator DestoryPlayer(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PhotonNetwork.Destroy(gameObject);
    }


    /// <summary>
    /// Used to customize synchronization of variables in a script watched by a photon network view.
    /// </summary>
    /// <param name="stream">The network bit stream.</param>
    /// <param name="info">The network message information.</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }

}
