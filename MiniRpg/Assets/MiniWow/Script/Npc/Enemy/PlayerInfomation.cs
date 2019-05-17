using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HotMountain
{
    public class PlayerInfomation : MonoBehaviour
    {
        [Serializable]
        public struct Infomation
        {
            public string _name;
            public string _level;
            public string _job;
            public string _tribe;
        };

        public Infomation _info;

        public Infomation info
        {
            get { return _info; }
        }
    }
}