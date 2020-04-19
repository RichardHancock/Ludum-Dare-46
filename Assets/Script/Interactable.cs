using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool InteractableFlag = false;
    protected bool InsertableFlag = false;

    public virtual bool IsInteractable() => InteractableFlag;
    public virtual bool IsInsertable() => InsertableFlag;

    public abstract bool Interact();

    public abstract bool InsertItem(GameObject item);
}
