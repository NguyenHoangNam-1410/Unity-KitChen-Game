using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnProgessChangedEventArg> OnProgessChanged;
    public class OnProgessChangedEventArg : EventArgs
    {
        public float progressNormalize;
    }
}
