using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
class RequireInterfaceAttribute : PropertyAttribute
{
    public System.Type requiredType { get; private set; }

    public RequireInterfaceAttribute(System.Type type)
    {
        this.requiredType = type;
    }
}
#endif