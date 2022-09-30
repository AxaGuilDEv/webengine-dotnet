﻿// Copyright (c) 2016-2022 AXA France IARD / AXA France VIE. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Modified By: YUAN Huaxing, at: 2022-5-13 18:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEngine.Test.UnitTests
{
    public class TestCaseMobileApp : AXA.WebEngine.MobileApp.TestCaseApp
    {
        public TestCaseMobileApp()
        {
            TestSteps = new AXA.WebEngine.TestStep[]
            {
                new AXA.WebEngine.TestStep{ Action = "MobileAppStep1" },
                new AXA.WebEngine.TestStep{ Action = "MobileAppStep2" },
                new AXA.WebEngine.TestStep{ Action = "MobileAppStep3" },
                new AXA.WebEngine.TestStep{ Action = "MobileAppStep4" }
            };
        }
    }
}
