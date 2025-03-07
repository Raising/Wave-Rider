
using System;
using UnityEngine;

namespace InputHandler
{
    public class MOUSE_HOVER : MOUSE
    {

        public MOUSE_HOVER(string param,int filter) : base(param,filter) { }

        public override bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();

            RaycastHit2D[] hits = GetHitsFromMouseRay(layer,filter);
            if (hits.Length > 0)
            {
                inputInfo = (object)hits[0];
                return true;
            }
            return false;
        }


    }
}
