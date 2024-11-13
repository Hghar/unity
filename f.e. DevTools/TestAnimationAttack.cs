using System;
using System.Collections;
using AssetStore.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class TestAnimationAttack : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] AnimationClip ClipCharge;
    [SerializeField] private Transform _armL;
    [SerializeField] private GameObject _targetEnemy;
    [SerializeField] private Transform _startPosBullet;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private AttackingRotateArcher _attackingRotateArcher;

    [Header("Check to disable arm auto rotation.")]
    public bool FixedArm;

    private float _delayAnimation = 0.4f;
    private float _chargeTime;
    private bool _IsInstantiate;
    private Transform arm;

    public bool IsInstantiate => _IsInstantiate;

    public Character Character => _character;

    public Transform ArmL => _armL;

    private void Start()
    {
        _targetEnemy = GameObject.Find("FakeTarget");
    }

    public event Action ReadyToAttack;

    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _character.Animator.SetBool("Ready", true);
            StartCoroutine(AttackArcherAnimation());
        }

        if (_character.IsReady())
        {
            Transform _arm;
            Transform _weapon;

            _arm = _armL.transform;
            _weapon = _character.BodyRenderers[3].transform;
            _attackingRotateArcher.RotateArm(_arm, _weapon,
                FixedArm ? _arm.position + 1000 * Vector3.right : _targetEnemy.transform.position, -40, 40);
            _character.Animator.SetInteger("Charge", 1);
        }
    }

    public IEnumerator AttackArcherAnimation(Action callback)
    {
        _character.Animator.SetBool("Ready", true);
        yield return new WaitForSeconds(_delayAnimation);
        var charged = Time.time - _chargeTime > ClipCharge.length;
        _character.Animator.SetInteger("Charge", charged ? 2 : 3);
        callback?.Invoke();
        yield return new WaitForSeconds(0.1f);
        _character.Animator.SetBool("Ready", false);
    }

    public IEnumerator AttackArcherAnimation()
    {
        yield return new WaitForSeconds(_delayAnimation);
        var charged = Time.time - _chargeTime > ClipCharge.length;
        _character.Animator.SetInteger("Charge", charged ? 2 : 3);
        GameObject projectile = Instantiate(_projectilePrefab, _startPosBullet.position,
            Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        _character.Animator.SetBool("Ready", false);
    }
}
