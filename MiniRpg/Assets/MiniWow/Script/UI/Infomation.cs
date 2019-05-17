using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace HotMountain
{
    public class Infomation : MonoBehaviour
    {
        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _level;

        [SerializeField]
        private Text _Job;

        [SerializeField]
        private Text _tribe;

        [SerializeField]
        private GameObject _infoFrame;

        [SerializeField]
        private UnityEngine.Camera _mainCamera;

        void Start()
        {

        }

        void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitObj;

            int layerMask = 1 << 8 | 1 << 10 | 1 << 9;

            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity, layerMask))
            {
                _infoFrame.SetActive(true);
                PlayerInfomation.Infomation info = hitObj.transform.GetComponent<PlayerInfomation>().info;

                _name.text = info._name;
                _Job.text = info._job;
                _level.text = info._level;
                _tribe.text = info._tribe;

                _infoFrame.transform.position = Input.mousePosition + new Vector3(300/2, -300/2);
            }
            else
            {
                _infoFrame.SetActive(false);
            }
        }
    }
}