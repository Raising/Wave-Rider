using System;
using UnityEngine;
namespace InputHandler
{
    public abstract class MOUSE : IInputHandler
    {
        protected int layer;
        protected int filter;
        public MOUSE(string param, int filter)
        {
            layer = Int32.Parse(param);
            this.filter = filter;
        }

        public virtual bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            return false;
        }

        protected static RaycastHit2D[] GetHitsFromMouseRay(int layer, int filter)
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
            return new RaycastHit2D[0];
        }
    }
}