using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    // Handle attacking logic
    public void HandleAttack(GameObject attacker, float attackRange, int attackDamage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(attacker.transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != attacker) // Ensure the attacker doesn't hit themselves
            {
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                    Debug.Log($"{attacker.name} hit {hitCollider.name} for {attackDamage} damage!");
                }
            }
        }
    }

    // Optional: Visualize attack range in the editor
    public void DrawAttackRange(Vector3 position, float attackRange)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRange);
    }
}
