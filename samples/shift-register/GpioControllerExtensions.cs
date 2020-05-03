using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

public static class GpioControllerExtensions
{
    public static void OpenPins(this GpioController controller, PinMode mode, params int[] pins)
    {
        foreach(var pin in pins)
        {
            controller.OpenPin(pin, mode);
        }
    }

    public static void OpenPinAndWrite(this GpioController controller, int pin, PinValue value)
    {
        controller.OpenPin(pin, PinMode.Output);
        controller.Write(pin, value);
    }

    public static void WriteValueToPins(this GpioController controller, int value, params int[] pins)
    {
        for (int i = 0; i < pins.Length; i++)
        {
            controller.Write(pins[i],value);
        }
    }

    public static void WriteValuesToPin(this GpioController controller, int pin, params int[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            controller.Write(pin,values[i]);
        }
    }
}