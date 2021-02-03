using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TEST
{
    public class Test : MonoBehaviour
    {
        public TextRunningHorseLamp text;

        private void Awake()
        {

        }

        private void Start()
        {
            text.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";// ("AAAABCD<emoji=walking0001/>HLH<emoji=walking0002/>TTTTTTTAAAABCD<emoji=walking0001/>HLH<emoji=walking0002/>zzz");
        }

        private void Update()
        {

        }
    }
}