using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public static CameraControl instance;

    private Vector2 velocity;
    private AudioSource audioS;

    public float smoothTimeX = 0.05f;

    private GameObject player; //ref to the player object in the scene
    private Vector3 distance; //distance between camera and player
    private bool playerGot = false;   //check if player is available

    [HideInInspector]
    public managerVars vars;

    void OnEnable()
    {
        vars = Resources.Load<managerVars>("managerVarsContainer");
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.clip = vars.backgroundMusic;
    }

    void Update()
    {
        if (GameManager.instance.gameOver == false)
        {
            if (!audioS.isPlaying)
            {
                audioS.Play();
            }
        }
        else if (GameManager.instance.gameOver == true)
        {
            if (audioS.isPlaying)
            {
                audioS.Stop();
            }
        }
    }

    //method which will be called by player spawns
    public void PlayerSettings()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //gets the distance between player and camera , we need this distance so to maintain it in the game
        distance = (player.transform.position - transform.position);
        playerGot = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (player == null || GameManager.instance.gameOver == true)
            return;

        Movement();
    }

    void Movement()
    {   //decide the x value
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x- distance.x, ref velocity.x, smoothTimeX);
        //set the x value of camera
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);

    }

}
