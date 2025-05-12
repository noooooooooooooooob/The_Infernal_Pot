using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCardEffect", menuName = "Scriptable Objects/SpecialCardEffect")]
public class SpecialCardEffect : ScriptableObject
{
    public string effectName;
    public Sprite icon;
    [TextArea] public string description;

    public virtual void ApplyEffect(Card user, Card target)
    {
        Debug.Log($"⚡ 스페셜 효과 '{effectName}'");
    }
}
