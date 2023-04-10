using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Students"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                string studentsRegex = "^S\\d{9}$";
                string programsRegex = "^P\\d{4}$";
                DataRow r = dt.Rows[0];
                if (Regex.IsMatch(("" + r["StId"]), studentsRegex) && !(("" + r["StName"]).Equals("")) && Regex.IsMatch(("" + r["ProgId"]), programsRegex))
                {
                    return Data.Students.UpdateStudents();
                }
                else
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Students Insertion/Update rejected: Student Id must have S (uppercase) and followed by 9 digits\nProgId must have P (uppercase) and followed by 4 digits\"");
                    ds.Tables["Students"].RejectChanges();
                    return -1;
                }
            }
            else
            {
                return Data.Students.UpdateStudents();
            }

            //return Data.Students.UpdateStudents();
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
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected. Course must be in the same Program of the Student.");
                    return false;
                }
                ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected. Course must be in the same Program of the Student.");
                return false;
            }
            ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected. Course must be in the same Program of the Student.");
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
            ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Addition/Modification rejected. Course must be in the same Program of the Student.");
            return false;
        }

        internal static bool IsValidFinalGrade(Data.Enrollments enroll)
        {
            DataSet ds = Data.DataTables.getDataSet();

            DataTable dtEnrollments = ds.Tables["Enrollments"];

            if (enroll.FinalGrade.ToString().Equals("") || (enroll.FinalGrade >= 0 && enroll.FinalGrade <= 100)) {
                foreach (DataRow enrollRow in dtEnrollments.Rows)
                {
                    if ((enrollRow["StId"].Equals(enroll.StId) && enrollRow["CId"].Equals(enroll.CId)))
                    {
                        return true;
                    }
                }
            }

            ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Final Grade Modification rejected. Final Grade can be empty or int number between 0 - 100");
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
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Business Rules: Deletion rejected. The One or more Final Grade is already set.");
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

            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Courses"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified);
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                string coursesRegex = "^C\\d{6}$";
                string programsRegex = "^P\\d{4}$";
                DataRow r = dt.Rows[0];
                if (Regex.IsMatch(("" + r["CId"]), coursesRegex) && !(("" + r["CName"]).Equals("")) && Regex.IsMatch(("" + r["ProgId"]), programsRegex))
                {
                    return Data.Courses.UpdateCourses();
                }
                else
                {
                    ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Courses Insertion/Update rejected: Course Id must have C (uppercase) and followed by 6 digits\nProgId must have P (uppercase) and followed by 4 digits\"");
                    ds.Tables["Courses"].RejectChanges();
                    return -1;
                }
            }
            else
            {
                return Data.Courses.UpdateCourses();
            }

        }
    }

    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            // =========================================================================
            //  Business rules for Programs
            // =========================================================================

            DataSet ds = Data.DataTables.getDataSet();

            DataTable dt = ds.Tables["Programs"]
                              .GetChanges(DataRowState.Added | DataRowState.Modified | DataRowState.Deleted);

            if ((dt != null) && (dt.Rows.Count == 1))
            {
                DataRow r = dt.Rows[0];
                if (r.RowState == DataRowState.Added || r.RowState == DataRowState.Modified)
                {
                    string programsRegex = "^P\\d{4}$";
                    
                    if (Regex.IsMatch(("" + r["ProgId"]), programsRegex) && !(("" + r["ProgName"]).Equals("")))
                    {
                        return Data.Programs.UpdatePrograms();
                    }
                    else
                    {
                        ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Programs Insertion/Update rejected: ProgId must have P (uppercase) and followed by 4 digits\"");
                        ds.Tables["Programs"].RejectChanges();
                        return -1;
                    }
                }
                //else if (r.RowState == DataRowState.Deleted)
                else
                {
                    var lines = ds.Tables["Students"].AsEnumerable()
                                     .Where(s => s.Field<string>("ProgId") == ("" + r["ProgId"]));
                    if(lines == null)
                    {
                        return Data.Programs.UpdatePrograms();
                    }
                    else
                    {
                        ProgramsCoursesStudentsEnrollments.Form1.UIMessage("Programs Delete rejected: There are students in the Program\"");
                        ds.Tables["Programs"].RejectChanges();
                        return -1;
                    }
                }
                
            }
            else
            {
                return Data.Programs.UpdatePrograms();
            }

        }
    }

}

