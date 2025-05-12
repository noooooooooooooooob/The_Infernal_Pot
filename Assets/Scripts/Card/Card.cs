using UnityEngine;

public class Card : MonoBehaviour
{
    public CardData cardData;
    int spriteIndex;
    private Sprite[] sprites;
    private Sprite sprite;

    public Sprite GetSprite()
    {
        if (sprite != null) return sprite;
        if (sprites == null) sprites = Resources.LoadAll<Sprite>("Cards");
        if(cardData == null) return sprites[41];
        if(cardData.rank == 0) return sprites[41];

        int baseIndex = 0;
        switch (cardData.type)
        {
            case CardType.Club: baseIndex = 0; break;
            case CardType.Diamond: baseIndex = 10; break;
            case CardType.Heart: baseIndex = 20; break;
            case CardType.Spade: baseIndex = 30; break;
        }
        return sprites[baseIndex + cardData.rank - 1];
    }
}
