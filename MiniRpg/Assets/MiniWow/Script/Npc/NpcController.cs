using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace HotMountain
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class NpcController : MonoBehaviour
    {
        protected NpcController _instance;

        [SerializeField]
        private GameObject _activedCircle;

        [SerializeField]
        protected PlayerController _playerContoller;

        [SerializeField]
        protected PlayerHp _playerHp;

        [SerializeField]
        protected String _name = "NPC";

        [SerializeField]
        protected float _maxHp = 100f;

        [SerializeField]
        protected float _healthPoint = 100f;

        [SerializeField]
        protected float _manaPoint = 100f;

        [SerializeField]
        protected bool _damageable = false;

        [SerializeField]
        protected bool _useDialogue = false;

        protected Animator _animator;

        protected bool _isDeath = false;

        protected bool _isCombat = false;

        [SerializeField]
        Material _combatMat;

        [SerializeField]
        Material _peaceMat;

        public String npcName
        {
            get { return _name; }
        }

        public float healtPoint
        {
            get { return _healthPoint; }
        }

        public float maxHp
        {
            get { return _maxHp; }
        }

        public float manaPoint
        {
            get { return _manaPoint; }
        }

        public bool damageable
        {
            get { return _damageable; }
        }

        public bool isCombat
        {
            get { return _isCombat; }
        }

        public bool useDialogue
        {
            get { return _useDialogue; }
        }

        public bool isDeath
        {
            get { return _isDeath; }
        }

        public NpcController instance
        {
            get { return _instance; }
        }


        protected void Init()
        {
            _instance = this;

            if (_playerContoller == null)
                _playerContoller = GameObject.Find("Player").GetComponent<PlayerController>();

            if (_playerHp == null)
                _playerHp = GameObject.Find("Player").GetComponent<PlayerHp>();

            if (_activedCircle.activeInHierarchy)
                _activedCircle.SetActive(false);

            _animator = this.GetComponent<Animator>();
        }

        void Update()
        {

        }

        public void SetActiveCircle(bool active)
        {
            _activedCircle.SetActive(active);
        }

        public void ChangeActiveCircle(bool isCombat = false)
        {
            if (isCombat)
                _activedCircle.GetComponent<MeshRenderer>().material = _combatMat;
            else
                _activedCircle.GetComponent<MeshRenderer>().material = _peaceMat;
        }

        protected void Damaged(int damage, string aniName, GameObject obj)
        {
            if (_damageable)
            {
                _healthPoint -= damage;

                if (_healthPoint <= 0)
                {
                    _healthPoint = 0;
                    StartCoroutine(Death(true, aniName, obj));
                }
            }
        }

        IEnumerator Death(bool isDestroy, string aniName, GameObject obj)
        {
            _playerContoller.RemoveEnemy(_instance as EnemyController);

            _animator.SetBool("Death", true);
            _damageable = false;
            _isDeath = true;

            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(aniName))
            {
                yield return null;
            }

            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f)
            {
                yield return null;
            }

            Destroy(obj);
        }
    }
}