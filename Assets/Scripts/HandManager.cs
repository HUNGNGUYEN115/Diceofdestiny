using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int MaxHandSize;
    [SerializeField] private GameObject[] cardPrefabs;
    [SerializeField] private SplineContainer splinecontainer;
    [SerializeField] private Transform cardspawnppoint;

    private List<GameObject> handCards = new();
    [SerializeField] private Transform[] targetPositions; // size = 2
    [SerializeField] private float moveDuration = 1f;

    private List<Cards> selectedCards = new();
    public static HandManager Instance;


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
        if (selectedCards.Count >= 2) return;

        Ray ray = Camera.main.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Cards card = hit.collider.GetComponent<Cards>();
            if (card == null || card.IsSelected) return;

            SelectCard(card);
    }
    }
 
    void SelectCard(Cards card)
    {
        card.transform.DOMoveY(card.transform.position.y + 1.5f, 0.15f);

        card.Select();
        card.Flip(true);
        selectedCards.Add(card);

        Tween flipTween = card.Flip(true);

        if (selectedCards.Count == 2)
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
        }

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
        if (handCards.Count >= MaxHandSize) return;

        // Get available prefabs that are not in hand yet
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < cardPrefabs.Length; i++)
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

        GameObject newCard = Instantiate(cardPrefabs[randomIndex], splinecontainer.transform);
        newCard.transform.position = cardspawnppoint.position;
        newCard.transform.localScale *=0.85f;
        handCards.Add(newCard);
        UpdateCardPositions();
    }


    void Start()
    {
        StartCoroutine(DrawCardsRoutine(MaxHandSize));
    }

    
  


}
