// Copyright (c) 2014 Luminary LLC
// Licensed under The MIT License (See LICENSE for full text)
using UnityEngine;
using System.Collections;

/*
A PropertyAttribute/PropertyDrawer combination that allows for properties in Unity

Example
-------
Unity allows you to serialize private fields with [SerializeField]. Use the new [SetProperty] attribute to have a 
public property set every time the field is modified in Unity's inspector. NOTE: It is okay to have private setters for
public properties. Vanilla classes (i.e. non-MonoBehaviours) also work as well.

    [SerializeField, SetProperty("Number")]
    private float number;
    public float Number
    {
        get
        {
            return number;
        }
        private set
        {
            number = Mathf.Clamp01(value);
        }
    }
*/

public class SetPropertyExample : MonoBehaviour
{
    [System.Serializable]
    public class VanillaClass
    {
        public enum ExtraType
        {
            None,
            HotFudge,
            Mint,
        }

        [SerializeField, SetProperty("Extra")]
        private ExtraType extra;
        public ExtraType Extra
        {
            get
            {
                return extra;
            }
            set
            {
                extra = value;

                // For illustrative purposes
                if (value == ExtraType.None)
                {
                    Debug.Log("Simple!");
                }
                else
                {
                    Debug.Log("Yummy!");
                }
            }
        }
    }

    [SerializeField, SetProperty("Number")]
    private float number;
    public float Number
    {
        get
        {
            return number;
        }
        private set
        {
            number = Mathf.Clamp01(value);
        }
    }

    [SerializeField, SetProperty("Text")]
    private string text;
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            text = value.ToUpper();
        }
    }

    public VanillaClass vanilla;
}
