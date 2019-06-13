using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class InputRequest
{
    public Dictionary<string, InputDescriptor> inputs;
    public List<InputDescriptor> maskedInputs;

    public InputRequest(List<InputDescriptor> actions, List<InputDescriptor> maskedInputs)
    {
        this.maskedInputs = maskedInputs;
        FillInputs(actions);

    }

    public InputRequest(List<InputDescriptor> actions)
    {
        maskedInputs = new List<InputDescriptor>();
        FillInputs(actions);
    }

    public InputRequest()
    {
        maskedInputs = new List<InputDescriptor>();
        inputs = new Dictionary<string, InputDescriptor>();
    }

 
    private void FillInputs(List<InputDescriptor> actions)
    {
        inputs = new Dictionary<string, InputDescriptor>();
        foreach (InputDescriptor descriptor in actions)
        {
            inputs.Add(descriptor.Name, descriptor);
        }
    }

}
