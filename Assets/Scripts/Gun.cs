using UnityEngine;

public class Gun : Weapon
{
    public override void Fire()
    {
        if (CanFire())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                    Debug.Log($"Hit {hit.transform.name} for {damage} damage with {weaponName}");
                }
            }
            UpdateNextFireTime();
        }
    }
}