using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotMountain
{
    public class MouseCusor : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _normalCur;

        [SerializeField]
        private Texture2D _clickCur;

        [SerializeField]
        private Texture2D _attackCur;

        [SerializeField]
        private UnityEngine.Camera _mainCamera;

        [SerializeField]
        private GameObject _npcCam;

        [SerializeField]
        private GameObject _enemyCam;

        [SerializeField]
        private UIManager _uIManager;

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitObj;

            if (Physics.Raycast(ray, out hitObj, Mathf.Infinity))
            {
                if (hitObj.transform.tag.Equals("Enemy"))
                {
                    Cursor.SetCursor(_attackCur, Vector2.zero, CursorMode.ForceSoftware);

                    if (Input.GetMouseButton(0))
                    {
                        _npcCam.SetActive(false);
                        _enemyCam.SetActive(true);

                        _uIManager.InteractWithNpc(hitObj.transform.GetComponent<NpcController>());
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    Cursor.SetCursor(_clickCur, Vector2.zero, CursorMode.ForceSoftware);

                }
                else
                {
                    Cursor.SetCursor(_normalCur, Vector2.zero, CursorMode.ForceSoftware);
                }
            }


        }

        //private void OnMouseOver(Collider collider)
        //{
        //    Debug.Log(collider.tag);

        //    if (collider.tag.Equals("Enemy"))
        //        Cursor.SetCursor(_attackCur, Vector2.zero, CursorMode.Auto);
        //}

        //private void OnMouseExit(Collider collider)
        //{
        //    if (collider.tag.Equals("Enemy"))
        //        Cursor.SetCursor(_normalCur, Vector2.zero, CursorMode.Auto);
        //}
    }
}