using UnityEngine;
using UnityEditor;

namespace InputHandler
{
    public interface IInputHandler
    {
        bool CheckInput(out object inputInfo);
    }
}