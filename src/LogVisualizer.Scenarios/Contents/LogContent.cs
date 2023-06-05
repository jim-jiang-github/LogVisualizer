using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;

namespace LogVisualizer.Scenarios.Contents
{
    internal abstract class LogContent<TBlockSchema, TColumnHeadSchema, TCellSchema> :
        ICellFinder,
        ILogContent
        where TBlockSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaBlock, new()
        where TColumnHeadSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaColumnHead, new()
        where TCellSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaCell, new()
    {

        protected readonly Stream _stream;
        protected readonly SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema> _schemaLog;
        protected readonly Encoding _encoding;
        protected readonly CellConvertorProvider _convertorProvider;

        private readonly List<LogHead> _blockSources = new();
        private readonly WordsCollection _wordsCollection = new();
        ~LogContent()
        {

        }

        protected LogContent(Stream stream, SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema> schemaLog)
        {
            _stream = stream;
            _schemaLog = schemaLog;
            _encoding = Encoding.GetEncoding(schemaLog.EncodingName);
            _convertorProvider = new CellConvertorProvider(this, schemaLog.Convertors);
            Filter = new LogFilter();
        }
        protected void Init()
        {
            foreach (var block in _schemaLog.Blocks)
            {
                var blockSource = CreateBlockSource(block);
                _blockSources.Add(blockSource);
            }
            InitContentSource(this);
        }
        protected abstract LogHead CreateBlockSource(
            TBlockSchema block);
        protected abstract void InitContentSource(
            ICellFinder cellFinder);
        protected void HandleContentCellValue(SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaColumn schemaColumn, object cell)
        {
            //if (schemaColumn.Enumeratable)
            //{
            //    var cellValue = logContentReader.GetValue(cellSource, cellIndex);
            //    _wordsCollection.AppendFromString(cellValue);
            //}
        }
        #region ICellFinder
        public object? GetCellValue(string recursivePath)
        {
            var paths = recursivePath.Split(".");
            return GetBlockCellValue(paths);
        }
        private object? GetBlockCellValue(IEnumerable<string> paths)
        {
            if (_blockSources == null)
            {
                return null;
            }
            var path = paths.FirstOrDefault();
            if (path == null)
            {
                return null;
            }
            var block = _blockSources.FirstOrDefault(b => b.Name == path);

            path = paths.Skip(1).FirstOrDefault();
            if (path == null)
            {
                return null;
            }
            var index = -1;
            for (int i = 0; i < block.Count; i++)
            {
                if (block.GetCellName(i) == path)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return null;
            }
            else
            {
                return block[index];
            }
        }
        #endregion
        #region ILogContent
        public string[] ColumnNames { get; protected set; }
        public LogRow[] Rows { get; protected set; }
        public int RowsCount { get; protected set; }
        public IEnumerable<string> EnumerateWords => _wordsCollection.Words;
        public LogFilter Filter { get; }
        #endregion
        #region IDisposable
        public virtual void Dispose()
        {
            _stream.Close();
        }
        #endregion
    }
}
