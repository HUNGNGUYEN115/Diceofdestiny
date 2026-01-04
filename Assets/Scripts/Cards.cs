using UnityEngine;
using TMPro;
using System.Collections;
public class Cards : MonoBehaviour
{
    public CardData cardData;
   

  
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI descriptionText;

    // Feedback affect 
    public float moveUpDistance = 10f;
    public float moveDuration = 0.2f;
    private bool isAnimating = false;
    private Vector3 startPos;
    public Vector3 vfxoffset;
    public AudioSource audioSource;



    private void Start()
    {
        UpdateCardData();
        startPos = transform.position;

    }
    void Update()
    {
 
    }
    public void UpdateCardData()
    {
        cardNameText.text = cardData.cardName;
        descriptionText.text = cardData.description;
    }
    public void CardFeedback()
    {
       

        if (isAnimating) return;

       
        GameObject instance = Instantiate(cardData.vfx, transform.position + vfxoffset, Quaternion.identity);
        audioSource.PlayOneShot(cardData.audioClip);
        Destroy(instance, cardData.vfx.GetComponent<ParticleSystem>().main.duration);
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
