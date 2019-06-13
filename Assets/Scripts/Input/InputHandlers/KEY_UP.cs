
using UnityEngine;
namespace InputHandler
{
    public class KEY_UP : IInputHandler
    {
        private string key;
        public KEY_UP(string param)
        {
            key = param;
        }

        public bool CheckInput(out object inputInfo)
        {
            inputInfo = new object();
            if (Input.GetKeyUp(key))
            {
                return true;
            }
            return false;
        }
    }
}