using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterSelectManager : MonoBehaviour
{
    [Serializable]
    public class Character
    {
        public GameObject[] Characters;
    }

    public Character[] Characters; // 캐릭터 배열
    public GameObject[] CharacterSelectSceneObjects; // 캐릭터 선택 씬 오브젝트
    int currentCharacterIndex = -1;

    public void Select(int num)
    {
        if (num == currentCharacterIndex) return; // 이미 선택된 캐릭터라면 아무것도 하지 않음

        if(currentCharacterIndex>=0)
            foreach (var obj in Characters[currentCharacterIndex].Characters)
            {
                obj.SetActive(false);
            }
        else
            foreach (var obj in CharacterSelectSceneObjects)
            {
                obj.SetActive(true);
            }
        currentCharacterIndex = num;

        // 새로 선택된 캐릭터 활성화
        foreach (var obj in Characters[currentCharacterIndex].Characters)
        {
            obj.SetActive(true);
        }
    }
}

