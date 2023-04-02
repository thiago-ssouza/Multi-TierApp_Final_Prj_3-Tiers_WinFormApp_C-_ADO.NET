using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    internal class Students
    {
        internal static int UpdateStudents()
        {
            // =========================================================================
            //  Business rules for Students
            // =========================================================================

            return Data.Students.UpdateStudents();
        }
    }

    internal class Enrollments
    {
        internal static int UpdateEnrollments()
        {
            // =========================================================================
            //  Business rules for Enrolments
            // =========================================================================

            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Enrollments"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                DataRow r = dt.Rows[0];
                if (Convert.ToInt32(r["FinalGrade"]) >= 0 && Convert.ToInt32(r["FinalGrade"]) <= 100)
                {
                    return Data.Enrollments.UpdateEnrollments();
                }
                else
                {
                    ProgramsCoursesStudentsEnrollments.Form1.msgInvalidFinalGrade();
                    ds.Tables["Enrollments"].RejectChanges();
                    return -1;
                }
            }
            else
            {
                return Data.Enrollments.UpdateEnrollments();
            }
        }
    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {
            // =========================================================================
            //  Business rules for Courses
            // =========================================================================

            return Data.Courses.UpdateCourses();
        }
    }

    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            // =========================================================================
            //  Business rules for Programs
            // =========================================================================

            return Data.Programs.UpdatePrograms();
        }
    }
}
