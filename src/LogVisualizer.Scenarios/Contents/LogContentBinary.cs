using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.Scenarios.Schemas;
using static LogVisualizer.Scenarios.Schemas.SchemaLogBinary;

namespace LogVisualizer.Scenarios.Contents
{
    internal class LogContentBinary : LogContent<SchemaLogBinary.SchemaBlockBinary, SchemaLogBinary.SchemaColumnHeadBinary, SchemaLogBinary.SchemaCellBinary>
    {
        private BinaryReader? _binaryReader = null;

        private LogContentBinary(Stream stream, SchemaLog<SchemaLogBinary.SchemaBlockBinary, SchemaLogBinary.SchemaColumnHeadBinary, SchemaLogBinary.SchemaCellBinary> schemaLog) : base(stream, schemaLog)
        {
            _binaryReader = new BinaryReader(stream, _encoding);
        }

        protected override LogHead CreateBlockSource(SchemaLogBinary.SchemaBlockBinary block)
        {
            if (_binaryReader == null)
            {
                throw new ArgumentException("_binaryReader is null.");
            }
            var blockCells = new LogHeadCell[block.Cells.Count(c => c.Type != SchemaLogBinaryType.Skip)];
            int cellIndex = 0;
            foreach (var blockCell in block.Cells)
            {
                if (blockCell.Type == SchemaLogBinaryType.Skip)
                {
                    if (blockCell.Length is int count)
                    {
                        _binaryReader.BaseStream.Position += count;
                    }
                    continue;
                }
                var cell = CreateCellBinary(blockCell, _binaryReader, _encoding);
                var cellConvertor = _convertorProvider.GetConvertor(blockCell.ConvertorName);
                blockCells[cellIndex] = new LogHeadCell(blockCell.Name, cellConvertor?.Convert(cell) ?? "");
                cellIndex++;
            }
            return new LogHead(block.Name, blockCells);
        }

        protected override void InitContentSource(ICellFinder cellFinder)
        {
            if (_binaryReader == null)
            {
                throw new ArgumentException("_binaryReader is null.");
            }
            var columnHeadTemplate = _schemaLog.ColumnHeadTemplate;
            int rowCount = 0;
            var rowCountParser = columnHeadTemplate.RowCount;
            if (int.TryParse(rowCountParser, out int count))
            {
                rowCount = count;
            }
            else
            {
                var cellValue = cellFinder.GetCellValue(rowCountParser);
                if (cellValue != null && int.TryParse(cellValue.ToString(), out int rowCountFromPath))
                {
                    rowCount = rowCountFromPath;
                }
            }
            var cellConvertors = new CellConvertor?[columnHeadTemplate.Columns.Length];
            for (int i = 0; i < columnHeadTemplate.Columns.Length; i++)
            {
                cellConvertors[i] = _convertorProvider.GetConvertor(columnHeadTemplate.Columns[i].Cell.ConvertorName);
            }
            var rows = Enumerable.Range(0, rowCount).Select(i =>
            {
                var cells = _schemaLog.ColumnHeadTemplate.Columns.Select((c, ci) => cellConvertors[ci].Convert(CreateCellBinary(c.Cell, _binaryReader, _encoding)));
                var row = new LogRow(i, cells.ToArray());
                return row;
            }).ToArray();
            ColumnNames = columnHeadTemplate.Columns.Select(t => t.Cell.Name).ToArray();
            Rows = rows;
            RowsCount = rowCount;
        }
        private object CreateCellBinary(SchemaLogBinary.SchemaCellBinary cell, BinaryReader binaryReader, Encoding encoding)
        {
            var type = cell.Type;
            var length = cell.Length;
            switch (type)
            {
                case SchemaLogBinaryType.Boolean:
                    return binaryReader.ReadBoolean();
                case SchemaLogBinaryType.Byte:
                    return binaryReader.ReadByte();
                case SchemaLogBinaryType.Char:
                    return binaryReader.ReadChar();
                case SchemaLogBinaryType.Decimal:
                    return binaryReader.ReadDecimal();
                case SchemaLogBinaryType.Double:
                    return binaryReader.ReadDouble();
                case SchemaLogBinaryType.Float:
                    return binaryReader.ReadSingle();
                case SchemaLogBinaryType.Int:
                    return binaryReader.ReadInt32();
                case SchemaLogBinaryType.Long:
                    return binaryReader.ReadInt64();
                case SchemaLogBinaryType.Short:
                    return binaryReader.ReadInt16();
                case SchemaLogBinaryType.StringWithLength:
                    if (length is int stringLength)
                    {
                        return encoding.GetString(binaryReader.ReadBytes(stringLength));
                    }
                    throw new ArgumentException("LogCellBinaryType.StringWithLength can not read as int.");
                case SchemaLogBinaryType.StringWithIntHead:
                    var stringHeadLength = binaryReader.ReadInt32();
                    return encoding.GetString(binaryReader.ReadBytes(stringHeadLength));
                case SchemaLogBinaryType.UInt:
                    return binaryReader.ReadUInt32();
                case SchemaLogBinaryType.ULong:
                    return binaryReader.ReadUInt64();
                case SchemaLogBinaryType.UShort:
                    return binaryReader.ReadUInt16();
                default:
                    throw new ArgumentException("Not define LogCellBinaryType.");
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _binaryReader?.Close();
        }
    }
}
