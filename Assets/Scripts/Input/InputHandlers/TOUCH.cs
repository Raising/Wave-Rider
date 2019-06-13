
using System;
using UnityEngine;
namespace InputHandler
{
    public class TOUCH : IInputHandler
    {
        protected int layer;
        public TOUCH(string param)
        {
            layer = Int32.Parse(param);
        }
        

        protected static RaycastHit[] GetHitsFromTouchRay(int layer)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                return Physics.RaycastAll(ray, Mathf.Infinity, 1 << layer);
            }
            return new RaycastHit[0];
        }

        public bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.touchCount > 0)
            {
                RaycastHit[] hits = GetHitsFromTouchRay(layer);
                if (hits.Length > 0)
                {
                    inputInfo = (object)hits[0];
                    return true;
                }
            }
            return false;
        }
    }
}