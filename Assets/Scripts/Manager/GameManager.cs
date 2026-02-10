using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RollDice dice;

    PointController pointController;
    public GameObject choosepanel;
    public GameObject textfirst;
    public GameObject textsecond;
    public GameObject endpanel;
    public TMP_Text nametext;
    public TMP_Text scoretext;

    private bool isPlayedsound = true;


    private InputAction Delete;
    public PlayerInputSystem controls;

    public AudioSource audioSource;
    public AudioClip endgameclip;
    private void Awake()
    {
        controls = new PlayerInputSystem();
    }
    void Start()
    {
        dice = FindAnyObjectByType<RollDice>();
        pointController = FindAnyObjectByType<PointController>();
        textsecond.SetActive(false);
        textfirst.SetActive(true);
        choosepanel.SetActive(true);
        dice.Isendgame = false;

       

    }

    // Update is called once per frame
    void Update()
    {
        if (pointController.turn == 6)
        {
            dice.Isendgame = true;
            if ((!dice.IsCountingAnimation && dice.Isrolled && !dice.Iscount) || (!dice.IsCountingAnimation && !dice.Isrolled && !dice.Iscount))
            {
                StartCoroutine(EndGame());
                Debug.Log("End");
                if (UserData.instance.PlayerName!=null)
                {
                    nametext.text = "Nice roll " + UserData.instance.PlayerName + "!!!";
                }
                else
                {
                    nametext.text = "Nice roll xxx !!!";
                }
                scoretext.text = pointController.calculator.finalpoint.ToString();

            }
        }
    }
    public void OpenChoosePanel()
    {
        choosepanel.SetActive(true);
        textsecond.SetActive(true);

    }
    public void CloseChoosePanel()
    {
        choosepanel.SetActive(false);
        textfirst.SetActive(false);

    }
    public void OpenEndPanel()
    {   
        choosepanel.SetActive(true);
        endpanel.SetActive(true);
        textfirst.SetActive(false);
        textsecond.SetActive(false);
        PlayMusic();



    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        OpenEndPanel();
    }
    public void PlayMusic()
            {
        if (isPlayedsound)
            {
                    
                    audioSource.volume = 0.8f;
            audioSource.PlayOneShot(endgameclip);
                    
            
            
        }
        isPlayedsound = false;
    }
    private void OnEnable()
    {
        Delete = controls.Player.Delete;
        if (Delete != null)
        {
            Delete.Enable();
            Delete.performed += OnDeletePressed;
        }
        else
        {
            Debug.LogWarning("Delete action not found in Input Actions.");
        }
    }


    private void OnDisable()
    {
        if (Delete != null)
        {
            Delete.performed -= OnDeletePressed;
            Delete.Disable();
        }
    }


    public void OnDeletePressed(InputAction.CallbackContext context)
    {
        HighScoreManager.ResetHighScores();
        Debug.Log("High scores reset.");
    }
}
