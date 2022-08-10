using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class DissolveImage : UIBehaviour, IMaterialModifier
{
    [SerializeField]
    private Shader _shader;

    [SerializeField]
    private float _time;

    public float Time
    {
        set
        {
            _time = value;
            if (_graphic == null)
            {
                Setup();
            }

            _graphic.SetMaterialDirty();
        }
        get => _time;
    }

    [SerializeField]
    private Texture2D _dissolveTexture;


    private Material _material;
    private Graphic _graphic;

    private readonly int DissolveTimePropertyID = Shader.PropertyToID("_DissolveTime");
    private readonly int DisspleveTexturePropertyID = Shader.PropertyToID("_DissolveTex");

    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Setup()
    {
        _graphic = GetComponent<Graphic>();
    }

    public Material GetModifiedMaterial(Material baseMaterial)
    {
        if (_graphic == null)
        {
            return baseMaterial;
        }

        if (_material == null)
        {
            _material = new Material(_shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _material.SetTexture(DisspleveTexturePropertyID, _dissolveTexture);
        }

        _material.SetFloat(DissolveTimePropertyID, _time);
        return _material;
    }

    protected override void OnDestroy()
    {
        DestroyMaterial();
        base.OnDestroy();
    }

    private void DestroyMaterial()
    {
        if (Application.isPlaying)
        {
            GameObject.Destroy(_material);
        }
        else
        {
            GameObject.DestroyImmediate(_material);
        }

        _material = null;
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        base.OnValidate();
        if (!IsActive() || _graphic == null) return;
        _graphic.SetMaterialDirty();
    }

    protected override void Reset()
    {
        base.Reset();
        Setup();
        DestroyMaterial();
        _graphic.SetMaterialDirty();
    }
#endif
}
