using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public PlayerDeckData playerDeckData;
    public EnemyDeckData enemyDeckData;
    public GameObject cardPrefab;
    public Transform handArea;
    public int handSize = 5;
    public int maxRedrawCount = 3;
    public int curRedrawCount = 3;
    public Transform leftBound;
    public Transform rightBound;
    public Transform deckArea;
    public float minSpacing = 0.4f;
    public float maxSpacing = 1.5f;
    public WaitForSeconds DrawTime;

    private List<GameObject> handCards = new List<GameObject>();
    private List<GameObject> cardzoneCards = new List<GameObject>();
    public GameObject[] TrashZone;
    public GameObject[] EnemyCards;
    public TextMeshProUGUI RedrawCountText;

    void Start()
    {
        SetTrashZoneDeActive();
    }
    public void GoDrawPhase()
    {
        SetEnemyCards();
        StartCoroutine(DrawCards());
    }
    public void SetEnemyCards()
    {
        for (int i = 0; i < EnemyCards.Length; i++)
        {
            EnemyCards[i].GetComponent<Card>().cardData = enemyDeckData.DrawCard();
        }
    }

    IEnumerator DrawCards()
    {
        for(int i=0;i<handSize;i++)
        {
            DrawCardToHand(1);
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    public void DrawHand(int count)
    {
        float totalWidth = Mathf.Abs(rightBound.position.x - leftBound.position.x);
        float spacing = Mathf.Clamp(totalWidth / Mathf.Max(1, count), minSpacing, maxSpacing);
        float startX = (leftBound.position.x + rightBound.position.x - spacing * (count - 1)) * 0.5f;
        float y = handArea.position.y;

        for (int i = 0; i < count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab);
            if (cardObj != null)
            {
                Vector3 pos = new Vector3(startX + i * spacing, y, 0);

                cardObj.transform.position = new Vector3(0, y, 0);
                cardObj.transform.DOMove(pos, 0.3f).SetEase(Ease.OutQuad);
                cardObj.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);

                handCards.Add(cardObj);

                SpriteRenderer spriteRenderer = cardObj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.sortingOrder = i + 5;
            }
        }
    }
    public void DrawCardToHand(int count)
    {
        float totalWidth = Mathf.Abs(rightBound.position.x - leftBound.position.x);
        float spacing = Mathf.Clamp(totalWidth / Mathf.Max(1, handCards.Count + count), minSpacing, maxSpacing);
        float startX = (leftBound.position.x + rightBound.position.x - spacing * (handCards.Count + count - 1)) * 0.5f;
        float y = handArea.position.y;

        // 기존 카드 재배치
        for (int i = 0; i < handCards.Count; i++)
        {
            Vector3 newPos = new Vector3(startX + i * spacing, y, 0);
            handCards[i].transform.DOMove(newPos, 0.3f).SetEase(Ease.OutQuad);
            handCards[i].GetComponent<SpriteRenderer>().sortingOrder = i + 5;
        }

        // 새 카드 추가
        for (int i = 0; i < count; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            if (cardObj != null)
            {
                int index = handCards.Count;
                Vector3 pos = new Vector3(startX + index * spacing, y, 0);

                cardObj.transform.DOMove(pos, 0.3f).SetEase(Ease.OutQuad);
                cardObj.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);

                handCards.Add(cardObj);

                SpriteRenderer spriteRenderer = cardObj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.sortingOrder = index + 5;
            }
        }
    }
    public void ClearHand()
    {
        foreach (var card in handCards)
        {
            Destroy(card);
        }
        foreach (var card in cardzoneCards)
        {
            Destroy(card);
        }
        handCards.Clear();
    }
    // 핸드에서 카드 클릭시 reorder
    public void ReorderHand(GameObject cardObj)
    {
        if (handCards.Contains(cardObj))
        {
            handCards.Remove(cardObj);
            cardzoneCards.Add(cardObj);
            RearrangeHandPositions();
        }

    }
    // 핸드로 카드가 되돌아갈시 reorder
    public void PlusHand(GameObject cardObj)
    {
        cardzoneCards.Remove(cardObj);
        handCards.Add(cardObj);
        RearrangeHandPositions();
    }

    private void RearrangeHandPositions()
    {
        float totalWidth = Mathf.Abs(rightBound.position.x - leftBound.position.x);
        float spacing = Mathf.Clamp(totalWidth / Mathf.Max(1, handCards.Count), minSpacing, maxSpacing);
        float startX = (leftBound.position.x + rightBound.position.x - spacing * (handCards.Count - 1)) * 0.5f;
        float y = handArea.position.y;

        for (int i = 0; i < handCards.Count; i++)
        {
            GameObject cardObj = handCards[i];
            Vector3 targetPos = new Vector3(startX + i * spacing, y, 0);

            cardObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad);
            cardObj.transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);

            SpriteRenderer sr = cardObj.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = i + 5;
        }
    }
    public void SetTrashZoneActive()
    {
        foreach(var trash in TrashZone)
            trash.SetActive(true);
    }
    
    public void SetTrashZoneDeActive()
    {
        RedrawCountText.text = curRedrawCount.ToString() + " / " + maxRedrawCount.ToString();
        foreach(var trash in TrashZone)
            trash.SetActive(false);
    }
}