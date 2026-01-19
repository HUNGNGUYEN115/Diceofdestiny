using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using System.Diagnostics.CodeAnalysis;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int MaxHandSize;
    [SerializeField] private List<GameObject> cardPrefabs;
    [SerializeField] private SplineContainer splinecontainer;
    [SerializeField] private Transform cardspawnppoint;

    public List<GameObject> handCards = new();
    [SerializeField] private List<Transform> targetPositions; // size = 2
    [SerializeField] private float moveDuration = 1f;

    public List<Cards> selectedCards = new();
    public static HandManager Instance;

    public RollDice dice;
    public bool ischoosecard;


    void Start()
    {
        StartCoroutine(DrawCardsRoutine(MaxHandSize));
        ischoosecard=true;
         
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
        
    }

    void HandleClick()
    {
        /*if (selectedCards.Count >= 2) return*/;

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Cards card = hit.collider.GetComponent<Cards>();
            if (card == null || card.IsSelected) return;
            if (dice.turn == 3)
            {
                SelectCard(card, 3);
            }
            else if(dice.turn == 0)
            {
                SelectCard(card, 2);
            }
        }
    }
 
    void SelectCard(Cards card,int turn)
    {
        card.transform.DOMoveY(card.transform.position.y + 1.5f, 0.15f);

        card.Select();
        card.Flip(true);
        
        selectedCards.Add(card);
       
        Tween flipTween = card.Flip(true);

        if (selectedCards.Count == turn)
        {
            

            
            Invoke(nameof(ResolveSelection), 0.6f);
         
        }

    }
    void ResolveSelection()
    {
        // Move selected cards to target positions
        for (int i = 0; i < selectedCards.Count; i++)
        {
       
            selectedCards[i].transform.DOMove(
                targetPositions[i].position,
                moveDuration
            );
            
            //targetPositions.RemoveAt(i);
            
        }
        StartCoroutine(RemoveSelectedCardsRoutine());

        // Move unselected cards back to spawn
        foreach (var cardGO in handCards)
        {
            Cards card = cardGO.GetComponent<Cards>();
            if (!card.IsSelected)
            {
                card.Flip(false);
                card.transform.DOMove(cardspawnppoint.position, moveDuration);
            }
        }
    }



    void UpdateCardPositions()
    {
        if(handCards.Count == 0) return;
        float cardSpacing = 1f / MaxHandSize;
        float firstPosition = 0.5f - (cardSpacing * (handCards.Count - 1)) / 2f;
        Spline spline = splinecontainer.Spline;
        for (int i = 0; i < handCards.Count; i++)
        {
            float t = firstPosition + i * cardSpacing;
            Vector3 position = spline.EvaluatePosition(t);
            Vector3 forward= spline.EvaluateTangent(t);
            Vector3 up = spline.EvaluateUpVector(t);
            Quaternion rotation=   Quaternion.LookRotation(Vector3.Cross(forward,up), up);
            handCards[i].transform.DOMove(position,0.25f);
            handCards[i].transform.DOLocalRotateQuaternion(rotation,0.25f);
        }

    }
    
    IEnumerator DrawCardsRoutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            
                DrawCard();
            
            
            yield return new WaitForSeconds(0.25f); // delay between cards
        }
    }
    private void DrawCard()
    {
        if (dice.IsCountingAnimation) return;
        if (handCards.Count >= MaxHandSize) return;

        // Get available prefabs that are not in hand yet
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < cardPrefabs.Count; i++)
        {
            bool inHand = false;
            foreach (var card in handCards)
            {
                if (card.name.StartsWith(cardPrefabs[i].name)) // prefab match
                {
                    inHand = true;
                    break;
                }
            }
            if (!inHand) availableIndexes.Add(i);
        }

        if (availableIndexes.Count == 0) return; // no more unique cards

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        GameObject prefab = cardPrefabs[randomIndex];
        GameObject newCard = Instantiate(cardPrefabs[randomIndex], splinecontainer.transform);
        newCard.transform.position = cardspawnppoint.position;
        newCard.transform.localScale *=0.85f;
        Cards cardComponent = newCard.GetComponent<Cards>();
        cardComponent.SourcePrefab  = prefab;
        handCards.Add(newCard);
        UpdateCardPositions();
    }


   

    IEnumerator RemoveSelectedCardsRoutine()
    {
        yield return new WaitForSeconds(moveDuration);

        foreach (var card in selectedCards)
        {
            // Remove from hand
            handCards.Remove(card.gameObject);

            // Remove prefab from future draws
            cardPrefabs.Remove(card.SourcePrefab);


        }

        //selectedCards.Clear();
    }
    void ClearHand()
    {
        foreach (var cardGO in handCards)
        {
            Destroy(cardGO);
        }

        handCards.Clear();
        //selectedCards.Clear();
    }

    public void OnPointAnimationFinished()
    {
        if (dice.turn == 3 && ischoosecard)
        {
            

            ClearHand();
            StartCoroutine(DrawCardsRoutine(MaxHandSize));
            ischoosecard = false;
        }
    }






}
