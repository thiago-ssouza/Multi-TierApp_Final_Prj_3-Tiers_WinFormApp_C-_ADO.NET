using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Data
{
    internal class Connect
    {
        // =========================================================================
        // We could use the Design Pattern Singleton for this class. 
        // Howeever, it is also possible (and a little simpler) to 
        // just use static attributes and static methods.
        // =========================================================================

        private static String studProgConnectionString = GetConnectString();

        internal static String ConnectionString { get => studProgConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }
    }

    internal class DataTables
    {
        // =========================================================================
        // We could use the Design Pattern Singleton for this class. 
        // Howeever, it is also possible (and a little simpler) to 
        // just use static attributes and static methods.
        // =========================================================================

        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterEnrollments = InitAdapterEnrollments();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM programs ORDER BY ProgId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM courses ORDER BY CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM students ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM enrollments ORDER BY StId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadCourses(ds);
            loadStudents(ds);
            loadEnrollments(ds);
            return ds;
        }

        private static void loadPrograms(DataSet ds)
        {
            // =========================================================================
            //adapterPrograms.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            // =========================================================================

            adapterPrograms.Fill(ds, "Programs");

            // =========================================================================
            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Programs"].Columns["ProgId"]};
            // =========================================================================
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Courses"].Columns["CId"]};

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Courses"].Columns["ProgId"],
                }
            );
            myFK.DeleteRule = Rule.Cascade;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(myFK);

            // =========================================================================  
        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;
            ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
                    { ds.Tables["Students"].Columns["StId"]};

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Programs"].Columns["ProgId"]
                },
                new DataColumn[] {
                    ds.Tables["Students"].Columns["ProgId"],
                }
            );
            myFK.DeleteRule = Rule.None;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Students"].Constraints.Add(myFK);

            // =========================================================================  
        }

        private static void loadEnrollments(DataSet ds)
        {
            adapterEnrollments.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["FinalGrade"].AllowDBNull = true;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2]
                    { ds.Tables["Enrollments"].Columns["StId"],
                    ds.Tables["Enrollments"].Columns["CId"] };

            // =========================================================================  
            /* Foreign Key between DataTables */

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[]{
                    ds.Tables["Students"].Columns["StId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["StId"],
                }
            );
            myFK.DeleteRule = Rule.Cascade;
            myFK.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK);

            ForeignKeyConstraint myFK2 = new ForeignKeyConstraint("MyFK2",
                new DataColumn[]{
                    ds.Tables["Courses"].Columns["CId"]
                },
                new DataColumn[] {
                    ds.Tables["Enrollments"].Columns["CId"],
                }
            );
            myFK2.DeleteRule = Rule.None;
            myFK2.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFK2);

            // =========================================================================  
        }

        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }
        internal static SqlDataAdapter getAdapterEnrollments()
        {
            return adapterEnrollments;
        }


        internal static DataSet getDataSet()
        {
            return ds;
        }
        internal static void ReInitDataSet()
        {
            ds = InitDataSet();
        }
    }

    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        //private static DataSet ds = DataTables.getDataSet();
        private static DataSet ds;

        internal static DataTable GetPrograms()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Programs"];
        }

        internal static int UpdatePrograms()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds;

        internal static DataTable GetCourses()
        {
           //ds.Tables["Courses"].AcceptChanges();
            ds = DataTables.getDataSet();
            return ds.Tables["Courses"];
        }

        internal static int UpdateCourses()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Courses"].HasErrors)
            {
                ds.Tables["Courses"].AcceptChanges();
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds;

        internal static DataTable GetStudents()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Students"];
        }

        internal static int UpdateStudents()
        {
         
            ds = DataTables.getDataSet();
            if (!ds.Tables["Students"].HasErrors)
            {
                ds.Tables["Students"].AcceptChanges();
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }

    internal class Enrollments
    {
        public string StId;
        public string StName;
        public string CId;
        public string CName;
        public string oldCId;
        public string oldCName;
        public Int32? FinalGrade;

        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
        private static DataSet ds;

        internal static DataTable GetEnrollments()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Enrollments"];
        }

        internal static int UpdateEnrollments()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Enrollments"].HasErrors)
            {
                return adapter.Update(ds.Tables["Enrollments"]);
            }
            else
            {
                return -1;
            }
        }

        internal static void InsertData(Enrollments enroll)
        {
            if (BusinessLayer.Enrollments.IsValidInsert(enroll))
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                try
                {
                    line.SetField("StId", enroll.StId);
                    line.SetField("CId", enroll.CId);
                    line.SetField("FinalGrade", enroll.FinalGrade);
                    ds.Tables["Enrollments"].Rows.Add(line);

                    adapter.Update(ds.Tables["Enrollments"]);
                }
                catch (SqlException)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Database: Insertion rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
                catch (Exception)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Data Layer: Insertion rejected. Student course enrollment already exist!");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }

            }
            else
            {
                ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
            }
        }

        internal static void ManageFinalGrade(Enrollments enroll)
        {
            if (BusinessLayer.Enrollments.IsValidFinalGrade(enroll))
            {
                try
                {
                    var line = ds.Tables["Enrollments"].AsEnumerable()
                                 .Where(s => s.Field<string>("StId") == enroll.StId && s.Field<string>("CId") == enroll.CId).SingleOrDefault();

                    if (line != null)
                    {
                        line.SetField("FinalGrade", enroll.FinalGrade);

                        adapter.Update(ds.Tables["Enrollments"]);
                    }
                }
                catch (SqlException)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Database: Update rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
                catch (Exception)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Data Layer: Update rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
            }
            else
            {
                ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
            }
        }

        internal static void UpdateData(Enrollments enroll)
        {
            if (BusinessLayer.Enrollments.IsValidUpdate(enroll))
            {
                try
                {
                    var line = ds.Tables["Enrollments"].AsEnumerable()
                                 .Where(s => s.Field<string>("StId") == enroll.StId && s.Field<string>("CId") == enroll.oldCId).SingleOrDefault();

                    if (line != null)
                    {
                        line.SetField("CId", enroll.CId);
                        line.SetField("FinalGrade", enroll.FinalGrade);

                        adapter.Update(ds.Tables["Enrollments"]);
                    }
                }
                catch (SqlException)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Database: Update rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
                catch (Exception)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Data Layer: Update rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
            }
            else
            {
                ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
            }
        }

        internal static void DeleteData(List<Enrollments> lEnrollments)
        {
            if (BusinessLayer.Enrollments.IsValidDelete(lEnrollments))
            {
               // bool deleted = false;
                try
                {
                    
                    List<string> lStudentId = new List<string>();
                    int count = 0;

                    foreach (Enrollments enrollElement in lEnrollments)
                    {
                        lStudentId.Add(enrollElement.StId);

                    }


                    var lines = ds.Tables["Enrollments"].AsEnumerable()
                                     .Where(s => lStudentId.Contains(s.Field<string>("StId")));

                    foreach (var line in lines)
                    {
                        if (line.Field<string>(0).Equals(lEnrollments[count].StId) && line.Field<string>(1).Equals(lEnrollments[count].CId))
                        {
                            line.Delete();
                            count++;
                        }
                    }




                    adapter.Update(ds.Tables["Enrollments"]);

                }
                catch (SqlException)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Database: Deletion rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
                catch (Exception)
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Data Layer: Deletion rejected");
                    ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
                }
            }
            else
            {
                ProgramsCoursesStudentsEnrollments.Form1.hasError = true;
            }
        }
    }

    internal class DispalyEnrollments
    {

        private static DataSet ds = DataTables.getDataSet();
        internal static DataTable GetDisplayEnrollments()
        {
            /* 
             * next line is needed to ensure "delete row"
             * due to the cascade are actually removed.
             */
            ds.Tables["Enrollments"].AcceptChanges();
            DataTables.ReInitDataSet();
            ds = DataTables.getDataSet();



            var query = (
            from enrollment in ds.Tables["Enrollments"].AsEnumerable()
            from student in ds.Tables["Students"].AsEnumerable()
            from course in ds.Tables["Courses"].AsEnumerable()
            from program in ds.Tables["Programs"].AsEnumerable()
            where enrollment.Field<string>("StId") ==
            student.Field<string>("StId")
            where enrollment.Field<string>("CId") ==
            course.Field<string>("CId")
            where student.Field<string>("ProgId") ==
            program.Field<string>("ProgId")
            select new
            {
                StId = student.Field<String>("StId"),
                StName = student.Field<string>("StName"),
                CId = course.Field<String>("CId"),
                CName = course.Field<string>("CName"),
                FinalGrade = enrollment.Field<Int32?>("FinalGrade"),
                ProgId = program.Field<String>("ProgId"),
                ProgName = program.Field<string>("ProgName")
            });
            DataTable result = new DataTable();
            result.Columns.Add("StId");
            result.Columns.Add("StName");
            result.Columns.Add("CId");
            result.Columns.Add("CName");
            result.Columns.Add("FinalGrade");
            result.Columns.Add("ProgId");
            result.Columns.Add("ProgName");
            foreach (var x in query)
            {
                object[] allFields = { x.StId, x.StName, x.CId, x.CName, x.FinalGrade, x.ProgId, x.ProgName};
                result.Rows.Add(allFields);
            }
            return result;
        }
    }
}
