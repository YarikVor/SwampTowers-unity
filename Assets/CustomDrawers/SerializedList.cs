#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;

public class SerializedList<TResult, TUnityObject> where TUnityObject : UnityEngine.Object
{
    private readonly SerializedProperty _property;
    private readonly Action<TResult, TUnityObject, int> _setter;
    private readonly Func<TUnityObject, int, TResult> _getter;

    private readonly SerializedObject _serializedObject;

    private TUnityObject TargetObject => (TUnityObject)_property.serializedObject.targetObject;

    public SerializedList(
        SerializedProperty property,
        Func<TUnityObject, int, TResult> getter,
        Action<TResult, TUnityObject, int> setter
    )
    {
        _property = property;
        _getter = getter;
        _setter = setter;
        _serializedObject = property.serializedObject;
    }

    public IEnumerator<SerializedProperty> GetEnumerator()
    {
        return (IEnumerator<SerializedProperty>)_property.GetEnumerator();
    }

    public void Add(TResult item)
    {
        var index = _property.arraySize++;
        ApplyChanges();
        this[index] = item;
    }

    public void Add()
    {
        _property.arraySize++;
        ApplyAndUpdate();
    }

    public void Switch(int indexFrom, int indexTo)
    {
        _property.MoveArrayElement(indexFrom, indexTo);
    }

    public void Clear()
    {
        _property.ClearArray();
    }

    public int Count
    {
        get => _property.arraySize;
        set => _property.arraySize = value;
    }

    public bool IsReadOnly => false;

    public void Insert(int index, TResult item)
    {
        _property.GetArrayElementAtIndex(index).managedReferenceValue = item;
    }

    public void Insert(int index)
    {
        _property.InsertArrayElementAtIndex(index);
    }

    public void RemoveAt(int index)
    {
        ApplyAndUpdate(() => _property.DeleteArrayElementAtIndex(index));
    }

    private void ApplyAndUpdate(Action action)
    {
        action();
        ApplyAndUpdate();
    }

    private void ApplyAndUpdate()
    {
        ApplyChanges();
        Update();
    }

    private void ApplyChanges()
    {
        _serializedObject.ApplyModifiedProperties();
    }

    private void Update()
    {
        _serializedObject.Update();
    }

    public SerializedProperty GetSerializedPropertyByIndex(int index)
    {
        return _property.GetArrayElementAtIndex(index);
    }

    public TResult this[int index]
    {
        get => _getter.Invoke(TargetObject, index);
        set => ApplyAndUpdate(() => _setter.Invoke(value, TargetObject, index));
    }
}

#endif