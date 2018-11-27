﻿using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestProject.Core.ViewModels
{
    public abstract class BaseViewModel 
        : MvxViewModel
    {
        protected BaseViewModel()
        {
        }

    }

    public abstract class BaseViewModel<TParameter, TResult> 
        : MvxViewModel<TParameter, TResult>
    {
        protected BaseViewModel()
        {
        }
    }

    public abstract class BaseViewModel<TParameter> 
        : MvxViewModel<TParameter>
    {
        protected BaseViewModel()
        {
        }
    }
}

