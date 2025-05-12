using UnityEngine;
using DG.Tweening;
using UnityEngine.XR;
using System;

public class PlayerDragableCard : Card
{
    private Vector3 offset;
    private Camera cam;
    private SpriteRenderer spriteRenderer;
    public PlayerDeckData playerDeckData;
    public HandManager handManager;
    
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private GameObject targetCardZone;
    private bool isDragging = false;
    private bool isReturning = false;
    public float hoverScale = 1.2f;
    public float hoverDuration = 0.2f;

    private int originalSortingOrder;
    private static PlayerDragableCard highlightedCard = null;
    private bool isOverZone = false;
    private bool isTrashZone = false;
    

    void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        originalSortingOrder = spriteRenderer.sortingOrder;
        handManager = FindObjectOfType<HandManager>();

        cardData = playerDeckData.DrawCard();
        Initialize(cardData);
    }

    public void Initialize(CardData data)
    {
        cardData = data;
        spriteRenderer.sprite = GetSprite();
    }

    void OnMouseEnter()
    {
        if (isOverZone || isDragging || (highlightedCard != null && highlightedCard != this))
            return;

        highlightedCard = this;
        transform.DOScale(originalScale * hoverScale, hoverDuration).SetEase(Ease.OutQuad);
        spriteRenderer.sortingOrder = 100;
    }

    void OnMouseExit()
    {
        if (highlightedCard == this)
        {
            transform.DOScale(originalScale, hoverDuration).SetEase(Ease.OutQuad);
            spriteRenderer.sortingOrder = originalSortingOrder;
            highlightedCard = null;
        }
    }

    void OnMouseDown()
    {
        if (isReturning) return;
        handManager.SetTrashZoneActive();
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        SetOriginalPosition();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
        handManager.ReorderHand(this.gameObject);
    }

    void OnMouseUp()
    {
        isDragging = false;
        if(isTrashZone && handManager.curRedrawCount > 0)
        {
            handManager.curRedrawCount--;
            handManager.SetTrashZoneDeActive();
            handManager.DrawCardToHand(1);
            playerDeckData.AddCardToDeadDeck(cardData);
            Destroy(gameObject);
            return;
        }
        else
        {
            handManager.SetTrashZoneDeActive();
        }

        if (!isOverZone)
        {
            isReturning = true;
            handManager.PlusHand(this.gameObject);
            isReturning = false;
            // transform.DOMove(originalPosition, 0.25f)
            //          .SetEase(Ease.OutQuad)
            //          .OnComplete(() => isReturning = false);

        }
        else
        {
            ClearTargetZone();
            transform.DOMove(targetCardZone.transform.position, 0.25f)
                     .SetEase(Ease.OutQuad);
            spriteRenderer.sortingOrder = targetCardZone.GetComponent<SpriteRenderer>().sortingOrder + 1;
            targetCardZone.GetComponent<CardZone>().SetCardData(cardData);
        }
        
    }

    public void SetOriginalPosition()
    {
        originalPosition = transform.position;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = 10f;
        return cam.ScreenToWorldPoint(mouseScreen);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Card Zone"))
        {
            // other.GetComponent<CardZone>().SetCardData(cardData);
            targetCardZone = other.gameObject;
            isOverZone = true;
        }
        if (other.CompareTag("Trash Zone"))
        {
            isTrashZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Card Zone") && targetCardZone == other.gameObject)
        {
            CardZone cardZone = other.GetComponent<CardZone>();

            // 이 카드가 실제로 해당 존을 점유하고 있던 카드일 때만 null 처리
            if (cardZone.cardData != null &&
                cardZone.cardData.type == cardData.type &&
                cardZone.cardData.rank == cardData.rank)
            {
                cardZone.SetCardData(null);
            }

            isOverZone = false;
            // **만약 TargetZone에 남아 있는 다른 카드가 없다면 값 복구**
            Collider2D[] colliders = Physics2D.OverlapCircleAll(other.transform.position, 0.1f);
            bool otherCardExists = false;

            foreach (Collider2D col in colliders)
            {
                PlayerDragableCard existingCard = col.GetComponent<PlayerDragableCard>();
                if (existingCard != null && existingCard != this)
                {
                    otherCardExists = true;
                    break;
                }
            }
        }
        if (other.CompareTag("Trash Zone"))
        {
            isTrashZone = false;
        }
    }

    void ClearTargetZone()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetCardZone.transform.position, 0.1f); // targetZone 근처에 있는 오브젝트 찾기
        foreach (Collider2D col in colliders)
        {
            PlayerDragableCard existingCard = col.GetComponent<PlayerDragableCard>();
            if (existingCard != null && existingCard != this)
            {
                handManager.PlusHand(existingCard.gameObject); // 기존 카드 핸드로 복귀
            }
        }
    }
}
