using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HotMountain
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]

    public class PlayerController : MonoBehaviour
    {
        public float _forwardSpeed = 4f;
        public float _backwardSpeed = 2f;
        public float _sideSpeed = 3f;
        public float _jumpPower = 2.5f;
        public float _rotateSpeed = 2f;
        public float _speedRate = 0.7f;

        public UIManager _uiManager;

        private EnemyController[] _enemyVec = new EnemyController[5];
        private int _enemyIdx = 0;


        private CapsuleCollider _collider;
        private PlayerInput _playerInput;
        private Animator _animator;
        private Rigidbody _rigidbody;

        AnimatorStateInfo _currentStateInfo;
        float _animSpeed = 1f;

        bool _isBlockInput = false;
        bool _canAttack = false;
        bool _isGrounded = false;
        bool _readToJump = true;
        bool _canInteract = false;

        [SerializeField]
        private UnityEngine.Camera _mainCamera;

        private NavMeshAgent _nav;

        readonly int _idForwadWalk = Animator.StringToHash("Player_ForwardWalk");
        readonly int _idSideWalk = Animator.StringToHash("Player_SideWalk");
        readonly int _idBackWalk = Animator.StringToHash("Player_BackWalk");
        readonly int _idRotate = Animator.StringToHash("Player_ForwardWalk");
        readonly int _idIdle = Animator.StringToHash("Player_Idle");
        readonly int _idAttack = Animator.StringToHash("Player_Attack1");
        readonly int _idJump = Animator.StringToHash("Player_Jump");

        public EnemyController[] enemyVec
        {
            get { return _enemyVec; }
        }

        public int enemyIdx
        {
            get { return _enemyIdx; }
        }


        void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();

            //_nav = GetComponent<NavMeshAgent>();
            //_nav.destination = Vector3.zero;
            if (_uiManager == null)
                _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            _animator.SetBool("Death", false);

            //_nav.destination = transform.position;
        }

        void FixedUpdate()
        {
            UpdateAnimation();

            UpdateMovement();




            //updateTargeting

            //PlayAudio

            //udateState

        }

        void UpdateAnimation()
        {
            Vector3 movement = _playerInput.movement;
            float direction = _playerInput.direction;

            float locomotionSpeed = movement.z;
            float sideSpeed = movement.x;

            _animator.speed = _animSpeed;

            _animator.SetFloat("Speed", locomotionSpeed);
            _animator.SetFloat("SideSpeed", sideSpeed);
            _animator.SetFloat("Directrion", direction);

            _currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        }

        void UpdateAttack()
        {
            if (_canAttack)
            {
                _animator.SetTrigger(_idAttack);
                _canAttack = false;
            }
        }

        void UpdateMovement()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitObj;

            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, layerMask))
                {
                   // _nav.destination = hitObj.transform.position;

                }
            }

            Move();
            Jump();
            Rotate();
        }

        void Move()
        {
            Vector3 movement = _playerInput.movement;

            Vector3 fowardMovement = new Vector3(0, 0, movement.z);
            fowardMovement = transform.TransformDirection(fowardMovement);

            Vector3 sideMovement = new Vector3(movement.x, 0, 0);
            sideMovement = transform.TransformDirection(sideMovement);

            if (movement.z > 0.1)
            {
                fowardMovement *= _forwardSpeed;
            }
            else if (movement.z < -0.1)
            {
                fowardMovement *= _backwardSpeed;
            }

            if (movement.x > 0.1 || movement.x < 0.1)
            {
                sideMovement *= _sideSpeed;
            }

            if (_enemyIdx != 0)
            {
                fowardMovement = fowardMovement * _speedRate;
                sideMovement = sideMovement * _speedRate;
            }

            transform.localPosition += (fowardMovement + sideMovement) * Time.deltaTime;
        }

        void Jump()
        {
            Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
                _isGrounded = true;


            if (_playerInput.isJump && _isGrounded && !_animator.GetBool("Jump"))
            {
                if (!_animator.GetBool("Jump"))
                {
                    //_rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
                    _animator.SetBool("Jump", true);

                    return;
                }
            }


            if (_animator.GetBool("Jump"))
            {
                _animator.SetBool("Jump", false);
                _isGrounded = false;
            }
        }

        void Rotate()
        {
            float direction = _playerInput.direction;

            direction *= _rotateSpeed;
            _animator.SetFloat("Direction", direction);

            transform.Rotate(0, direction, 0);

        }

        public void AddEnemy(EnemyController instance)
        {
            if (instance.damageable)
            {
                for (int idx = 0; idx < _enemyIdx; idx++)
                {
                    if (_enemyVec[idx] == instance)
                        return;
                }

                for (int idx = 0; idx <= _enemyIdx; idx++)
                {
                    if (_enemyVec[idx] == null)
                    {
                        _enemyVec[idx] = instance;
                        _enemyIdx++;

                        break;
                    }
                }
            }
        }

        public void RemoveEnemy(EnemyController instance)
        {
            if (instance.damageable)
            {
                for (int idx = 0; idx < _enemyIdx; idx++)
                {
                    if (_enemyVec[idx] == instance)
                    {
                        instance.SetActiveCircle(false);

                        _enemyVec[idx] = null;
                        _enemyIdx--;

                        _uiManager.HideNpcStatus();
                    }
                }

                for (int idx = 0; idx <= _enemyIdx; idx++)
                {
                    if (_enemyVec[idx] != null)
                    {
                        _enemyVec[idx].SetActiveCircle(true);

                        _uiManager.curInstance = _enemyVec[idx];
                        _uiManager.ShowNpcStatus();

                        break;
                    }
                }
            }
        }


        void OnCollisionStay(Collision collision)
        {
            if (collision.collider.tag == "NPC"
                || collision.collider.tag == "Enemy")
            {
                _canInteract = true;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (collision.collider.tag == "NPC"
                || collision.collider.tag == "Enemy")
            {
                _canInteract = false;
            }
        }
    }
}