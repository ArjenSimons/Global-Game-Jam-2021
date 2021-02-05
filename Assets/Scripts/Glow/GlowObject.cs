using UnityEngine;
using System.Collections.Generic;

public class GlowObject : MonoBehaviour
{
    public Color GlowColor;
    public float LerpFactor = 10;
    public bool PartOfGroup = false;

    [SerializeField]
    private SharedPlayerData playerData = null;

    bool glow = false;

    public Renderer[] Renderers
    {
        get;
        private set;
    }

    public Color CurrentColor
    {
        get { return _currentColor; }
    }

    private List<Material> _materials = new List<Material>();
    private Color _currentColor;
    private Color _targetColor;

    private void Start()
    {
        Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
        {
            _materials.AddRange(renderer.materials);
        }
    }

    private void LateUpdate()
    {
        if (!PartOfGroup)
        {
            if (playerData.controller.IsInGrabRange(transform.position))
            {
                SetGlow(glow);
            }
            else SetGlow(false);
        }   
        glow = false;
    }

    public void EnableGlow()
    {
        glow = true;
    }

    private void OnMouseEnter()
    {
        if (!PartOfGroup && playerData.controller.IsInGrabRange(transform.position))
        {
            //SetGlow(true);
        }
    }

    private void OnMouseExit()
    {
        if (!PartOfGroup)
        {
            //SetGlow(false);
        }
    }

    public void SetGlow(bool value)
    {
        if (!playerData.controller.CanGrab)
        {
            return;
        }

        if (value)
        {
            glow = true;
            _targetColor = GlowColor;
        }
        else
        {
            _targetColor = Color.black;
        }
    }

    /// <summary>
    /// Loop over all cached materials and update their color, disable self if we reach our target color.
    /// </summary>
    private void Update()
    {
        

        if (_currentColor != _targetColor)
        {
            _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

            for (int i = 0; i < _materials.Count; i++)
            {
                _materials[i].SetColor("_GlowColor", _currentColor);
            }
        }
    }
}