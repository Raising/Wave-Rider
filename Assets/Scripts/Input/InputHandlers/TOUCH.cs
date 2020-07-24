
using System;
using UnityEngine;
namespace InputHandler
{
    public class TOUCH
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

        /*public bool CheckInput(out object inputInfo)
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
        }*/
    }

    public class TOUCH_DOWN : TOUCH, IInputHandler
    {
        private static object PreviouslyTouched = null;
        public TOUCH_DOWN(string param) : base(param)
        {
        }

        public  bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();

            if (PreviouslyTouched == null)
            {
                if (Input.touchCount > 0)
                {
                    RaycastHit[] hits = GetHitsFromTouchRay(layer);
                    if (hits.Length > 0)
                    {
                        inputInfo = (object)hits[0];
                        PreviouslyTouched = inputInfo;
                        return true;
                    }
                }
                else
                {
                    PreviouslyTouched = null;
                }
            }
            return false;
        }
    }

    public class TOUCH_UP : TOUCH, IInputHandler
    {
        private static object PreviouslyTouched = null;

        public TOUCH_UP(string param) : base(param)
        {
        }

        public bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.touchCount == 0 && PreviouslyTouched != null)
            {
                inputInfo = PreviouslyTouched;
                PreviouslyTouched = null;
                return true;
                
            }

            if (Input.touchCount > 0)
            {
                RaycastHit[] hits = GetHitsFromTouchRay(layer);
                if (hits.Length > 0)
                {
                    PreviouslyTouched = (object)hits[0];
                }

            }
            return false;
        }
    }
}