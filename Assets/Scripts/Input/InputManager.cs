using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    private static Dictionary<string, InputDescriptor> listenedInputs;
    private static List<InputRequest> inputRequests;

    private static InputManager inputManager;
    public static InputManager Instance
    {
        get
        {
            if (!inputManager)
            {
                inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;

                if (!inputManager)
                {
                    Debug.LogError("There needs to be one active InputManger script on a GameObject in your scene.");
                }
                else
                {
                    inputManager.Init();
                }
            }

            return inputManager;
        }
    }

    void Init()
    {
        if (inputRequests == null)
        {
            listenedInputs = new Dictionary<string, InputDescriptor>();

            inputRequests = new List<InputRequest>();
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        ServeInputs();
    }


    public static void AddInputRequest(InputRequest inputRequest)
    {
        Instance.Init();

        inputRequests.Add(inputRequest);
        ResetListeningInputs();
    }

    public static void RemoveInputRequest(InputRequest inputRequest)
    {
        inputRequests.Remove(inputRequest);
        ResetListeningInputs();
    }


    private static void ResetListeningInputs()
    {
        listenedInputs.Clear();

        foreach (InputRequest request in inputRequests)
        {
            List<string> inputsToDetect = request.inputs.Keys.ToList();
           // List<InputDescriptor> inputsToMask = request.maskedInputs;

            foreach (string inputDescriptorName in inputsToDetect)
            {
                //Used direct Assignation because we want to overwrite the previous value if it exists, Using Add whould lead to an error.
                listenedInputs[inputDescriptorName] = request.inputs[inputDescriptorName];
            }

            foreach (InputDescriptor inputDescriptor in request.maskedInputs)
            {
                listenedInputs.Remove(inputDescriptor.Name);
            }
        }
    }


    private static void ServeInputs()
    {
        List<InputDescriptor> inputsToDetect = listenedInputs.Values.ToList();
        foreach (InputDescriptor inputDescriptor in inputsToDetect)
        {
            object inputParameter; 
            if (InputHandlerHub.CheckInputState(inputDescriptor, out inputParameter))
            {
                inputDescriptor.callback.Invoke(inputParameter);
            }
            else if (inputDescriptor.failCallback != null)
            {
                inputDescriptor.failCallback.Invoke(inputParameter);
            }
        }
    }
}


