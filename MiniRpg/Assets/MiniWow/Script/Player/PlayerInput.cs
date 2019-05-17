using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class PlayerInput : MonoBehaviour
    {
        Vector3 _movement;
        float _direction;
        Vector3 _camVec;

        bool _isAttack;
        bool _isJump;
        bool _isPause;
        bool _isMove;
        bool _isTurn;

        [SerializeField]
        float _attackDelay = 0.03f;

        public Vector3 movement
        {
            get { return _movement; }
        }

        public float direction
        {
            get { return _direction; }
        }



        public Vector3 camInput
        {
            get { return _camVec; }
        }

        public bool isAttack
        {
            get { return _isAttack; }
        }

        public bool isJump
        {
            get { return _isJump; }
        }

        public bool isPuase
        {
            get { return _isPause; }
        }

        void Awake()
        {

        }


        void Update()
        {
            UpdateInput();
        }

        void UpdateInput()
        {
            _movement.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            _direction = Input.GetAxis("Direction");

            _camVec = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

            _isJump = Input.GetButton("Jump");

            //_isPause = Input.GetButton("Pause");

            _isAttack = Input.GetButton("Fire1");
        }

    }
}