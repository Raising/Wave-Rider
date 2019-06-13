using UnityEngine;
using System.Collections;

public interface ISelectorImpl
{
    T Select<T>();
}
