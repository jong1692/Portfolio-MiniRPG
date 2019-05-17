using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HotMountain
{
    public class StatusBar : MonoBehaviour, IPointerExitHandler ,IPointerEnterHandler
    {
        [SerializeField]
        private GameObject _healthText;

        [SerializeField]
        private GameObject _manaText;

        void OnGUI()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _healthText.SetActive(true);
            _manaText.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _healthText.SetActive(false);
            _manaText.SetActive(false);
        }
    }
}