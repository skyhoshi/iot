using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public interface IPinSegment : IDisposable, IEnumerable<int>
{

    public int Length { get; }
    public int this[int index] { get; set; }
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => Enumerable.Range(0,Length).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Enumerable.Range(0,Length).GetEnumerator();
}