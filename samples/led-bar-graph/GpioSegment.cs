using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading;

public class GpioSegment : IPinSegment, IDisposable
{
    private GpioController _controller;
    private int[] _pins;
    private bool disposedValue = false;

    public GpioSegment(int[] pins)
    {
        _pins = pins;
        _controller = new GpioController();
        
        foreach (var pin in _pins)
        {
            _controller.OpenPin(pin, PinMode.Output);
        }    
    }

    public int Length => _pins.Length;

    public void Write(int pin, int value)
    {
        var pinValue = PinValue.Low;
        if (value == 1)
        {
            pinValue = PinValue.High;
        }
        _controller.Write(_pins[pin], pinValue);    
    }

    public int Read(int pin)
    {
        return _controller.Read(_pins[pin]) == PinValue.High ? 1 : 0;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _controller.Dispose();
            }

            disposedValue = true;
        }
    }
    
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
