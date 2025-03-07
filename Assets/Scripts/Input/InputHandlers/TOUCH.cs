
using System;
using UnityEngine;
namespace InputHandler
{
    public class TOUCH
    {
        protected int layer;
        protected int filter;
        public TOUCH(string param, int filter)
        {
            layer = Int32.Parse(param);
            this.filter = filter;
        }


        protected static RaycastHit2D[] GetHitsFromTouchRay(int layer, int filter)
        {
            if (Input.touchCount > 0)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero, Mathf.Infinity, filter + (1 << layer));

                if (hits.Length > 0)
                {
                    Debug.Log("HITS = " + hits.Length);
                    if (filter == 0 || hits.Length == 1)
                    {
                        int hitLayer = hits[0].collider.gameObject.layer;

                        if (hitLayer == layer)
                        {
                            return new RaycastHit2D[] { hits[0] };
                        }
                    }
                }

            }
            return new RaycastHit2D[0];
        }

        /*public bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.touchCount > 0)
            {
                RaycastHit2D[] hits = GetHitsFromTouchRay(layer);
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
        public TOUCH_DOWN(string param, int filter) : base(param, filter)
        {
        }

        public bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();

            if (PreviouslyTouched == null)
            {
                if (Input.touchCount > 0)
                {
                    RaycastHit2D[] hits = GetHitsFromTouchRay(layer, filter);
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

        public TOUCH_UP(string param, int filter) : base(param, filter)
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
                RaycastHit2D[] hits = GetHitsFromTouchRay(layer, filter);
                if (hits.Length > 0)
                {
                    PreviouslyTouched = (object)hits[0];
                }

            }
            return false;
        }
    }
}