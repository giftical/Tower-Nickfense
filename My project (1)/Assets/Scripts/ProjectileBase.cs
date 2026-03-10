using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void Init(IDamageable target, float damage);
}