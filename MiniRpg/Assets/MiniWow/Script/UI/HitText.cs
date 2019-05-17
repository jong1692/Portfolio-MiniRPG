using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotMountain
{
    public class HitText : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerCam;

        [SerializeField]
        private GameObject _npcCam;

        [SerializeField]
        private GameObject _textObj;

        void Awake()
        {

        }

        void Update()
        {
        }

        public void ShowPlayerHpText(int hp)
        {
            GameObject child = Instantiate(_textObj, _playerCam.transform) as GameObject;
            child.transform.parent = _playerCam.transform;

            if (hp > 0)
                child.GetComponent<Text>().color = Color.green;
            else
                child.GetComponent<Text>().color = Color.red;

            child.GetComponent<HpText>().hp = hp;
        }

        public void ShowEnemyHpText(int hp)
        {
            GameObject child = Instantiate(_textObj, _npcCam.transform) as GameObject;
            child.transform.parent = _npcCam.transform;

            if (hp > 0)
                child.GetComponent<Text>().color = Color.green;
            else
                child.GetComponent<Text>().color = Color.red;

            child.GetComponent<HpText>().hp = hp;
        }
    }
}