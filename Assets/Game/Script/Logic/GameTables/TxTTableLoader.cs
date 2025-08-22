using System;
using Core.Table;

namespace Games.Table
{
    public class TxTTableLoader : ITableLoader
    {
        public string ReadTable(Type type)
        {
           return TableUtils.ReadFileString(TableMapDefine.TableFileMap[type]);
        }
    }
}