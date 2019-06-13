using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Events;
using System;



/***
 *  
 *  Intended Use
 *  Selector.
 * 
 * 
 */

public delegate void Resolutor<T>(T input);


public class Selector<T> : List<T> where T : ISelectableObject<T>
{
    private List<T> options;

    public Selector()
    {

    }

    public Selector(int EntityLocationLayer,T classType )
    {
        
    }

    public async Task<T> MakeSelection( string description = "")
    {
        T result = default(T);
        ShowDescription(description);
        var makingSelection = new Task<T>(() => { return result; });

        Resolutor<T> completeSelection = (selection) =>
        {
            result = selection;
            makingSelection.Start();
         
        };

        List<VerificableAction> unListeners = this
            .Select(selectable => selectable.RequestSelection(completeSelection))
            .ToList(); // Si no se hace el to list no se generan los RequestSelection



        return await makingSelection.ContinueWith(  
            (task,obj) => {
                unListeners.Select(unListener => unListener()).ToList();
                return task.Result;
                }, TaskContinuationOptions.AttachedToParent);
            
    }
    
    private void ShowDescription(string description)
    {
      //  TextMeshProUGUI descriptionComponent = GameObject.FindGameObjectWithTag("InfoText").GetComponent<TextMeshProUGUI>();
      //  descriptionComponent.text = description;
    }
}
