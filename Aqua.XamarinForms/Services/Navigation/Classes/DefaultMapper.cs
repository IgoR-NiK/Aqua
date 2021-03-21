﻿using System;

using Aqua.Core.Attributes;
using Aqua.XamarinForms.Mvvm;

namespace Aqua.XamarinForms.Services.Navigation.Classes
{
    [ManualRegistration]
    public class DefaultMapper : MapperBase
    {
        public override void Map<TViewModel, TView>()
        {
            if (typeof(TViewModel).GetConstructor(Type.EmptyTypes) == null)
                throw new InvalidOperationException($"{typeof(TViewModel)} must have a parameterless constructor");
            
            ViewModelTypeToViewTypeMap.Add(typeof(TViewModel), typeof(TView));
            ViewTypeToViewModelTypeMap.Add(typeof(TView), typeof(TViewModel));
        }

        protected override Func<Type, bool> ViewModelPredicate => 
            it => typeof(ViewModelBase).IsAssignableFrom(it) 
                  && !it.IsAbstract 
                  && it.GetConstructor(Type.EmptyTypes) != null;
    }
}