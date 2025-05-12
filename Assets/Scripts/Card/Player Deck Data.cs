using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDeckData", menuName = "Scriptable Objects/PlayerDeckData")]
public class PlayerDeckData : ScriptableObject
{
    public List<CardData> playerDeck = new List<CardData>();
    public List<CardData> playerDeadDeck = new List<CardData>();
    public List<CardData> Deck;

    void OnEnable()
    {
        makeNewDeck();
    }

    public void makeNewDeck()
    {
        if (Deck == null || Deck.Count == 0)
        {
            Deck = new List<CardData>();
            for (int suit = 0; suit < 4; suit++)
            {
                for (int value = 1; value <= 10; value++)
                {
                    Deck.Add(new CardData((CardType)suit, value));
                }
            }
            Debug.Log("✅ Deck 초기화 완료!");
        }
    }

    public void InitializeDeck()
    {
        playerDeck.Clear();
        playerDeadDeck.Clear();
        foreach (var card in Deck)
        {
            playerDeck.Add(new CardData(card.type, card.rank));
        }
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < playerDeck.Count; i++)
        {
            var temp = playerDeck[i];
            int randomIndex = Random.Range(i, playerDeck.Count);
            playerDeck[i] = playerDeck[randomIndex];
            playerDeck[randomIndex] = temp;
        }
    }

    public void AddCard(CardData newCard)
    {
        Deck.Add(newCard);
        Debug.Log("🃏 카드 추가: " + newCard.type + " " + newCard.rank);
    }

    public void RemoveCard(CardData cardToRemove)
    {
        Deck.Remove(cardToRemove);
    }
    public CardData? DrawCard()
    {
        if (playerDeck.Count <= 0)
        {
            Debug.Log("❌ 더 이상 뽑을 카드가 없습니다!");
            DeckReInitialize();
        }
        
        CardData drawnCard = playerDeck[0];
        playerDeck.RemoveAt(0); // 실제로 덱에서 제거
        Debug.Log("🃏 카드 뽑기: " + drawnCard.type + " " + drawnCard.rank);
        return drawnCard;
    }
    private void DeckReInitialize()
    {
        InitializeDeck();
        ShuffleDeck();
    }
    public void AddCardToDeadDeck(CardData card)
    {
        playerDeadDeck.Add(card);
        Debug.Log("🪦 카드 죽은 덱에 추가: " + card.type + " " + card.rank);
    }
}
