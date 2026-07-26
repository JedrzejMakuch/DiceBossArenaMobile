using System;
using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class UIBackInputSource : MonoBehaviour
    {
        public event Action BackPressed;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackPressed?.Invoke();
            }
        }
    }
}