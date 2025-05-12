using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyDeckData", menuName = "Scriptable Objects/EnemyDeckData")]
public class EnemyDeckData : ScriptableObject
{
    public List<CardData> EnemyPlayingDeck = new List<CardData>();
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
        EnemyPlayingDeck.Clear();
        foreach (var card in Deck)
        {
            EnemyPlayingDeck.Add(new CardData(card.type, card.rank));
        }
        ShuffleDeck();
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < EnemyPlayingDeck.Count; i++)
        {
            var temp = EnemyPlayingDeck[i];
            int randomIndex = Random.Range(i, EnemyPlayingDeck.Count);
            EnemyPlayingDeck[i] = EnemyPlayingDeck[randomIndex];
            EnemyPlayingDeck[randomIndex] = temp;
        }
    }
    public CardData? DrawCard()
    {
        if (EnemyPlayingDeck.Count <= 0)
        {
            Debug.Log("❌ 더 이상 뽑을 카드가 없습니다!");
            DeckReInitialize();
        }
        
        CardData drawnCard = EnemyPlayingDeck[0];
        EnemyPlayingDeck.RemoveAt(0); // 실제로 덱에서 제거
        Debug.Log("🃏 카드 뽑기: " + drawnCard.type + " " + drawnCard.rank);
        return drawnCard;
    }
    private void DeckReInitialize()
    {
        InitializeDeck();
        ShuffleDeck();
    }
}
