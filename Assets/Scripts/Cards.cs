using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class Cards : MonoBehaviour
{
    public CardData cardData;

    // Visual components
    private MeshRenderer[] renderers;
    private Material[] runtimeMaterials;
    private Color[] originalColors;
    public int colorchange;

    // Feedback affect 
    public float moveUpDistance = 10f;
    public float moveDuration = 0.2f;
    private bool isAnimating = false;
    public bool isActivate = true;
    private Vector3 startPos;
    public Vector3 vfxoffset;
    public AudioSource audioSource;

    // Card states
    public GameObject SourcePrefab { get; set; }
    public bool IsFaceUp { get; private set; }
    public bool IsSelected { get; private set; }
    public bool IsReady { get; private set; }


    private void Awake()

    {
        renderers = GetComponentsInChildren<MeshRenderer>(true);

        runtimeMaterials = new Material[renderers.Length];
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            runtimeMaterials[i] = renderers[i].material;
            originalColors[i] = runtimeMaterials[i].color;
        }
        
    }
    private void Start()
    {

        colorchange = 40;


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
    public void Ready(bool value)
    {
        IsReady = value;
    }
    IEnumerator DectivateCard()
    {
        for (int i = 0; i < runtimeMaterials.Length; i++)
        {
            float t = 0f;
            Color start = runtimeMaterials[i].color;
            Color target = new Color(colorchange/255f, colorchange/255f, colorchange / 255f, start.a);

            while (t < 0.5f)
            {
                t += Time.deltaTime;
                runtimeMaterials[i].color = Color.Lerp(start, target, t);
                yield return null;
            }
            //runtimeMaterials[i].color = originalColors[i] * 0.6f;
            //runtimeMaterials[i].DisableKeyword("_EMISSION");
            
        }

    }






    public void CardFeedback()
    {
       

        if (isAnimating) return;

       if(!isActivate) return;
        if (!IsReady) return;
        GameObject instance = Instantiate(cardData.vfx, transform.position + vfxoffset, Quaternion.identity);
        audioSource.PlayOneShot(cardData.audioClip);
        Destroy(instance, cardData.vfx.GetComponent<ParticleSystem>().main.duration);
        startPos = transform.position;
        StartCoroutine(MoveUp());
        isActivate = false;



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
        //DectivateCard();
        StartCoroutine(DectivateCard());


    }



}
