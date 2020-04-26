using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public interface IPinSegment
{
    public int Length { get; }
    public void Write(int pin, int value);
    public int Read(int pin);
}