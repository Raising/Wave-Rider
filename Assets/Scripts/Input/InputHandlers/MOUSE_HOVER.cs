
using System;
using UnityEngine;

namespace InputHandler
{
    public class MOUSE_HOVER : MOUSE
    {

        public MOUSE_HOVER(string param) : base(param) { }

        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();

            RaycastHit[] hits = GetHitsFromMouseRay(layer);
            if (hits.Length > 0)
            {
                inputInfo = (object)hits[0];
                return true;
            }
            return false;
        }


    }
}
