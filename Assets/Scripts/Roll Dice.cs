using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class RollDice : MonoBehaviour
{
    Rigidbody rb;
  
    PointController pointcontroller;

    public float maxrandomforcevalue;
    public float rollingforce;
    private float forcex,forcey, forcez;
    public int facenum;
    public bool Iscount = false;
    public bool Isrolled;
    public bool IsCountingAnimation;
    public bool Isendgame;
    public bool Ishandingcard;

    public CardManager cardManager;

    public AudioSource audioSource;
    public AudioClip dicesound;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f),Random.Range(0f, 360f),Random.Range(0f, 360f) );
        Isrolled =false;
       
        pointcontroller = FindAnyObjectByType<PointController>();
        Ishandingcard = true;
    }
    public void RollTheDice()
    {   // Prevent new rolls while counting handing cards or end the game
        if (IsCountingAnimation) return;
        if(Isendgame) return;
        if (Ishandingcard) return;
        //Point.instance.ResetUI();
        pointcontroller.turn++;
        pointcontroller.ResetAll();
        HandManager.Instance.ReadyCard(true);
        Iscount = true;
        //disable rolling if the dice is still moving
        if (rb.linearVelocity.magnitude > 0.1f ||rb.angularVelocity.magnitude > 0.1f)
        {
            
            return;
        }
       
        rb.isKinematic = false;
        // Apply random torque and force
        forcex = Random.Range(0, maxrandomforcevalue);
        forcey = Random.Range(0, maxrandomforcevalue);
        forcez = Random.Range(0, maxrandomforcevalue);
        rb.AddForce(Vector3.up * rollingforce);
        rb.AddTorque(forcex, forcey, forcez);
        
        Isrolled = true;
        Ishandingcard = true;
        







    }

    void Update()
    {
        
        // Dice is rolling  do nothing
        if (!Isrolled) return;
       
        if (Isrolled)
        {

            cardManager.Checkcard();


        }
       

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("plane"))
        {
            audioSource.PlayOneShot(dicesound);
        }
    }

}


