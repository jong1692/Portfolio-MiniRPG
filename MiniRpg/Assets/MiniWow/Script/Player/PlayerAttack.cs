using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private int _damage = 20;

        [SerializeField]
        private float _attackRange = 40;

        private bool _isAttacking = false;

        [SerializeField]
        private float _attackDelay = 1.5f;

        [SerializeField]
        private UIManager _uiMgr;

        private float _attackTimer = 1.5f;

        public float attackTimer
        {
            get { return _attackTimer; }
        }

        public float attackDelay
        {
            get { return _attackDelay; }
        }

        private PlayerController _playerController;

        void Awake()
        {
            _isAttacking = false;

            GetComponent<SphereCollider>().radius = _attackRange;

            _playerController = transform.parent.GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _isAttacking = false;

            _attackTimer = 1.5f;

            _attackDelay = 1.5f;
        }

        void FixedUpdate()
        {
            _attackTimer += Time.deltaTime;

            EnemyController instance = _uiMgr.curInstance as EnemyController;
            if (instance == null)
            {
                _isAttacking = false;

                if (Input.GetMouseButtonDown(1))
                {
                    _uiMgr.ShowWarningMsg("지정된 적이 없습니다.");
                }

                return;
            }

            float distance = Vector3.Distance(transform.position, instance.transform.position);
            if (distance < 2.0f)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _isAttacking = true;

                    _playerController.AddEnemy(_uiMgr.curInstance as EnemyController);

                }
            }
            else
            {
                if (_isAttacking == true)
                {
                    _isAttacking = false;

                    _uiMgr.ShowWarningMsg("적과의 거리가 멉니다.");
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _uiMgr.ShowWarningMsg("적과의 거리가 멉니다.");
                }
            }

            if (_attackTimer > _attackDelay)
            {
                if (_isAttacking)
                {
                    _attackTimer = 0;


                    StartCoroutine(Attack());
                }
            }
        }

        IEnumerator Attack()
        {
            _animator.SetBool("Attack", true);

            EnemyController instance = _uiMgr.curInstance as EnemyController;
            instance.Damaged(_damage);

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack1"))
            {
                yield return null;
            }

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                yield return null;
            }

            _animator.SetBool("Attack", false);
        }
    }
}