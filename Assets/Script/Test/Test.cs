using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Test : MonoBehaviour
    {
        public Text text;

        private void Awake()
        {

        }

        private void Start()
        {
            text.SetTextWithEmoji("AAAABCD<emoji=walking0001/>HLH<emoji=walking0002/>TTTTTTTAAAABCD<emoji=walking0001/>HLH<emoji=walking0002/>zzz");
        }

        private void Update()
        {

        }
    }
}