// -----------------------------------------------------------------------
// <copyright file="IMessageConsumer.cs" company="Jon Rowlett">
// Copyright (C) Jon Rowlett. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ConsoleHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Consumes messages.
    /// </summary>
    public interface IMessageConsumer
    {
        /// <summary>
        /// Posts the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Post(Message message);
    }
}
