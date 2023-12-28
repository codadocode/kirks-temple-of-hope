using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContainerData<C, T> where C : ICollection<T>
{
    protected C collection;

    public C Collection
    {
        get => this.collection;
    }

    public BaseContainerData(C genericCollection)
    {
        this.collection = genericCollection;
    }

    public virtual void AddObject(T objectToAdd)
    {
        this.collection.Add(objectToAdd);
    }

    public virtual void RemoveObject(T objectToRemove)
    {
        this.collection.Remove(objectToRemove);
    }

    public void ClearCollection()
    {
        this.collection.Clear();
    }
}
