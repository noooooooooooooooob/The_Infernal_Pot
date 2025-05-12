[System.Serializable]
public class CardData
{
    public CardType type;
    public int rank;
    public SpecialCardEffect specialEffect;

    public CardData(CardType type, int rank,SpecialCardEffect effect = null)
    {
        this.type = type;
        this.rank = rank;
        this.specialEffect = effect;
    }
}