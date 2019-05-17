using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHealthBar_SpaceshipExample;

namespace HotMountain
{
    public class PlayerHp : MonoBehaviour
    {
        [SerializeField]
        private int _maxHp = 120;
        private int _healthPoint;
        private int _manaPoint = 100;
        private bool _isPlayerDeath = false;
        private bool _isDamageable = true;
        private Animator _animator;

        [SerializeField]
        private SimpleHealthBar _healthBar;

        [SerializeField]
        private UIManager _uiMgr;

        private float _healDelay = 1f;
        private float _healTimer = 0;
        private int _healAmount = 20;

        public bool isDamageable
        {
            get { return _isDamageable; }
        }

        void Awake()
        {
            _animator = GetComponent<Animator>();

            Init();
        }

        public void ResponePlayer()
        {
            Init();
        }

            public void Init()
        {
            if (GetComponent<PlayerInput>().isActiveAndEnabled == false)
                GetComponent<PlayerInput>().enabled = true;

            if (GetComponent<PlayerController>().isActiveAndEnabled == false)
                GetComponent<PlayerController>().enabled = true;

            if (transform.GetComponentInChildren<PlayerAttack>().isActiveAndEnabled == false)
                transform.GetComponentInChildren<PlayerAttack>().enabled = true;

            _healthPoint = _maxHp;
            _manaPoint = 100;

            _isPlayerDeath = false;
            _isDamageable = true;

            _healthBar.UpdateBar(_healthPoint, _maxHp);
        }

        void FixedUpdate()
        {
            _healthBar.UpdateBar(_healthPoint, _maxHp);

            Heal();
        }

        void Heal()
        {
            if (_isPlayerDeath) return;

            if (GetComponent<PlayerController>().enemyIdx == 0)
            {
                _healTimer += Time.deltaTime;
                if (_healTimer > _healDelay && _healthPoint < _maxHp)
                {
                    _healTimer = 0;

                    _healthPoint += _healAmount;

                    _uiMgr.GetComponent<HitText>().ShowPlayerHpText(_healAmount);

                    if (_healthPoint > _maxHp)
                        _healthPoint = _maxHp;
                }
            }
            else
                _healTimer = 0;
        }

        public void Damaged(int damage)
        {
            if (_isDamageable == false) return;

            _healthPoint -= damage;

            _uiMgr.GetComponent<HitText>().ShowPlayerHpText(-damage);

            CheckPlayerDeath();
        }

        IEnumerator Death()
        {
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            transform.GetComponentInChildren<PlayerAttack>().enabled = false;

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Death"))
            {

                yield return null;
            }

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
            {

                yield return null;
            }
            _animator.SetBool("Death", false);

            _uiMgr.ShowResponeConsole();

        }

        private void CheckPlayerDeath()
        {
            if (_healthPoint <= 0 && _isPlayerDeath == false)
            {
                _isPlayerDeath = true;
                _isDamageable = false;
                _animator.SetBool("Death", true);

                StartCoroutine(Death());
            }
        }
    }
}