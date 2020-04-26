using System;
using System.Device.Gpio;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

public class AnimateLeds
{
    private CancellationToken _cancellation;
    private IPinSegment _pinSegment;
    private int[] _pins;
    private int[] _pinsReverse;
    public int LitTimeDefault = 200;
    public int DimTimeDefault = 50;
    public int LitTime = 200;
    public int DimTime = 50;

    public AnimateLeds(IPinSegment pinSegment, CancellationToken token)
    {        
        _pinSegment = pinSegment;
        _cancellation = token;
        _pins = Enumerable.Range(0,_pinSegment.Length).ToArray();
        _pinsReverse = _pins.Reverse().ToArray();
    }

    private void CycleLeds(params int[] pins)
    {
        if (_cancellation.IsCancellationRequested)
        {
            return;
        }

        // light time
        foreach (var pin in pins)
        {
            _pinSegment.Write(pin, 1);
        }
        Thread.Sleep(LitTime);

        // dim time
        foreach (var pin in pins)
        {
            _pinSegment.Write(pin, 0);
        }
        Thread.Sleep(DimTime);
    }

    public void ResetTime()
    {
        LitTime = LitTimeDefault;
        DimTime = DimTimeDefault;
    }

    public void Sequence(IEnumerable<int> pins)
    {
        Console.WriteLine(nameof(Sequence));
        foreach (var pin in pins)
        {
            CycleLeds(pin);
        }
    }

    public void FrontToBack(bool skipLast = false)
    {
        Console.WriteLine(nameof(FrontToBack));
        var iterations = skipLast ? _pinSegment.Length : _pinSegment.Length - 2;
        Sequence(_pins.AsSpan(0,iterations).ToArray());
    }
    public void BacktoFront()
    {
        Console.WriteLine(nameof(BacktoFront));
        Sequence(_pinsReverse);
    }

    public void MidToEnd()
    {
        Console.WriteLine(nameof(MidToEnd));
        var half = _pinSegment.Length / 2;

        if (_pinSegment.Length % 2 == 1)
        {
            CycleLeds(half);
        }

        for (var i = 1; i < half+1; i ++)
        {
            var pinA= half - i;
            var pinB = half - 1 + i;

            CycleLeds(pinA,pinB);
        }
    }

    public void EndToMid()
    {
        Console.WriteLine(nameof(EndToMid));
        var half = _pinSegment.Length / 2;

        for (var i = 0; i < half ; i++)
        {
            var ledA = i;
            var ledB = _pinSegment.Length - 1 - i;

            CycleLeds(ledA, ledB);
        }

        if (_pinSegment.Length % 2 == 1)
        {
            CycleLeds(half);
        }
    }

    public void LightAll()
    {
        Console.WriteLine(nameof(LightAll));
        foreach(var pin in _pins)
        {
            if (_cancellation.IsCancellationRequested)
            {
                return;
            }
            _pinSegment.Write(pin, 1);
        }
        Thread.Sleep(LitTime);
    }

    public void DimAllAtRandom()
    {
        Console.WriteLine(nameof(DimAllAtRandom));
        var random = new Random();

        var pinList = _pins.ToList();

        while (pinList.Count > 0)
        {
            if (_cancellation.IsCancellationRequested)
            {
                return;
            }

            var pin = random.Next(_pinSegment.Length);

            if (pinList.Remove(pin))
            {
                _pinSegment.Write(pin, 0);
                Thread.Sleep(DimTime);
            }
        }

    }
}
