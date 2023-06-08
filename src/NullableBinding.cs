using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.NullableBinding
{
    public sealed class NullableBinding : Binding
    {
        private void Initialize()
        {
            DataSourceNullValue = null;
            NullValue = null;
        }
        public NullableBinding(string propertyName, object dataSource, string dataMember) : base(propertyName, dataSource, dataMember)
        {
            Initialize();
        }

        public NullableBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled) : base(propertyName, dataSource, dataMember, formattingEnabled)
        {
            Initialize();
        }

        public NullableBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode) : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode)
        {
            Initialize();
        }

        protected override void OnParse(ConvertEventArgs args)
        {
            if (IsNullable(args.DesiredType) && (args.Value is null or ""))
            {
                args.Value = null;
            }

            base.OnParse(args);
        }

        private static bool IsNullable<T>(T obj)
        {
            return default(T) == null || obj is null;
        }
    }
}
