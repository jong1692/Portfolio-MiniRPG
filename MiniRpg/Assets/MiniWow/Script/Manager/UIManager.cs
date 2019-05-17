using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotMountain
{
    public class UIManager : MonoBehaviour
    {
        [Serializable]
        public struct Status
        {
            public GameObject _gameObj;
            public Text _name;
        }

        [Serializable]
        public struct Dialogue
        {
            public GameObject _gameObj;
            public Text _npcName;
            public Text _text;
        }

        [Serializable]
        public struct Respone
        {
            public GameObject _gameObj;
            public Image _image;
        }

        [Serializable]
        public struct Warning
        {
            public GameObject _gameObj;
            public Text _text;
        }

        public Status _interactiveStatus;

        public Dialogue _dialogue;

        public Respone _respone;

        public Warning _warning;

        public UnityEngine.Camera _mainCamera;


        private NpcController _pastInstance;
        private NpcController _curInstance;

        [SerializeField]
        private GameObject _npcCam;

        [SerializeField]
        private GameObject _enemyCam;

        public NpcController curInstance
        {
            get { return _curInstance; }
            set { _curInstance = value; }
        }

        [SerializeField]
        private int _maxHp = 100;

        private int _healthPoint;

        [SerializeField]
        private SimpleHealthBar _healthBar;

        [SerializeField]
        private PlayerHp _playerHp;

        float _timer = 0;
        float _delay = 1.0f;

        private void FixedUpdate()
        {
            if (_curInstance != null)
            {
                _healthPoint = (int)_curInstance.healtPoint;
                _maxHp = (int)_curInstance.maxHp;

                _healthBar.UpdateBar(_healthPoint, _maxHp);

                if (_healthPoint <= 0)
                    _healthBar.transform.parent.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hitObj;

                int layerMask = 1 << 10 | 1<< 9;

                if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, layerMask))
                {
                    //if (hitObj.transform.tag.Equals("Enemy"))
                    //{
                    //    _npcCam.SetActive(false);
                    //    _enemyCam.SetActive(true);

                    //    InteractWithNpc(hitObj.transform.GetComponent<NpcController>());
                    //}

                     if (hitObj.transform.tag.Equals("NettralityNpc"))
                    {
                        hitObj.transform.GetComponent
                            <NeutralityNpcController>().CheckDistanceWithPlayer();

                        _npcCam.SetActive(true);
                        _enemyCam.SetActive(false);

                        InteractWithNpc(hitObj.transform.GetComponent<NpcController>());
                    }
                }
            }
        }

        public void ShowResponeConsole()
        {
            _respone._gameObj.SetActive(true);
            _respone._image.enabled = true;
        }

        public void ResponePlayer()
        {
            _respone._gameObj.SetActive(false);
            _respone._image.enabled = false;

            _playerHp.ResponePlayer();
        }

        public void ShowWarningMsg(string text)
        {
            _warning._gameObj.SetActive(true);
            _warning._text.text = text;

            StopAllCoroutines();
            StartCoroutine(HideWaringMsg());
        }

        IEnumerator HideWaringMsg()
        {
            _timer = 0;

            while (_timer < _delay)
            {
                _timer += Time.deltaTime;

                yield return null;
            }

            _warning._gameObj.SetActive(false);
        }

        public void InteractWithNpc(NpcController instance)
        {
            if (instance == null) return;

            _curInstance = instance;

            ShowNpcStatus();
            ShowNpcDialogue();
        }

        public void HideNpcStatus()
        {
            _curInstance = null;
            _interactiveStatus._gameObj.SetActive(false);
        }

        public void ShowNpcStatus()
        {
            if (_curInstance != _pastInstance && _pastInstance != null)
                _pastInstance.SetActiveCircle(false);

            _interactiveStatus._gameObj.SetActive(true);
            _interactiveStatus._name.text = _curInstance.npcName;

            _curInstance.SetActiveCircle(true);
            _curInstance.ChangeActiveCircle(_curInstance.isCombat);

            _pastInstance = _curInstance;
        }

        private void ShowNpcDialogue()
        {
            if (!_curInstance.useDialogue) return;

            _dialogue._npcName.text = _curInstance.npcName;
            _dialogue._gameObj.SetActive(true);
        }

        public void CloseDialogue()
        {
            if (_dialogue._gameObj.activeInHierarchy)
                _dialogue._gameObj.SetActive(false);
        }
    }
}