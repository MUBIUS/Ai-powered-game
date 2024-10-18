using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage = 20f;
    public float range = 2f;
    public float attackRate = 2f;
    public Camera fpsCam;

    private float nextTimeToAttack = 0f;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f / attackRate;
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Melee Hit: " + hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
        }
    }
}