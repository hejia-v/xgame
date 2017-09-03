using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using System;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using Invector;

public class vEditorHelper : Editor
{
    /// <summary>
    /// Get PropertyName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyLambda">You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'</param>
    /// <returns></returns>
    public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
    {
        var me = propertyLambda.Body as MemberExpression;

        if (me == null)
        {
            throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
        }

        return me.Member.Name;
    }

    /// <summary>
    /// Check if type is a <see cref="UnityEngine.Events.UnityEvent"/>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsUnityEventyType(Type type)
    {
        if (type.Equals(typeof(UnityEngine.Events.UnityEvent))) return true;
        if (type.BaseType.Equals(typeof(UnityEngine.Events.UnityEvent))) return true;
        if (type.Name.Contains("UnityEvent") || type.BaseType.Name.Contains("UnityEvent")) return true;
        return false;
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(vMonoBehaviour), true)]
public class vEditorBase : Editor
{
    #region Variables   
    public string[] ignoreEvents;
    public string[] notEventProperties;
    public string[] ignore_vMono = new string[] { "openCloseWindow", "openCloseEvents" };
    public vClassHeaderAttribute headerAttribute;
    public GUISkin skin;
    public Texture2D m_Logo;
    #endregion

    protected virtual void OnEnable()
    {
        var targetObject = serializedObject.targetObject;
        var hasAttributeHeader = targetObject.GetType().IsDefined(typeof(vClassHeaderAttribute), true);
        if (hasAttributeHeader)
        {
            var attributes = Attribute.GetCustomAttributes(targetObject.GetType(), typeof(vClassHeaderAttribute), true);
            if (attributes.Length > 0)
                headerAttribute = (vClassHeaderAttribute)attributes[0];
        }

        skin = Resources.Load("skin") as GUISkin;
        m_Logo = Resources.Load("icon_v2") as Texture2D;
        var prop = serializedObject.GetIterator();

        if (headerAttribute != null && ((vMonoBehaviour)target) != null)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            List<string> events = new List<string>();

            while (prop.NextVisible(true))
            {
                var fieldInfo = targetObject.GetType().GetField(prop.name, flags);
                if (fieldInfo != null)
                {

                    if ((vEditorHelper.IsUnityEventyType(fieldInfo.FieldType) && !events.Contains(fieldInfo.Name)))
                    {
                        events.Add(fieldInfo.Name);

                    }
                }
            }

            ignoreEvents = events.vToArray();

            m_Logo = Resources.Load(headerAttribute.iconName) as Texture2D;
        }
    }

    protected bool openCloseWindow
    {
        get
        {
            return serializedObject.FindProperty("openCloseWindow").boolValue;
        }
        set
        {
            var _openClose = serializedObject.FindProperty("openCloseWindow");
            if (_openClose != null && value != _openClose.boolValue)
            {
                _openClose.boolValue = value;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    protected bool openCloseEvents
    {
        get
        {
            var _openCloseEvents = serializedObject.FindProperty("openCloseEvents");
            return _openCloseEvents != null ? _openCloseEvents.boolValue : false;
        }
        set
        {
            var _openCloseEvents = serializedObject.FindProperty("openCloseEvents");
            if (_openCloseEvents != null && value != _openCloseEvents.boolValue)
            {
                _openCloseEvents.boolValue = value;
                serializedObject.ApplyModifiedProperties();
            }
        }
    }

    public override void OnInspectorGUI()
    {
       
        if (headerAttribute == null)
        {
            if (((vMonoBehaviour)target) != null)
                DrawPropertiesExcluding(serializedObject, ignore_vMono);
            else
                base.OnInspectorGUI();
        }
        else
        {

            GUILayout.BeginVertical(headerAttribute.header, skin.window);

            GUILayout.Label(m_Logo,skin.label, GUILayout.MaxHeight(25));
            if (headerAttribute.openClose)
            {
                openCloseWindow = GUILayout.Toggle(openCloseWindow, openCloseWindow ? "Close Properties" : "Open Properties", EditorStyles.toolbarButton);
            }

            if (!headerAttribute.openClose || openCloseWindow)
            {
                if (headerAttribute.useHelpBox)
                    EditorGUILayout.HelpBox(headerAttribute.helpBoxText, MessageType.Info);

                if (ignoreEvents != null && ignoreEvents.Length > 0)
                {
                    var ignoreProperties = ignoreEvents.Append(ignore_vMono);
                    DrawPropertiesExcluding(serializedObject, ignoreProperties);
                    openCloseEvents = GUILayout.Toggle(openCloseEvents, (openCloseEvents ? "Close " : "Open ") + "Events ", skin.button);
                   

                    if (openCloseEvents)
                    {

                        foreach (string propName in ignoreEvents)
                        {
                            var prop = serializedObject.FindProperty(propName);
                            if (prop != null)
                                EditorGUILayout.PropertyField(prop);
                        }
                    }
                }
                else
                {
                    var ignoreProperties = ignoreEvents.Append(ignore_vMono);
                    DrawPropertiesExcluding(serializedObject, ignoreProperties);
                }
            }
            EditorGUILayout.EndVertical();

        }
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(serializedObject.targetObject);
        }
       
    }
}