using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 100;
    public int shield = 0;
    public int buff = 0;
    public int currentHP;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI shiledText;
    public TextMeshProUGUI buffText;
    Animator animator;
    public bool hasHit = false;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }
    // 데미지 받고 남은 체력 반환
    public int TakeDamage(int amount)
    {
        if (shield > 0)
        {
            shield -= amount;
            if (shield < 0)
            {
                amount = -shield;
                shield = 0;
            }
            else
            {
                amount = 0;
            }
            shiledText.text = $"{shield}";
        }
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        Debug.Log($"{gameObject.name} 체력: {currentHP}/{maxHP}");
        hpText.text = $"{currentHP} / {maxHP}";

        if (currentHP <= 0)
            Die();

        return currentHP;
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
    public void AddShield(int amount)
    {
        shield += amount;
        shiledText.text = $"{shield}";
    }
    public void AddBuff(int amount)
    {
        buff += amount;
        buffText.text = $"{buff}";
    }
    public void PlayHit()
    {
        animator.SetTrigger("Hit");
    }
    public void PlayAttack()
    {
        hasHit = false;
        animator.SetTrigger("Attack");
    }
    public void AttackFinished()
    {
        animator.SetTrigger("Finished");
        hasHit = true;
    }
    public void PlayDie()
    {
        animator.SetTrigger("Die");
    }
    public void AnimFinished()
    {
        animator.SetTrigger("Finished");
    }
}
