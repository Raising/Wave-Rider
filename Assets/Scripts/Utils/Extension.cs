using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class Extension
    {
        public static Selector<T> ToSelector<T>(this IEnumerable<T> collection) where T : ISelectableObject<T>
        {
            Selector<T> selector = new Selector<T>();
            foreach (T seleccionable in collection)
            {
                selector.Add(seleccionable);
            }

            List<int> p = new List<int>().Where(el => el < 5).ToList();

            return selector;
        }


        public static async Task<T> MakeSelection<T>(this IEnumerable<T> collection,string description = "") where T : ISelectableObject<T>
        {
            return await collection.ToSelector().MakeSelection(description);
        }
        
    }
}
