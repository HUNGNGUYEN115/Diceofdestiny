using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class FaceDetector : MonoBehaviour
{
    RollDice dice;
    //Conditions for checking when the dice is stopped
    private const float Threshold = 0.05f;
    PointController point;
    //UI
    
    public TextMeshProUGUI dicenumtext;
    //Audio
    public AudioSource audioSource;
    public AudioClip clicksound;

    void Start()
    {
        dice=FindAnyObjectByType<RollDice>();
        point=FindAnyObjectByType<PointController>();
    }

   
    private void OnTriggerStay(Collider other)
    {
        
        if (dice!=null && dice.Iscount)
        {
            //Check both spinning and movíng of the dice
            if (dice.GetComponent<Rigidbody>().linearVelocity.sqrMagnitude < Threshold * Threshold &&
            dice.GetComponent<Rigidbody>().angularVelocity.sqrMagnitude < Threshold * Threshold)
            {
                dice.facenum=int.Parse(other.gameObject.name);
               dicenumtext.gameObject.SetActive(true);
                audioSource.PlayOneShot(clicksound);
                dicenumtext.text=  dice.facenum.ToString();
                dicenumtext.transform.localPosition = dice.transform.position + new Vector3(0, dice.transform.position.z + 0.5f, -dice.transform.position.z);
                dicenumtext.transform.rotation = Quaternion.Euler(80, 0,    0);
                LeanTween.moveLocalZ(dicenumtext.gameObject,-0.4f,1f).setDelay(0.2f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(()=>
                {
                    dicenumtext.gameObject.SetActive(false);
                });

                //Destroy(dicenumtext.gameObject, 1.5f);
                point.CheckFace(dice.facenum);

                dice.Iscount = false;
            }
        }
    }
   
}
