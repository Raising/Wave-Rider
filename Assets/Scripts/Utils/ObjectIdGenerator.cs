using UnityEngine;

public class ObjectIdGenerator
{
    private static int currentId = 0;

    public static string getId()
    {
        currentId += 1;
        string stringNumber = currentId.ToString();
        return new string('0',12 - stringNumber.Length)  + stringNumber;
    }
}