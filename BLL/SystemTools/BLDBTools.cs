using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace BLL.SystemTools
{
    public class BLDBTools
    {
        #region Language
        public static void ImportDataFromExcel(string excelFilePath)
        {
            //declare variables - edit these based on your particular situation
            string sqlExcelTable = "ExcelDictionary";
            // make sure your sheet name is correct, here sheet name is sheet1, so you can change your sheet name if have different
            string excelLocalLanguageQuery = "select * from [Dictionary$]";
            try
            {
                //create our connection strings
                string excelConnectionString = @"provider=microsoft.jet.oledb.4.0;data source=" + excelFilePath +
                ";extended properties=" + "\"excel 8.0;hdr=yes;\"";


                #region
                OleDbConnection oledbconn = new OleDbConnection(excelConnectionString);

                DataTable dtExcelData = new DataTable();

                using (OleDbDataAdapter oda = new OleDbDataAdapter(excelLocalLanguageQuery, oledbconn))
                {
                    oda.Fill(dtExcelData);
                }

                oledbconn.Close();

                var excelActiveLanguage = new List<string>();
                foreach (var item in dtExcelData.Columns)
                {
                    if ((item as DataColumn).ColumnName.Contains("-"))
                    {
                        excelActiveLanguage.Add((item as DataColumn).ColumnName);
                    }
                }

                new BLLanguage().SetActiveLanguages(excelActiveLanguage);

                var activeLanguage = new BLLanguage().GetActiveLanguages();


                #endregion



                string createExcelTableQuery = "DROP TABLE " + sqlExcelTable + " \n" +
                                                "SET ANSI_NULLS ON \n" +
                                                "SET QUOTED_IDENTIFIER ON \n" +
                                                "CREATE TABLE [dbo].[" + sqlExcelTable + "] ( \n" +
                                                "[Id][int] IDENTITY(1,1) NOT NULL, \n";

                foreach (var activeLang in activeLanguage)
                {
                    createExcelTableQuery += "[" + activeLang.CultureInfo + "]" + "[nvarchar](max) NULL, \n";
                }

                createExcelTableQuery += "CONSTRAINT[PK_" + sqlExcelTable + "] PRIMARY KEY CLUSTERED" +
                                          "(" +
                                          "[Id] ASC" +
                                          ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
                                          ") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]\n";
                createExcelTableQuery +=
                                        "DROP TABLE RefrenceWord \n" +
                                        "SET ANSI_NULLS ON \n" +
                                        "SET QUOTED_IDENTIFIER ON \n" +
                                        "CREATE TABLE [dbo].[RefrenceWord]" +
                                        "(" +

                                        "[Id][int] IDENTITY(1,1) NOT NULL," +

                                        "[Word] [nvarchar]" +
                                            "(max) NULL," +
                                        "CONSTRAINT[PK_RefrenceWord] PRIMARY KEY CLUSTERED" +
                                        "(" +
                                        "[Id] ASC" +
                                        ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
                                        ") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]\n";

                string sqlClrealQueries =
                                    createExcelTableQuery + "\n" +
                                    "delete " + sqlExcelTable + "\n" +
                                    "delete Dictionary" + "\n" +
                                    "delete RefrenceWord";

                string sqlConnectionString = ConfigurationManager.ConnectionStrings["IdentityDbContext"].ConnectionString;
                SqlConnection sqlconn = new SqlConnection(sqlConnectionString);
                SqlCommand sqlcmd = new SqlCommand(sqlClrealQueries, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();


                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = sqlExcelTable;

                        foreach (var activeLang in activeLanguage)
                        {
                            sqlBulkCopy.ColumnMappings.Add(activeLang.CultureInfo, activeLang.CultureInfo);
                        }

                        con.Open();
                        sqlBulkCopy.WriteToServer(dtExcelData);
                        con.Close();
                    }
                }

                string sqlFillDictionaryQueries = "delete ExcelDictionary where [en-US] is NULL   \n " +
                                   "insert RefrenceWord (Word) select [en-US] from " + sqlExcelTable + "\n";

                foreach (var activeLang in activeLanguage)
                {

                    sqlFillDictionaryQueries +=
                                    "insert Dictionary (CultureInfoCode, RefrenceWordId, Value) select '" + activeLang.CultureInfo + "', Id, [" + activeLang.CultureInfo + "] from " + sqlExcelTable + "\n";

                }

                sqlconn = new SqlConnection(sqlConnectionString);
                sqlcmd = new SqlCommand(sqlFillDictionaryQueries, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }
        #endregion

    }
}
