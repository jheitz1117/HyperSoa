﻿using HyperSoa.Contracts;

namespace HyperSoa.Service.TaskIdProviders
{
    /// <summary>
    /// Abstract implementation of <see cref="ITaskIdProvider"/>.
    /// </summary>
    public abstract class TaskIdProviderBase : ITaskIdProvider
    {
        /// <summary>
        /// When overridden in a derived class, provides the opportunity to run custom initialization code for <see cref="ITaskIdProvider"/>
        /// implementations. This method is called immediately after the <see cref="ITaskIdProvider"/> instance is instantiated. Once the
        /// <see cref="ITaskIdProvider"/> has been initialized, it exists for the lifetime of the <see cref="IHyperNodeService"/>. 
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Creates a Task ID using the specified <see cref="IReadOnlyHyperNodeMessageInfo"/> object. This method must be overridden in a derived class.
        /// </summary>
        /// <param name="message">The <see cref="IReadOnlyHyperNodeMessageInfo"/> to use.</param>
        /// <returns></returns>
        public abstract string CreateTaskId(IReadOnlyHyperNodeMessageInfo message);
    }
}
