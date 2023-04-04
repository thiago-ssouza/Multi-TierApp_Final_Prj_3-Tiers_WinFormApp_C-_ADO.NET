using Data;
using prjWinCs3TierAppFinalPrj;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProgramsCoursesStudentsEnrollments
{
    public partial class Form1 : Form
    {
        internal static Form1 current;
        internal static Boolean init;

        internal DataGridViewSelectedRowCollection c;
        public Form1()
        {
            current = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init = false;
            new Form2();
            Form2.current.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSourceStudents.DataSource = Data.Students.GetStudents();
            bindingSourceStudents.Sort = "StId";
            dataGridView1.DataSource = bindingSourceStudents;

            dataGridView1.Columns["StId"].HeaderText = "Student ID";
            dataGridView1.Columns["StName"].HeaderText = "Student Name";
            dataGridView1.Columns["ProgId"].HeaderText = "Program ID";
            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["StName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;
        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!init)
            {
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                //dataGridView1.DataSource = Data.Enrollments.GetEnrollments();

                //dataGridView1.Sort(dataGridView1.Columns["StId"], ListSortDirection.Ascending);

                //dataGridView1.Columns["StId"].HeaderText = "Student ID";
                //dataGridView1.Columns["CId"].HeaderText = "Course ID";
                //dataGridView1.Columns["FinalGrade"].HeaderText = "Final Grade";
                //dataGridView1.Columns["StId"].DisplayIndex = 0;
                //dataGridView1.Columns["CId"].DisplayIndex = 1;
                //dataGridView1.Columns["FinalGrade"].DisplayIndex = 2;

                dataGridView1.DataSource = Data.DispalyEnrollments.GetDisplayEnrollments();
                dataGridView1.Sort(dataGridView1.Columns["StId"], ListSortDirection.Ascending);

                dataGridView1.Columns["StId"].HeaderText = "Student ID";
                dataGridView1.Columns["StName"].HeaderText = "Student Name";
                dataGridView1.Columns["CId"].HeaderText = "Course ID";
                dataGridView1.Columns["CName"].HeaderText = "Course Name";
                dataGridView1.Columns["FinalGrade"].HeaderText = "Final Grade";
                dataGridView1.Columns["ProgId"].HeaderText = "Program ID";
                dataGridView1.Columns["ProgName"].HeaderText = "Program Name";

                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["CId"].DisplayIndex = 2;
                dataGridView1.Columns["CName"].DisplayIndex = 3;
                dataGridView1.Columns["FinalGrade"].DisplayIndex = 4;
                dataGridView1.Columns["ProgId"].DisplayIndex = 5;
                dataGridView1.Columns["ProgName"].DisplayIndex = 6;
                //StId, StName, CId, CName, FinalGrade, ProgId, ProgName
                init = true;
            }


        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSourceCourses.DataSource = Data.Courses.GetCourses();
            bindingSourceCourses.Sort = "CId";
            dataGridView1.DataSource = bindingSourceCourses;

            dataGridView1.Columns["CId"].HeaderText = "Course ID";
            dataGridView1.Columns["CName"].HeaderText = "Course Name";
            dataGridView1.Columns["ProgId"].HeaderText = "Program ID";
            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;
            init = false;

        }

        private void programToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSourcePrograms.DataSource = Data.Programs.GetPrograms();
            bindingSourcePrograms.Sort = "ProgId";
            dataGridView1.DataSource = bindingSourcePrograms;

            dataGridView1.Columns["ProgId"].HeaderText = "Program ID";
            dataGridView1.Columns["ProgName"].HeaderText = "Program Name";
            dataGridView1.Columns["ProgId"].DisplayIndex = 0;
            dataGridView1.Columns["ProgName"].DisplayIndex = 1;
            init = false;
        }

        private void bindingSourceStudents_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
            init = false;
        }

        private void bindingSourceCourses_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Courses.UpdateCourses();
            init = false;
        }

        private void bindingSourcePrograms_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
            init = false;
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
            BusinessLayer.Enrollments.UpdateEnrollments();
            BusinessLayer.Courses.UpdateCourses();
            BusinessLayer.Programs.UpdatePrograms();
            init = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
        }

        internal static void msgInvalidFinalGrade()
        {
            MessageBox.Show("Order rejected: Final Grade cannot be less than 0 or greater than 100");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.INSERT, null);
            init = false;
            BusinessLayer.Enrollments.UpdateEnrollments();
            dataGridView1.DataSource = Data.DispalyEnrollments.GetDisplayEnrollments();

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form2.current.Start(Form2.Modes.UPDATE, c);
                init = false;
                BusinessLayer.Enrollments.UpdateEnrollments();
                dataGridView1.DataSource = Data.DispalyEnrollments.GetDisplayEnrollments();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }
            else // (c.Count > 1)
            {
                List<Enrollments> lEnrollments = new List<Enrollments>();
                for (int i = 0; i < c.Count; i++)
                {
                    Enrollments enroll = new Enrollments();
                    

                    enroll.StId = "" + c[i].Cells["StId"].Value;
                    enroll.StName = "" + c[i].Cells["StName"].Value;
                    enroll.CId = "" + c[i].Cells["CId"].Value;
                    enroll.CName = "" + c[i].Cells["CName"].Value;
                    if ( !(("" + c[i].Cells["FinalGrade"].Value).Equals("")) )
                    {
                        enroll.FinalGrade = Convert.ToInt32("" + c[i].Cells["FinalGrade"].Value);
                    }
                    else
                    {
                        enroll.FinalGrade = null;
                    }
                    //enroll.FinalGrade = !( (""+c[i].Cells["FinalGrade"].Value).Equals("")) ? Convert.ToInt32(""+c[i].Cells["FinalGrade"].Value) : Convert.ToInt32(null);

                    lEnrollments.Add(enroll);
                }
                Data.Enrollments.DeleteData(lEnrollments);
                BusinessLayer.Enrollments.UpdateEnrollments();
                dataGridView1.DataSource = Data.DispalyEnrollments.GetDisplayEnrollments();
                init = false;
            }
        }

        //private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
        //    if (c.Count == 0)
        //    {
        //        MessageBox.Show("At least one line must be selected for deletion");
        //    }
        //    else // (c.Count > 1)
        //    {
        //        List<string> lId = new List<string>();
        //        for (int i = 0; i < c.Count; i++)
        //        {
        //            lId.Add("" + c[i].Cells["StId"].Value);
        //        }
        //        Data.Enrollments.DeleteData(lId);
        //        init = false;
        //    }
        //}

        private void manageFinalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            init = false;

        }

        internal static void UIMessage(string msg)
        {
            MessageBox.Show(msg);
        }
    }


}
