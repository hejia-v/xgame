using UnityEngine;
using System.Collections;

public class vReadOnlyAttribute : PropertyAttribute
{
    public readonly bool justInPlayMode;
   
    public vReadOnlyAttribute(bool justInPlayMode = true)
    {
       
        this.justInPlayMode = justInPlayMode;
    }
}
