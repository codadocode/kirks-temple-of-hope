using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGroupData<T> where T : Collider2D
{
    protected BaseContainerData<List<T>, T> colliderContainerData;
    protected bool hasTagFilter = false;
    protected string[] tags;

    public ColliderGroupData(string[] tags = null)
    {
        this.colliderContainerData = new BaseContainerData<List<T>, T>(new List<T>());

        if (tags != null)
        {
            this.tags = tags;
            hasTagFilter = true;
        }
    }

    public virtual void AddCollider(T collider)
    {
        if (!HasTag(collider)) return;
        
        if (this.colliderContainerData.Collection.Contains(collider)) return;
        this.colliderContainerData.AddObject(collider);
    }

    public virtual void RemoveCollider(T collider)
    {
        if (!this.colliderContainerData.Collection.Contains(collider)) return;

        this.colliderContainerData.Collection.Remove(collider);
    }

    private bool HasTag(T collider)
    {
        if (!hasTagFilter) return true;

        for (int i = 0; i < this.tags.Length; i++)
        {
            string actualTag = this.tags[i];
            if (collider.gameObject.CompareTag(actualTag)) return true;
        }

        return false;
    }

    public virtual bool FindTag(string tag)
    {
        if (hasTagFilter)
        {
            if (this.colliderContainerData.Collection.Count > 0) return true;
        }
        else
        {
            if (this.colliderContainerData.Collection.Count > 0)
            {
                for (int i = 0; i < this.colliderContainerData.Collection.Count; i++)
                {
                    T actualCollider2D = this.colliderContainerData.Collection[i];
                    if (actualCollider2D.gameObject.CompareTag(tag)) return true;
                }
            }
        }
        
        return false;
    }

    public void ResetColliderGroupData()
    {
        this.colliderContainerData.ClearCollection();
    }
}
