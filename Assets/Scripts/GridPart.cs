using System;
using JetBrains.Annotations;
using UnityEngine;

public class GridPart : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    public Vector2Int cord;

    public bool isChecked;

    private void Awake()
    {
        if (!_renderer) _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        _renderer.sprite = GridController.Instance.Cross;
        isChecked = true;
        GridController.Instance.SelectPart(this);
    }

    public void ClearCross()
    {
        _renderer.sprite = GridController.Instance.Square;
        isChecked = false;
    }


}