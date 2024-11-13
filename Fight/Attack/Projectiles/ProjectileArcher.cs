using System;
using System.Collections;
using Fight.Damaging;
using Units;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    public class ProjectileArcher : MonoBehaviour
    {
        [SerializeField] private float _speedProjectile;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float maxAngle = 45f;
        private Action _onHit;


        public void DamageProjectile(IMinion target, int damage, IMinion caster, Action onHit)
        {
            _onHit = onHit;
            StartCoroutine(MoveBullet(target, damage, caster));
        }

        private IEnumerator MoveBullet(IMinion target, int damage, IMinion caster)
        {
            while ((target as MonoBehaviour) && Vector3.Distance(transform.position, target.GameObject.transform.position) > 0.01)
            {
                if (target.GameObject == null)
                {
                    Destroy(gameObject);
                }

                AngleAttacking(target);
                transform.position = Vector3.MoveTowards(transform.position, target.GameObject.transform.position,
                    _speedProjectile * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            target.Damage(new Damage(damage), caster);
            _onHit?.Invoke();
            Destroy(gameObject);
        }

        private void AngleAttacking(IMinion target)
        {
            Vector3 direction = target.GameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -maxAngle, maxAngle);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}