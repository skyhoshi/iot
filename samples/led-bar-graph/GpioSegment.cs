using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading;

public class GpioSegment : IPinSegment
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

    public int this[int index] { 
                                get => _controller.Read(_pins[index]) == PinValue.High ? 1 : 0;
                                set { 
                                        var pinValue = PinValue.Low;
                                        if (value == 1)
                                        {
                                            pinValue = PinValue.High;
                                        }
                                        _controller.Write(_pins[index], pinValue);
                                    }   
                                }

    public int Length => _pins.Length;

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
