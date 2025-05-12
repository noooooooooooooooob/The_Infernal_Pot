using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    public int maxHP = 100;
    public int currentHP;
    public TextMeshProUGUI hpText;
    Animator animator;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log($"{gameObject.name} 체력: {currentHP}/{maxHP}");
        hpText.text = $"{currentHP} / {maxHP}";

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        // 게임 오버, 패배 처리 등
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayDie()
    {
        animator.SetTrigger("Die");
    }
}
