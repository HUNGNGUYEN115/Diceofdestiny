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
   

    private Vector3[] facerandom =
     {
        new Vector3(-90,0,0), //face 1
        new Vector3(0,0,0),   //face 2
        new Vector3(0,0,-90), //face 3
        new Vector3(0,0,90),  //face 4
        new Vector3(180,0,0), //face 5
        new Vector3(90,0,0)   //face 6
    };



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        int initialface = Random.Range(0, facerandom.Length);
        Debug.Log("Initial Face: " + (initialface));
        transform.rotation = Quaternion.Euler(facerandom[initialface]);
        
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


