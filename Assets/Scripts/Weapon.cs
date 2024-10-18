using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    public float range;
    public float fireRate;

    protected float nextFireTime;

    public abstract void Fire();

    protected bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    protected void UpdateNextFireTime()
    {
        nextFireTime = Time.time + 1f / fireRate;
    }
}