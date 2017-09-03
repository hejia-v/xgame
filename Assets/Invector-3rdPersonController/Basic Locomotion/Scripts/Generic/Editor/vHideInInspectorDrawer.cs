using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomPropertyDrawer(typeof(vHideInInspectorAttribute))]
public class vHideInInspectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _Position, SerializedProperty _Property, GUIContent _Label)
    {
        vHideInInspectorAttribute _attribute = attribute as vHideInInspectorAttribute;
        if (_attribute != null)
        {
            var booleamProperty = _Property.serializedObject.FindProperty(_attribute.refbooleanProperty);
            if (booleamProperty != null)
            {
                var valid = _attribute.invertValue ? !booleamProperty.boolValue : booleamProperty.boolValue;
                if(valid) EditorGUI.PropertyField(_Position, _Property);
            }
            else
                EditorGUI.PropertyField(_Position, _Property);
        }
        else
            EditorGUI.PropertyField(_Position, _Property);      
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        vHideInInspectorAttribute _attribute = attribute as vHideInInspectorAttribute;
        if (_attribute != null)
        {
            var booleamProperty = property.serializedObject.FindProperty(_attribute.refbooleanProperty);
            if (booleamProperty != null)
            {
                var valid = _attribute.invertValue ? !booleamProperty.boolValue : booleamProperty.boolValue;
                if (valid) return base.GetPropertyHeight(property, label);
                else return 0;
            }
            else
                return base.GetPropertyHeight(property, label);
        }
        return base.GetPropertyHeight(property, label);
    }
}