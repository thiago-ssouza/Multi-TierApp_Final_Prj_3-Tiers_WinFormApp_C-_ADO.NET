using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prjWinCs3TierAppFinalPrj
{
    public partial class Form2 : Form
    {

        //DataSet dsStudents;
        //DataSet dsCourse;
        //DataTable dtStudents;
        //DataTable dtCourse;


        internal enum Modes
        {
            INSERT,
            UPDATE
        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;
        private string oldCId = "";
        private string oldCName = "";

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;

            if (mode == Modes.INSERT)
            {
                cboxStudentId.DisplayMember = "StId";
                cboxStudentId.ValueMember = "StId";
                cboxStudentId.DataSource = Data.Students.GetStudents();
                //cboxStudentId.DataSource = dtStudents;

                cboxStudentId.SelectedItem = null;
                cboxStudentId.Enabled = true;
                txtStudentName.Text = "";
                txtStudentName.ReadOnly = true;

                cboxCourseId.DisplayMember = "CId";
                cboxCourseId.ValueMember = "CId";
                cboxCourseId.DataSource = Data.Courses.GetCourses();
                cboxStudentId.Enabled = true;
                //cboxCourseId.DataSource = dtCourse;
                cboxCourseId.SelectedItem = null;

                txtCourseName.Text = "";
                txtCourseName.ReadOnly = true;
            }

            if (mode == Modes.UPDATE)
            {

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
                cboxCourseId.Enabled = true;
                //cboxCourseId.DataSource = dtCourse;
                cboxCourseId.SelectedValue = c[0].Cells["CId"].Value;
                txtCourseName.Text = "" + c[0].Cells["CName"].Value;
                txtCourseName.ReadOnly = true;

                oldCId = "" + c[0].Cells["CId"].Value;
                oldCName = "" + c[0].Cells["CName"].Value;

            }

            ShowDialog();
        }

        private void cboxStudentId_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboxStudentId.SelectedItem != null)
            {
                //var a = from r in dtStudents.AsEnumerable()
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<String>("StId").Equals(cboxStudentId.SelectedValue)
                        select new
                        {
                            StId = r.Field<string>("StId"),
                            StName = r.Field<string>("StName")
                        };
                cboxStudentId.Text = a.Single().StId;
                txtStudentName.Text = "" + a.Single().StName;
            }
        }

        private void cboxCourseId_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboxCourseId.SelectedItem != null)
            {
                //var a = from r in dtCourse.AsEnumerable()
                var a = from r in Data.Courses.GetCourses().AsEnumerable()
                        where r.Field<String>("CId").Equals(cboxCourseId.SelectedValue)
                        select new
                        {
                            CId = r.Field<string>("CId"),
                            CName = r.Field<string>("CName")
                        };
                cboxCourseId.Text = a.Single().CId;
                txtCourseName.Text = "" + a.Single().CName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Data.Enrollments enroll = new Data.Enrollments();

            if (mode == Modes.INSERT)
            {
                
                if(cboxCourseId.SelectedItem != null && cboxStudentId.SelectedItem != null)
                {
                    enroll.StId = cboxStudentId.Text;
                    enroll.StName = txtStudentName.Text;
                    enroll.CId = cboxCourseId.Text;
                    enroll.CName = txtCourseName.Text;
                    enroll.FinalGrade = null;
                }
                else
                {
                    MessageBox.Show("Student Id and Course Id selected");
                    return;
                }
                
                //try { 
                //    enroll.StId = cboxStudentId.Text;
                //    enroll.CId = cboxCourseId.Text;
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("Student Id and Course Id selected");
                //    textBox1.Text = "";
                //    return;
                //}
            }
            if (mode == Modes.UPDATE)
            {
                enroll.StId = cboxStudentId.Text;
                enroll.StName = txtStudentName.Text;
                enroll.CId = cboxCourseId.Text;
                enroll.CName = txtCourseName.Text;
                enroll.oldCId = oldCId;
                enroll.oldCName = oldCName;
                enroll.FinalGrade = null;

            }

            if (mode == Modes.INSERT) { Data.Enrollments.InsertData(enroll); }
            if (mode == Modes.UPDATE) { Data.Enrollments.UpdateData(enroll); }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
            ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
        }
    }
}
