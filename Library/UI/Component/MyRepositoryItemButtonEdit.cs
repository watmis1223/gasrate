using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CalculationOilPrice.Library.UI.Component
{
    //

    // Custom RepositoryItemButtonEdit

    //

    public class MyRepositoryItemButtonEdit : RepositoryItemButtonEdit

    {

        public override DevExpress.XtraEditors.ViewInfo.BaseEditViewInfo CreateViewInfo()

        {

            return new MyRepositoryItemButtonEditViewInfo(this);

        }

    }

    public class MyRepositoryItemButtonEditViewInfo : ButtonEditViewInfo

    {

        public MyRepositoryItemButtonEditViewInfo(RepositoryItem item) : base(item) { }



        protected override DevExpress.XtraEditors.Drawing.EditorButtonObjectInfoArgs CreateButtonInfo(EditorButton button, int index)

        {

            return base.CreateButtonInfo(new MyEditorButton(), index);

        }

    }

    public class MyEditorButton : EditorButton

    {

        public MyEditorButton() : this(string.Empty) { }

        public MyEditorButton(string myCaption)

        {

            this.myCaption = myCaption;

            Kind = ButtonPredefines.Glyph;

        }

        string myCaption = "";

        public override string Caption

        {

            get

            {

                return myCaption;

            }

            set

            {

                myCaption = value;

            }

        }

    }
}
