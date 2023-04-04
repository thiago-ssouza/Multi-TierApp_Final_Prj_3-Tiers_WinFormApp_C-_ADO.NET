using Data;
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
                if (r["FinalGrade"].ToString().Equals("") || (Convert.ToInt32(r["FinalGrade"]) >= 0 && Convert.ToInt32(r["FinalGrade"]) <= 100) )
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

        //internal static bool IsValid(Data.Enrollments enroll)
        //{
        //    if (!(emp.Age < 18 || emp.Salary < 15000))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        Query1aPlus.Form1.UIMessage("Business Rules: Addition/Modification rejetée");
        //        return false;
        //    }
        //}

        internal static bool IsValidInsert(Data.Enrollments enroll)
        {
            DataSet ds = Data.DataTables.getDataSet();

            //DataTable dtEnrollments = ds.Tables["Enrollments"];
            DataTable dtStudents = ds.Tables["Students"];
            DataTable dtCourses = ds.Tables["Courses"];
            //DataTable dtPrograms = ds.Tables["Programs"];

            if ( ((dtStudents != null) && (dtStudents.Rows.Count > 0)) && ((dtCourses != null) && (dtCourses.Rows.Count > 0)))
            {
                DataRow studentRow = dtStudents.Rows.Find(enroll.StId);
                DataRow courseRow = dtCourses.Rows.Find(enroll.CId);
                if (studentRow != null)
                {
                    if (courseRow != null)
                    {
                        if (studentRow["ProgId"].Equals(courseRow["ProgId"]))
                        {
                            return true;
                        }
                    }
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejetée");
                    return false;
                }
                ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejetée");
                return false;
            }
            ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected");
            return false;
        }

        internal static bool IsValidUpdate(Data.Enrollments enroll)
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dtEnrollments = ds.Tables["Enrollments"];
            DataTable dtStudents = ds.Tables["Students"];
            DataTable dtCourses = ds.Tables["Courses"];
            //DataTable dtPrograms = ds.Tables["Programs"];

            if (((dtStudents != null) && (dtStudents.Rows.Count > 0)) && ((dtCourses != null) && (dtCourses.Rows.Count > 0)))
            {
                DataRow studentRow = dtStudents.Rows.Find(enroll.StId);
                DataRow courseRow = dtCourses.Rows.Find(enroll.CId);
                DataRow courseOldRow = dtCourses.Rows.Find(enroll.oldCId);
                //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter IF " + studentRow["ProgId"] + " " + courseRow["ProgId"]);
                //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter IF " + studentRow["ProgId"] + " " + courseOldRow["ProgId"]);
                if (studentRow != null)
                {
                    //if (courseRow != null)
                    if (courseOldRow != null)
                    {
                        foreach (DataRow enrollRow in dtEnrollments.Rows)
                        {

                            ///ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseOldRow["ProgId"] + " " + enrollRow["StId"] + "/" + enroll.StId + " " + enrollRow["CId"] + "/" + enroll.oldCId + " " + enrollRow["FinalGrade"] + " " + Convert.ToString(enrollRow["FinalGrade"].ToString().Equals("") ) + " " + enrollRow["FinalGrade"]);

                            //if (studentRow["ProgId"].Equals(courseRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.CId)) && enrollRow["FinalGrade"] == null )
                            //if (studentRow["ProgId"].Equals(courseOldRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.oldCId)) && enrollRow["FinalGrade"].ToString().Equals("") )
                            if (studentRow["ProgId"].Equals(courseRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.oldCId)) && enrollRow["FinalGrade"].ToString().Equals(""))
                            {
                                //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseRow["ProgId"] + " " + enrollRow["StId"].Equals(enroll.StId) + " " + enrollRow["CId"].Equals(enroll.CId) + " " + Convert.ToString(enrollRow["FinalGrade"] == null));
                                //enroll.FinalGrade = Convert.ToInt32(enrollRow["FinalGrade"]);
                                ///ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseOldRow["ProgId"] + " " + enrollRow["StId"] + "/" + enroll.StId + " " + enrollRow["CId"] + "/" + enroll.oldCId + " " + enrollRow["FinalGrade"]);
                                //return false;
                                return true;
                            }
                        }
                        
                    }
                }
            }
            ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected");
            return false;
        }


        internal static bool IsValidDelete(List<Data.Enrollments> lEnrollments)
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dtEnrollments = ds.Tables["Enrollments"];
            //DataTable dtStudents = ds.Tables["Students"];
            //DataTable dtCourses = ds.Tables["Courses"];
            //DataTable dtPrograms = ds.Tables["Programs"];

            foreach (Data.Enrollments enrollElement in lEnrollments)
            {
                if (!(enrollElement.FinalGrade.ToString().Equals(""))  )
                {
                    //ProgramsCoursesStudentsEnrollments.Form1.UIMessage(""+enrollElement.FinalGrade);
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Deletion rejected");
                    return false;
                }
            }
            return true;

            //if (((dtStudents != null) && (dtStudents.Rows.Count > 0)) && ((dtCourses != null) && (dtCourses.Rows.Count > 0)))
            //{
            //    DataRow studentRow = dtStudents.Rows.Find(enroll.StId);
            //    DataRow courseRow = dtCourses.Rows.Find(enroll.CId);
            //    DataRow courseOldRow = dtCourses.Rows.Find(enroll.oldCId);
            //    //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter IF " + studentRow["ProgId"] + " " + courseRow["ProgId"]);
            //    //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter IF " + studentRow["ProgId"] + " " + courseOldRow["ProgId"]);
            //    if (studentRow != null)
            //    {
            //        //if (courseRow != null)
            //        if (courseOldRow != null)
            //        {
            //            foreach (DataRow enrollRow in dtEnrollments.Rows)
            //            {

            //                ///ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseOldRow["ProgId"] + " " + enrollRow["StId"] + "/" + enroll.StId + " " + enrollRow["CId"] + "/" + enroll.oldCId + " " + enrollRow["FinalGrade"] + " " + Convert.ToString(enrollRow["FinalGrade"].ToString().Equals("") ) + " " + enrollRow["FinalGrade"]);

            //                //if (studentRow["ProgId"].Equals(courseRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.CId)) && enrollRow["FinalGrade"] == null )
            //                //if (studentRow["ProgId"].Equals(courseOldRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.oldCId)) && enrollRow["FinalGrade"].ToString().Equals("") )
            //                if (studentRow["ProgId"].Equals(courseRow["ProgId"]) && (enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.oldCId)) && enrollRow["FinalGrade"].ToString().Equals(""))
            //                {
            //                    //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseRow["ProgId"] + " " + enrollRow["StId"].Equals(enroll.StId) + " " + enrollRow["CId"].Equals(enroll.CId) + " " + Convert.ToString(enrollRow["FinalGrade"] == null));
            //                    //enroll.FinalGrade = Convert.ToInt32(enrollRow["FinalGrade"]);
            //                    ///ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Enter FOREACH " + studentRow["ProgId"] + " " + courseOldRow["ProgId"] + " " + enrollRow["StId"] + "/" + enroll.StId + " " + enrollRow["CId"] + "/" + enroll.oldCId + " " + enrollRow["FinalGrade"]);
            //                    //return false;
            //                    return true;
            //                }
            //            }

            //        }
            //    }
            //}
            //ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected");
            //return false;
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

