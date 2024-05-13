﻿// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Core;

namespace Mercury.Language.Exceptions
{
    /// <summary>
    /// MathUnsupportedOperationException Description
    /// </summary>
    public class MathUnsupportedOperationException : MathArithmeticException
    {
        public MathUnsupportedOperationException() : base(LocalizedResources.Instance().UNSUPPORTED_OPERATION)
        { 
        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public MathUnsupportedOperationException(System.Exception cause) : base(cause.Message, cause)
        { 
        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        public MathUnsupportedOperationException(String message) : base(message)
        {
        }

        /// <summary>
        /// Creates an exception with a message.
        /// </summary>
        /// <param name="message">the message, may be null</param>
        /// <param name="cause">the underlying cause, may be null</param>
        public MathUnsupportedOperationException(String message, System.Exception cause) : base(message, cause)
        {
        }
    }
}
