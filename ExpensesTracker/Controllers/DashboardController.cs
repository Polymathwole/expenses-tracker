﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ExpensesTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public string Index()
        {
            return "You are in!";
        }
    }
}