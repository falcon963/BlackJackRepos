using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TestProject.Core.CustomSynchronizationContexts
{
    public class CustomSynchronizationContext : SynchronizationContext, IDisposable
{
    private readonly AutoResetEvent _workerResetEvent;
    private readonly ConcurrentQueue<WorkItem> _workItems;
    private readonly Thread _thread;

    public CustomSynchronizationContext()
    {
        _workerResetEvent = new AutoResetEvent(false);
        _workItems = new ConcurrentQueue<WorkItem>();
        _thread = new Thread(DoWork);
        _thread.Start(this);
    }

    private void DoWork(object obj)
    {
        SetSynchronizationContext(obj as SynchronizationContext);

        while (true)
        {
            WorkItem workItem;
            while (_workItems.TryDequeue(out workItem))
                workItem.Execute();

            //Note: race condition here
            _workerResetEvent.Reset();
            _workerResetEvent.WaitOne();
        }
    }

    public override void Send(SendOrPostCallback d, object state)
    {
        if (Thread.CurrentThread == _thread)
            d(state);
        else
        {
            using (var resetEvent = new AutoResetEvent(false))
            {
                var wiExecutionInfo = new WorkItemExecutionInfo();
                _workItems.Enqueue(new SynchronousWorkItem(d, state, resetEvent, ref wiExecutionInfo));
                _workerResetEvent.Set();

                resetEvent.WaitOne();
                if (wiExecutionInfo.HasException)
                    throw wiExecutionInfo.Exception;
            }
        }
    }

    public override void Post(SendOrPostCallback d, object state)
    {
        _workItems.Enqueue(new AsynchronousWorkItem(d, state));
        _workerResetEvent.Set();
    }

    public void Dispose()
    {
        _workerResetEvent.Dispose();
        _thread.Abort();
    }

    private class WorkItemExecutionInfo
    {
        public bool HasException => Exception != null;
        public Exception Exception { get; set; }
    }

    private abstract class WorkItem
    {
        protected readonly SendOrPostCallback SendOrPostCallback;
        protected readonly object State;

        protected WorkItem(SendOrPostCallback sendOrPostCallback, object state)
        {
            SendOrPostCallback = sendOrPostCallback;
            State = state;
        }

        public abstract void Execute();
    }

    private class SynchronousWorkItem : WorkItem
    {
        private readonly AutoResetEvent _syncObject;
        private readonly WorkItemExecutionInfo _workItemExecutionInfo;

        public SynchronousWorkItem(SendOrPostCallback sendOrPostCallback, object state, AutoResetEvent resetEvent,
            ref WorkItemExecutionInfo workItemExecutionInfo) : base(sendOrPostCallback, state)
        {
            if (workItemExecutionInfo == null)
                throw new NullReferenceException(nameof(workItemExecutionInfo));

            _syncObject = resetEvent;
            _workItemExecutionInfo = workItemExecutionInfo;
        }

        public override void Execute()
        {
            try
            {
                SendOrPostCallback(State);
            }
            catch (Exception ex)
            {
                _workItemExecutionInfo.Exception = ex;
            }
            _syncObject.Set();
        }
    }

    private class AsynchronousWorkItem : WorkItem
    {
        public AsynchronousWorkItem(SendOrPostCallback sendOrPostCallback, object state)
            : base(sendOrPostCallback, state)
        {
        }

        public override void Execute()
        {
            SendOrPostCallback(State);
        }
    }
}
}
