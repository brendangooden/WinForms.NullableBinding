using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.NullableBinding
{
    public class BindingSourceGeneric<T> : BindingSource where T : class
    {
        public BindingSourceGeneric() : base()
        {

        }

        public BindingSourceGeneric(T dataSource) : base()
        {
            this.DataSource = dataSource;
        }

        public BindingSourceGeneric(T dataSource, string dataMember) : base()
        {
            this.DataSource = dataSource;
            this.DataMember = dataMember;
        }

        public new T Current => (T)base.Current;

        public new T DataSource
        {
            get => (T)base.DataSource;
            set => base.DataSource = value;
        }
    }
}
