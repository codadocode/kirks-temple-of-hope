using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionModule : BaseModule
{
    public event Action<Collision2D> EvtCollisionEnter2D;
    public event Action<Collision2D> EvtCollisionExit2D;
    public event Action<Collider2D> EvtTriggerEnter2D;
    public event Action<Collider2D> EvtTriggerExit2D;

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        this.EvtCollisionEnter2D?.Invoke(other);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        this.EvtTriggerEnter2D?.Invoke(other);
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
        this.EvtCollisionExit2D?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this.EvtTriggerExit2D?.Invoke(other);
    }
}
