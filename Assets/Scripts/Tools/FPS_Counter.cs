using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS_Counter : MonoBehaviour 
{
    [SerializeField] private int _frameRange = 60;
    public int AverageFPS { get; private set; }
    private int[] _fpsBuffer;    
    private int _fpsBufferIndex;

    private void Update() 
    {
           
        if(_fpsBuffer == null || _frameRange != _fpsBuffer.Length)
        {
            InitializeBuffer();
        }

        UpdateBuffer();
        Calculate();

    }

  
    private void InitializeBuffer() {
        if (_frameRange <= 0) {
            _frameRange = 1;
        }

        _fpsBuffer = new int[_frameRange];
        _fpsBufferIndex = 0;

    }

    private void UpdateBuffer() 
    {
        _fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if(_fpsBufferIndex >= _frameRange)
        {
            _fpsBufferIndex = 0;
        }
    }

    private void Calculate()
    {
        int sum = 0;
        for (int i = 0; i < _frameRange;i++)
        {
            sum += _fpsBuffer[i];
        }

        AverageFPS = sum / _frameRange;
    }
}

