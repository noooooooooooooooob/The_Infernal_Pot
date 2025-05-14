using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
    Character playerCharacter;
    Enemy enemyCharacter;
    public TurnManager turnManager;
    public HandManager handManager;
    public GameObject[] PlayerPlayingCards;
    public GameObject[] EnemyPlayingCards;
    // 현재 배틀 중인 플레이어 카드와 적 카드
    Card curPlayerCard;
    Card curEnemyCard;
    // 비활성화 해야 할 것들
    public CardZone[] cardZones;
    public Card[] EnemyCards;
    public GameObject GameReadyPanel;
    public GameObject PlayerCalculatePanel;
    public GameObject BattleStartButton;

    void Awake()
    {
        handManager = FindObjectOfType<HandManager>();
        turnManager = FindObjectOfType<TurnManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        playerCharacter = player.GetComponent<Character>();
        enemyCharacter = enemy.GetComponent<Enemy>();
    }
    public void StartBattleButton()
    {
        StartCoroutine(StartBattle());
    }
    IEnumerator StartBattle()
    {
        SetPlayerPlayingCards();
        SetEnemyPlayingCards();
        GameReadyPanel.SetActive(false);
        PlayerCalculatePanel.SetActive(false);
        BattleStartButton.SetActive(false);
        for (int i = 0; i < PlayerPlayingCards.Length; i++)
        {
            PlayerPlayingCards[i].SetActive(true);
            EnemyPlayingCards[i].SetActive(true);
        }
        handManager.ClearHand();

        yield return new WaitForSeconds(2.0f);
        int cardCount = PlayerPlayingCards.Length;

        for (int i = 0; i < cardCount; i++)
        {
            curPlayerCard = PlayerPlayingCards[0].GetComponent<Card>();
            curEnemyCard = EnemyPlayingCards[0].GetComponent<Card>();

            yield return StartCoroutine(ResolveOneBattle(curPlayerCard, curEnemyCard));

            // 한 칸씩 당기기
            for (int j = 0; j < cardCount - 1; j++)
            {
                PlayerPlayingCards[j].GetComponent<Card>().cardData = PlayerPlayingCards[j + 1].GetComponent<Card>().cardData;
                EnemyPlayingCards[j].GetComponent<Card>().cardData = EnemyPlayingCards[j + 1].GetComponent<Card>().cardData;
            }

            // 마지막 칸은 null로
            PlayerPlayingCards[cardCount - 1].GetComponent<Card>().cardData = null;
            EnemyPlayingCards[cardCount - 1].GetComponent<Card>().cardData = null;

            SetPlayingCardsSprite();
        }

        yield return new WaitForSeconds(2.0f);
        EndBattle();
    }

    // 전투 처리 메서드
    IEnumerator ResolveOneBattle(Card playerCard, Card enemyCard)
    {
        // 애니메이션 등 시각 효과 시작 (예시)
        Debug.Log($"🔥 전투 : Player {playerCard.cardData.type} {playerCard.cardData.rank} vs Enemy {enemyCard.cardData.type} {enemyCard.cardData.rank}");

        // 예시: rank 비교로 승부 결정
        string result;
        if (playerCard.cardData.rank > enemyCard.cardData.rank)
        {
            switch (playerCard.cardData.type)
            {
                case CardType.Club:
                    playerCharacter.AddShield(playerCard.cardData.rank);
                    break;
                case CardType.Diamond:
                    playerCharacter.PlayAttack(); // 애니메이션 먼저 재생
                    yield return new WaitUntil(() => playerCharacter.hasHit); // 애니메이션 중 타격 이벤트 대기
                    enemyCharacter.TakeDamage(playerCard.cardData.rank - enemyCard.cardData.rank);
                    break;
                case CardType.Heart:
                    playerCharacter.Heal(playerCard.cardData.rank);
                    break;
                case CardType.Spade:
                    playerCharacter.AddShield(playerCard.cardData.rank);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            switch (enemyCard.cardData.type)
            {
                case CardType.Club:
                    enemyCharacter.AddShield(enemyCard.cardData.rank);
                    break;
                case CardType.Heart:
                    enemyCharacter.Heal(enemyCard.cardData.rank);
                    break;
                case CardType.Spade:
                    enemyCharacter.AddShield(enemyCard.cardData.rank);
                    break;
                case CardType.Diamond:
                    enemyCharacter.PlayAttack(); // 애니메이션 먼저 재생
                    yield return new WaitUntil(() => enemyCharacter.hasHit); // 애니메이션 중 타격 이벤트 대기
                    playerCharacter.TakeDamage(enemyCard.cardData.rank - playerCard.cardData.rank);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            result = "Player Wins!";
        }
        else if (playerCard.cardData.rank < enemyCard.cardData.rank)
        {
            switch (enemyCard.cardData.type)
            {
                case CardType.Club:
                    enemyCharacter.AddShield(enemyCard.cardData.rank);
                    break;
                case CardType.Diamond:
                    enemyCharacter.PlayAttack(); // 애니메이션 먼저 재생
                    yield return new WaitUntil(() => enemyCharacter.hasHit); // 애니메이션 중 타격 이벤트 대기
                    playerCharacter.TakeDamage(enemyCard.cardData.rank - playerCard.cardData.rank);
                    break;
                case CardType.Heart:
                    enemyCharacter.Heal(enemyCard.cardData.rank);
                    break;
                case CardType.Spade:
                    enemyCharacter.AddShield(enemyCard.cardData.rank);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            switch (playerCard.cardData.type)
            {
                case CardType.Club:
                    playerCharacter.AddShield(playerCard.cardData.rank);
                    break;
                case CardType.Heart:
                    playerCharacter.Heal(playerCard.cardData.rank);
                    break;
                case CardType.Spade:
                    playerCharacter.AddShield(playerCard.cardData.rank);
                    break;
                case CardType.Diamond:
                    playerCharacter.PlayAttack(); // 애니메이션 먼저 재생
                    yield return new WaitUntil(() => playerCharacter.hasHit); // 애니메이션 중 타격 이벤트 대기
                    enemyCharacter.TakeDamage(playerCard.cardData.rank - enemyCard.cardData.rank);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            result = "Enemy Wins!";
        }
        else
        {
            result = "Draw!";
            yield return new WaitForSeconds(2.0f);
        }

        Debug.Log($"⚔️ 결과: {result}");

        // TODO: 승패에 따라 애니메이션/효과/점수 계산 등 처리

        yield return new WaitForSeconds(1.5f); // 시각적 텀 부여
    }
    public void EndBattle()
    {
        GameReadyPanel.SetActive(true);
        PlayerCalculatePanel.SetActive(true);
        BattleStartButton.SetActive(true);
        for (int i = 0; i < PlayerPlayingCards.Length; i++)
        {
            PlayerPlayingCards[i].SetActive(false);
            EnemyPlayingCards[i].SetActive(false);
        }
        turnManager.isBattle = false;
    }

    void SetPlayerPlayingCards()
    {
        for (int i = 0; i < PlayerPlayingCards.Length; i++)
        {
            PlayerPlayingCards[i].GetComponent<Card>().cardData = cardZones[i].cardData;
            PlayerPlayingCards[i].GetComponent<SpriteRenderer>().sprite = PlayerPlayingCards[i].GetComponent<Card>().GetSprite();
        }
    }
    void SetEnemyPlayingCards()
    {
        for (int i = 0; i < EnemyPlayingCards.Length; i++)
        {
            EnemyPlayingCards[i].GetComponent<Card>().cardData = EnemyCards[i].cardData;
            EnemyPlayingCards[i].GetComponent<SpriteRenderer>().sprite = EnemyPlayingCards[i].GetComponent<Card>().GetSprite();
        }
    }
    void SetPlayingCardsSprite()
    {
        for (int i = 0; i < PlayerPlayingCards.Length; i++)
        {
            PlayerPlayingCards[i].GetComponent<SpriteRenderer>().sprite = PlayerPlayingCards[i].GetComponent<Card>().GetSprite();
        }
        for (int i = 0; i < EnemyPlayingCards.Length; i++)
        {
            EnemyPlayingCards[i].GetComponent<SpriteRenderer>().sprite = EnemyPlayingCards[i].GetComponent<Card>().GetSprite();
        }
    }
}
