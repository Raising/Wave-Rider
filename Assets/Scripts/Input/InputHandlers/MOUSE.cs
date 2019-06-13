
using System;
using UnityEngine;
namespace InputHandler
{
    public abstract class MOUSE : IInputHandler
    {
        protected int layer;
        public MOUSE(string param)
        {
            layer = Int32.Parse(param);
        }

        public virtual bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            return false;
        }

        protected static RaycastHit[] GetHitsFromMouseRay(int layer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.RaycastAll(ray, Mathf.Infinity, 1 << layer);
        }
    }
}