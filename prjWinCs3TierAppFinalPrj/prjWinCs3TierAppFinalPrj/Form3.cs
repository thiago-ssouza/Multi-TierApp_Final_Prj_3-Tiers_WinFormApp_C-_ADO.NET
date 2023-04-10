using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static prjWinCs3TierAppFinalPrj.Form2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prjWinCs3TierAppFinalPrj
{
    public partial class Form3 : Form
    {
        internal static Form3 current;
        public Form3()
        {
            current = this;
            InitializeComponent();
        }

        //internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        internal void Start(DataGridViewSelectedRowCollection c)
        {
            
            //if (mode == Modes.INSERT)
            //{
            //    cboxStudentId.DisplayMember = "StId";
            //    cboxStudentId.ValueMember = "StId";
            //    cboxStudentId.DataSource = Data.Students.GetStudents();
            //    //cboxStudentId.DataSource = dtStudents;

            //    cboxStudentId.SelectedItem = null;
            //    cboxStudentId.Enabled = true;
            //    txtStudentName.Text = "";
            //    txtStudentName.ReadOnly = true;

            //    cboxCourseId.DisplayMember = "CId";
            //    cboxCourseId.ValueMember = "CId";
            //    cboxCourseId.DataSource = Data.Courses.GetCourses();
            //    cboxStudentId.Enabled = true;
            //    //cboxCourseId.DataSource = dtCourse;
            //    cboxCourseId.SelectedItem = null;

            //    txtCourseName.Text = "";
            //    txtCourseName.ReadOnly = true;
            //}

            //if (mode == Modes.UPDATE)
            //{

                cboxStudentId.DisplayMember = "StId";
                cboxStudentId.ValueMember = "StId";
                cboxStudentId.DataSource = Data.Students.GetStudents();
                //cboxStudentId.DataSource = dtStudents;
                cboxStudentId.SelectedValue = c[0].Cells["StId"].Value;
                cboxStudentId.Enabled = false;
                txtStudentName.Text = "" + c[0].Cells["StName"].Value;
                txtStudentName.ReadOnly = true;

                cboxCourseId.DisplayMember = "CId";
                cboxCourseId.ValueMember = "CId";
                cboxCourseId.DataSource = Data.Courses.GetCourses();
                cboxCourseId.Enabled = false;
                //cboxCourseId.DataSource = dtCourse;
                cboxCourseId.SelectedValue = c[0].Cells["CId"].Value;
                txtCourseName.Text = "" + c[0].Cells["CName"].Value;
                txtCourseName.ReadOnly = true;

                txtFinalGrade.Text = "" + c[0].Cells["Finalgrade"].Value;
                txtFinalGrade.ReadOnly = false;
                //oldCId = "" + c[0].Cells["CId"].Value;
                //oldCName = "" + c[0].Cells["CName"].Value;

            //}

            ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Data.Enrollments enroll = new Data.Enrollments();

            enroll.StId = cboxStudentId.Text;
            enroll.StName = txtStudentName.Text;
            enroll.CId = cboxCourseId.Text;
            enroll.CName = txtCourseName.Text;
            if (!(txtFinalGrade.Text.Equals("")))
            {
                try { enroll.FinalGrade = int.Parse(txtFinalGrade.Text); }
                catch (Exception)
                {
                    MessageBox.Show("Final Grade must be an integer or live it empty");
                    return;
                }
            }
            else
            {
                enroll.FinalGrade = null;
            }

            Data.Enrollments.ManageFinalGrade(enroll);

            Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
        }
    }
}
