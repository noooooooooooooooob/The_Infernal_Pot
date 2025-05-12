using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public enum TurnState { Start, Draw, Play, End }
public class TurnManager : MonoBehaviour
{
    public TurnState currentTurnState = TurnState.Start;
    public HandManager handManager;
    public BattleManager battleManager;
    private bool playConfirmed = false;
    public bool isBattle { get; set; }

    void Start()
    {
        StartCoroutine(TurnRoutine());
    }
    IEnumerator TurnRoutine()
    {
        while (true)
        {
            switch (currentTurnState)
            {
                case TurnState.Start:
                    yield return StartCoroutine(StartTurn());
                    break;
                case TurnState.Draw:
                    yield return StartCoroutine(DrawPhase());
                    break;
                case TurnState.Play:
                    yield return StartCoroutine(PlayPhase());
                    break;
                case TurnState.End:
                    yield return StartCoroutine(EndTurn());
                    break;
            }
        }
    }
    IEnumerator StartTurn()
    {
        Debug.Log("턴 시작");
        yield return new WaitForSeconds(1.5f);
        currentTurnState = TurnState.Draw;
        yield return null;
    }
    IEnumerator DrawPhase()
    {
        Debug.Log("드로우 및 행동 선택");
        handManager.GoDrawPhase();
        battleManager.EndBattle();
        yield return new WaitUntil(() => playConfirmed);
        currentTurnState = TurnState.Play;
        playConfirmed = false;
        
    }
    IEnumerator PlayPhase()
    {
        Debug.Log("카드 실행 및 전투 처리");
        isBattle = true;
        // 예시: 배틀 매니저가 플레이를 처리
        yield return new WaitUntil(() => isBattle == false);

        currentTurnState = TurnState.Draw;
        yield return null;
    }

    IEnumerator EndTurn()
    {
        Debug.Log("턴 종료 처리");
        yield return new WaitForSeconds(0.5f);
        currentTurnState = TurnState.Start;
    }
    public void OnPlayButtonClicked()
    {
        playConfirmed = true;
        battleManager.StartBattleButton();
    }
}
