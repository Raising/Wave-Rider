using UnityEngine;
using System.Collections;
using System;
using InputHandler;

public class InputHandlerHub
{
   public static bool CheckInputState(InputDescriptor inputDescriptor, out object inputInfo)
    {        
        IInputHandler handler = GetInputHandler(inputDescriptor);

        bool inputSucces = handler.CheckInput(out inputInfo);
        return inputSucces;
    }


    private static IInputHandler GetInputHandler(InputDescriptor inputDescriptor)
    {
        IInputHandler handler = (IInputHandler)Activator.CreateInstance(inputDescriptor.handler, inputDescriptor.param);
        return handler;
    }
}
