using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField]
        private int _damage = 10;

        private EnemyController _enemyController;
        
        private bool _attacked = false;

        public bool attacked
        {
            get { return _attacked; }
            set { _attacked = value; }
        }


        void Awake()
        {
            _enemyController = transform.parent.parent.GetComponent<EnemyController>();
        }

        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && _attacked == false)
            {
                other.GetComponent<PlayerHp>().Damaged(_damage);
                _attacked = true;
            }
        }

    }
}