using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    RollDice dice;
    //Conditions for checking when the dice is stopped
    private const float Threshold = 0.05f;
    PointController point;

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
                point.CheckFace(dice.facenum);

                dice.Iscount = false;
            }
        }
    }
   
}
