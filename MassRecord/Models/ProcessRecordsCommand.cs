using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MassRecord.Repositories;

namespace MassRecord.Models
{
    public class ProcessRecordsCommand<T> : ICustomCommand where T : class
    {
        private FileType _FileType;
        private FileAction _FileAction;
        private TransformBlock<T, T> _TransformBlock;
        private ActionBlock<T> _ActionBlock;
        public int _MaxDegreeOfParallelism { get; set; }

        public ProcessRecordsCommand(FileType fileType, FileAction fileAction)
        {
            _FileType = fileType;
            _FileAction = fileAction;
            _MaxDegreeOfParallelism = 20;
        }

        protected virtual void SetupTransformBlock()
        {
            _TransformBlock = new TransformBlock<T, T>(
                x =>
                {
                    var webSvc = new BaseRepository<T>()
                    x.Response = webSvc;
                    return x;
                },
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = _MaxDegreeOfParallelism
                });
        }
        protected virtual void SetupActionBlock()
        {
            _ActionBlock = new ActionBlock<T>(
                x =>
                {
                    //perform UI update
                },
                new ExecutionDataflowBlockOptions
                {
                    TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext()
                });
        }

        protected virtual void LinkBlocks()
        {
            _TransformBlock.LinkTo(_ActionBlock);
        }

        public void ExecuteCommand()
        {
            SetupTransformBlock();
            SetupActionBlock();
            LinkBlocks();
        }
    }
}
