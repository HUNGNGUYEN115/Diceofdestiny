using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class Cards : MonoBehaviour
{
    public CardData cardData;
   

  


    // Feedback affect 
    public float moveUpDistance = 10f;
    public float moveDuration = 0.2f;
    private bool isAnimating = false;
    private Vector3 startPos;
    public Vector3 vfxoffset;
    public AudioSource audioSource;

    // Card states
    public GameObject SourcePrefab { get; set; }
    public bool IsFaceUp { get; private set; }
    public bool IsSelected { get; private set; }

    private void Start()
    {
       
       
       

    }

    public Tween Flip(bool faceUp)
    {
        if (IsFaceUp == faceUp) return null;

        IsFaceUp = faceUp;

        return transform
            
            .DORotate(new Vector3(0,  0f, faceUp ? 180f : 0f), 0.3f)
            .SetEase(Ease.OutQuad);
            

    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        IsSelected = false;
    }


 



    public void CardFeedback()
    {
       

        if (isAnimating) return;

       
        GameObject instance = Instantiate(cardData.vfx, transform.position + vfxoffset, Quaternion.identity);
        audioSource.PlayOneShot(cardData.audioClip);
        Destroy(instance, cardData.vfx.GetComponent<ParticleSystem>().main.duration);
        startPos = transform.position;
        StartCoroutine(MoveUp());



    }
    IEnumerator MoveUp()
    {
        isAnimating = true;
       
        Vector3 targetPos = startPos + Vector3.up * moveUpDistance;
        // move up
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        //  move back down
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            transform.position = Vector3.Lerp(targetPos, startPos, t);
            yield return null;
        }

        transform.position = startPos;
        isAnimating = false;
        

    }



}
