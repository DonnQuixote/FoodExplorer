using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProcessChangedEventArgs> OnProcessChanged;
    public class OnProcessChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
