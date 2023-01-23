using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class GlobalUtils
    {
        private static Camera _mainCamera;

        private static Camera MainCamera
        {
            get 
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera; 
            }
        }

        public static Vector3 GetMouseWorldPosition()
        {
            var mouseWorldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            return mouseWorldPosition;
        }

        public static Vector3 GetRandomDirection() => new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

    }
}
