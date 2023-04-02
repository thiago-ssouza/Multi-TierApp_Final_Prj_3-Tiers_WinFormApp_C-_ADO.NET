using prjWinCs3TierAppFinalPrj;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramsCoursesStudentsEnrollments
{
    public partial class Form1 : Form
    {
        internal static Form1 current;

        internal DataGridViewSelectedRowCollection c;
        public Form1()
        {
            current = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.DataSource = Data.Enrollments.GetEnrollments();
            dataGridView1.Sort(dataGridView1.Columns["StId"], ListSortDirection.Ascending);

            dataGridView1.Columns["StId"].HeaderText = "Student ID";
            dataGridView1.Columns["CId"].HeaderText = "Course ID";
            dataGridView1.Columns["FinalGrade"].HeaderText = "Final Grade";
            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["CId"].DisplayIndex = 1;
            dataGridView1.Columns["FinalGrade"].DisplayIndex = 2;

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
        }

        private void bindingSourceStudents_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
        }

        private void bindingSourceCourses_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Courses.UpdateCourses();
        }

        private void bindingSourcePrograms_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
            BusinessLayer.Enrollments.UpdateEnrollments();
            BusinessLayer.Courses.UpdateCourses();
            BusinessLayer.Programs.UpdatePrograms();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
        }

        internal static void msgInvalidFinalGrade()
        {
            MessageBox.Show("Order rejected: Final Grade cannot be less than 0 or greater than 100");
        }
    }


}
