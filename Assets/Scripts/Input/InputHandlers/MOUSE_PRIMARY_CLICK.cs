
using System;
using UnityEngine;

namespace InputHandler
{
    public class MOUSE_PRIMARY_CLICK : MOUSE
    {
        public MOUSE_PRIMARY_CLICK(string param) : base(param) { }

        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit[] hits = GetHitsFromMouseRay(layer);
                if (hits.Length > 0)
                {
                    inputInfo = (object)hits[0];
                    return true;
                }
            }
            return false;
        }
    }

    public class MOUSE_PRIMARY_DOWN : MOUSE
    {
        public MOUSE_PRIMARY_DOWN(string param) : base(param) { }
        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit[] hits = GetHitsFromMouseRay(layer);
                if (hits.Length > 0)
                {
                    inputInfo = (object)hits[0];
                    return true;
                }
            }

            return false;
        }
    }
    public class MOUSE_PRIMARY_UP : MOUSE
    {
        public MOUSE_PRIMARY_UP(string param) : base(param) { }
        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit[] hits = GetHitsFromMouseRay(layer);
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
