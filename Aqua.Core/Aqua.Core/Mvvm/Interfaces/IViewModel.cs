﻿using System.Threading.Tasks;

using Aqua.Core.Contexts;
using Aqua.Core.Ioc;

namespace Aqua.Core.Mvvm
{
    public interface IViewModel : IViewModelContext, IResolvable
    {
        Task OnAppearing();

        Task OnDisappearing();
    }
}