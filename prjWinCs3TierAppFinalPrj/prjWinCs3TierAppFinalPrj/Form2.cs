using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prjWinCs3TierAppFinalPrj
{
    public partial class Form2 : Form
    {
        internal enum Modes
        {
            INSERT,
            UPDATE
        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }
    }
}
