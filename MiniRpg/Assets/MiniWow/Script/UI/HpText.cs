using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotMountain
{
    public class HpText : MonoBehaviour
    {
        public int hp;

        void Awake()
        {

        }

        void Update()
        {
            Text text = GetComponent<Text>();

            text.text = hp.ToString();

            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime);

            if (text.color.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}