
using System;
using UnityEngine;

namespace InputHandler
{
    public class MOUSE_SECONDARY_CLICK : MOUSE
    {

        public MOUSE_SECONDARY_CLICK(string param) : base(param) { }

        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();

            if (Input.GetMouseButtonUp(1))
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