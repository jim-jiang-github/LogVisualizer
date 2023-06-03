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

namespace LogVisualizer.Scenarios.Sources
{
    internal abstract class LogSource<TBlockSchema, TColumnHeadSchema, TCellSchema> :
        ICellFinder,
        ILogSource
        where TBlockSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaBlock, new()
        where TColumnHeadSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaColumnHead, new()
        where TCellSchema : SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaCell, new()
    {

        protected readonly Stream _stream;
        protected readonly SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema> _schemaLog;
        protected readonly Encoding _encoding;
        protected readonly CellConvertorProvider _convertorProvider;

        private readonly List<BlockSource> _blockSources = new();
        private ContentSource? _logContent;
        private readonly WordsCollection _wordsCollection = new();
        private ReadonlyItemCollection _readonlyItemCollection;
        ~LogSource()
        {

        }

        protected LogSource(Stream stream, SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema> schemaLog)
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
            _logContent = CreateContentSource(this);
            _readonlyItemCollection = new(_logContent);
        }
        protected abstract BlockSource CreateBlockSource(
            TBlockSchema block);
        protected abstract ContentSource CreateContentSource(
            ICellFinder cellFinder);
        protected void HandleContentCellValue(SchemaLog<TBlockSchema, TColumnHeadSchema, TCellSchema>.SchemaColumn schemaColumn, object cell)
        {
            //if (schemaColumn.Enumeratable)
            //{
            //    var cellValue = logSourceReader.GetValue(cellSource, cellIndex);
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
        #region ILogSource
        public string[] ColumnNames => _logContent?.ColumnHeadTemplate ?? Array.Empty<string>();
        public IEnumerable<string> EnumerateWords => _wordsCollection.Words;
        public LogFilter Filter { get; }
        public int RowsCount => _logContent?.RowCount ?? 0;
        public IEnumerable<LogRow> GetRows(int start, int length)
        {
            if (_logContent == null)
            {
                yield break;
            }
            foreach (var row in _logContent.GetRows(start, length))
            {
                yield return row;
            }
        }
        public IList GetRows()
        {
            return _readonlyItemCollection;
        }
        #endregion
        #region IDisposable
        public virtual void Dispose()
        {
            _stream.Close();
        }
        #endregion
    }
}
