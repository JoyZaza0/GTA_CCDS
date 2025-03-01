using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyComponent : MonoBehaviour
{
    [SerializeField] private Transform _parent;


    [ContextMenu("RemoveVisuals")]
    public void RemoveMeshVisual()
    {
        foreach (Transform child in _parent)
        {
            if (child.childCount > 0)
            {
                foreach (Transform childOfChild in child)
                {
                    if (childOfChild.TryGetComponent(out MeshRenderer meshRenderer))
                    {
                        if (!meshRenderer.enabled)
                        {
                            RemoveComponent(meshRenderer);

                            if (childOfChild.TryGetComponent(out MeshFilter meshFilter))
                            {
                                RemoveComponent(meshFilter);
                            }
                        }
                    }
                }
            }


            if (child.TryGetComponent(out MeshRenderer renderer))
            {
                if (!renderer.enabled)
                {
                    RemoveComponent(renderer);

                    if (child.TryGetComponent(out MeshFilter mesh))
                    {
                        RemoveComponent(mesh);
                    }
                }
            }
        }
    }

    [ContextMenu("RemoveNonColliderObjects")]
    public void RemoveNonColliderObjects()
    {
        foreach (Transform child in _parent)
        {
            if (child.childCount > 0)
            {
                foreach (Transform childOfChild in child)
                {
                    if (childOfChild.TryGetComponent(out Collider collider))
                    {
                        continue;
                    }
                    else
                    {
                        DestroyImmediate(childOfChild.gameObject);
                    }
                }
            }

            if (child.TryGetComponent(out Collider col))
            {
                continue;
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void RemoveComponent(Component component)
    {
        if (component)
            DestroyImmediate(component);
    }
}
