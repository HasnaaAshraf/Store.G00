﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace ServicesAbstractions
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);

    }
}
