using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotMountain
{
    public class SkillCool : MonoBehaviour
    {
        [SerializeField]
        private Image _coolImage;

        [SerializeField]
        private Text _coolText;

        [SerializeField]
        private PlayerAttack _playerAttack;

        void Awake()
        {
            _coolImage.fillAmount = 0;

            _coolText.text = "0";
            _coolText.enabled = false;  
        }

        void Update()
        {
            if (_playerAttack == null) return;

            _coolImage.fillAmount = 1 - _playerAttack.attackTimer / _playerAttack.attackDelay;

            float coolTime = _playerAttack.attackDelay - _playerAttack.attackTimer;
            if (coolTime > 0)
            {
                _coolText.enabled = true;
                _coolText.text = coolTime.ToString("0.#");
            }
            else
                _coolText.enabled = false;
        }
    }
}