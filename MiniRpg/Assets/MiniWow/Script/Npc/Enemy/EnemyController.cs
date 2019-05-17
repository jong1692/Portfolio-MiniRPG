using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HotMountain
{
    public class EnemyController : NpcController
    {
        private float _attackRange;

        [SerializeField]
        private float _gazeRange;

        private float _originGazeRange;

        private float _minGazeRange = 1.0f;

        [SerializeField]
        private float _moveSpeed = 3.5f;

        [SerializeField]
        private float rotationSpeed = 5f;

        private NavMeshAgent _nav;

        [SerializeField]
        private Transform _headPos;

        private Vector3 _responePos;
        private bool _isGaze = false;
        private bool _canAttack = false;

        [SerializeField]
        private float _attackDelay = 2.0f;
        private float _attackTimer = 2.0f;

        [SerializeField]
        private float _randMoveDelay = 2.0f;
        private float _randMoveTimer = 0.0f;

        private bool _isReturn = false;
        private bool _isDamaged = false;
        private bool _isAttacking = false;  

        private float _originRotSpeed;
        private float _originStoppingDistance;

        [SerializeField]
        private UIManager _uiMgr;

        void Awake()
        {
            Init();

            _originGazeRange = _gazeRange;

            _useDialogue = false;
            _damageable = true;

            _responePos = transform.position;

            _nav = GetComponent<NavMeshAgent>();
            _nav.destination = _responePos;
            _nav.speed = _moveSpeed;

            _uiMgr = GameObject.Find("UIManager").GetComponent<UIManager>();

            _originRotSpeed = _nav.angularSpeed;
            _originStoppingDistance = _nav.stoppingDistance;

            _attackRange = _nav.stoppingDistance;
        }

        void Update()
        {
            if (!_isDeath)
            {
                CheckCombatStatus();
                Attack();

                if (!_isAttacking)
                {
                    Move();
                    Return();
                }
            }
        }

        private void CheckCombatStatus()
        {
            if (!_isReturn && _isDamaged && _playerHp.isDamageable)
            {
                _nav.destination = _playerContoller.transform.position;
                _nav.stoppingDistance = _originStoppingDistance;

                StopCoroutine(RandomMove());

                _isCombat = true;
            }
            else
            {
                _isDamaged = false;
                _isCombat = false;

                _healthPoint = _maxHp;

                _randMoveTimer += Time.deltaTime;
                if (_randMoveTimer >= _randMoveDelay)
                {
                    _randMoveTimer = 0;

                    StartCoroutine(RandomMove());
                }
            }

            ChangeActiveCircle(isCombat);
        }

        IEnumerator RandomMove()
        {
            float randX = Random.Range(-_gazeRange / 2f, _gazeRange / 2f);
            float randZ = Random.Range(-_gazeRange / 2f, _gazeRange / 2f);

            _nav.destination = _responePos + new Vector3(randX, 0, randZ);
            _nav.stoppingDistance = 0;
            _nav.speed = _moveSpeed / 3;

            _animator.SetBool("Walk", true);


            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Walk"))
            {

                yield return null;
            }

            float distance;
            do
            {
                distance = Vector3.Distance(this.transform.position, _nav.destination);

                yield return null;
            } while (distance > 0.1f);

            _nav.speed = _moveSpeed;
            _animator.SetBool("Walk", false);
        }

        private void Move()
        {
            if (_isReturn || !_isCombat) return;

            Vector3 playerVec = _playerContoller.transform.position;
            float distance = Vector3.Distance(this.transform.position, playerVec);

            if (distance <= _gazeRange)
            {
                if (distance <= _nav.stoppingDistance) Rotate();
                else
                {
                    _nav.speed = _moveSpeed;

                    _animator.SetBool("Move", true);
                    _animator.SetBool("Walk", false);
                }
            }
            else if (_animator.GetBool("Move"))
                _animator.SetBool("Move", false);
        }

        private bool Return()
        {
            if (Vector3.Distance(transform.position, _responePos) > _gazeRange && !_isReturn)
            {
                _nav.destination = _responePos;

                _animator.SetBool("Move", true);
                _animator.SetBool("Walk", false);

                _isReturn = true;
                _isDamaged = false;

                _playerContoller.RemoveEnemy(_instance as EnemyController);

                _nav.speed = _moveSpeed * 2f;
                      

            }

            if (Vector3.Distance(transform.position, _responePos) < 1.5f && _isReturn)
            {
                _animator.SetBool("Move", false);
                _animator.SetBool("Walk", false);

                _isReturn = false;

                _nav.speed = _moveSpeed;              
            }

            return _isReturn;
        }

        private void Rotate()
        {
            Vector3 playerVec = _playerContoller.transform.position;

            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Attack")
                && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Death"))
            {
                if (Vector3.Distance(_headPos.transform.position, playerVec) > _attackRange / 2)
                {
                    Vector3 vec = playerVec - transform.position;
                    vec.Normalize();

                    Quaternion qua = Quaternion.LookRotation(vec);

                    Transform originForm = transform;
                    transform.rotation = Quaternion.Slerp(transform.rotation, qua, 5 * Time.deltaTime);
                }
            }

            _animator.SetBool("Move", false);
        }


        void Attack()
        {
            if (!_playerHp.isDamageable || _isReturn || !_isCombat) return;

            Vector3 playerVec = _playerContoller.transform.position;
            float distance = Vector3.Distance(this.transform.position, playerVec);

            if (_canAttack)
            {
                if (distance <= _attackRange)
                {
                    _canAttack = false;
                    _animator.SetBool("Attack", true);
                    StartCoroutine(CheckAnimationState());
                }
            }
            else
            {
                _attackTimer += Time.deltaTime;
                if (_attackDelay < _attackTimer)
                {
                    _canAttack = true;
                    _attackTimer = 0;

                    transform.GetComponentInChildren<EnemyAttack>().attacked = false;
                }
            }
        }

        IEnumerator CheckAnimationState()
        {
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Attack"))
            {
                yield return null;
            }

            _isAttacking = true;

            _nav.speed = 0;
            _nav.angularSpeed = 0;

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
            {
                yield return null;
            }

            _animator.SetBool("Attack", false);

            _nav.speed = _moveSpeed;
            _nav.angularSpeed = _originRotSpeed;

            _isAttacking = false;
        }

        public void Damaged(int damage)
        {
            _isDamaged = true;
            
           _uiMgr.GetComponent<HitText>().ShowEnemyHpText(-damage);

            Damaged(damage, "Wolf_Death", this.gameObject);
        }
    }
}