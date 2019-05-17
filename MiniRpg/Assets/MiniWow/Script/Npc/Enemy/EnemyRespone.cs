using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class EnemyRespone : MonoBehaviour
    {
        [SerializeField]
        private GameObject _enemy;

        [SerializeField]
        int _maxEnemyCnt = 3;

        [SerializeField]
        int _responeCooltime = 5;

        float _responeTimer = 0f;

        void Update()
        {
            CreateEnemy();
        }

        void CreateEnemy()
        {
            _responeTimer += Time.deltaTime;

            if (_maxEnemyCnt > this.transform.childCount
                && _responeTimer > _responeCooltime)
            {
                Vector3 randPosVec = new Vector3(Random.Range(-4.5f, 4.5f),
                    transform.position.y, Random.Range(-10f, 10f));

                int cnt = gameObject.transform.childCount;
                for (int idx = 0; idx < cnt; idx++)
                {
                    Vector3 enemyPos = gameObject.transform.GetChild(idx).position;
                    float distance = Vector3.Distance(randPosVec + transform.position, enemyPos);

                    if (distance < 3.0f)
                    {
                        return; 
                    }
                }

                Quaternion randRot = Quaternion.Euler(0, Random.Range(0, 360f), 0);

                GameObject child = Instantiate(_enemy, transform.position + randPosVec, randRot) as GameObject;
                child.transform.parent = this.transform;

                _responeTimer = 0;
            }
        }
    }
}