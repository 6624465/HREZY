using HR.Web.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class YearlyReportsController : BaseController
    {
        public static int PageNo = 1;
        public static int pageCount = 0;
        public static decimal? TotalSalary = 0.0M;
        public static decimal? TotalEmployeeContribution = 0.0M;
        public static decimal? TotalEmployerContribution = 0.0M;
        public static decimal? TotalWHT = 0.0M;
        public static decimal? TotalTaxableIncome = 0.0M;
        // GET: YearlyReports
        public ActionResult YearlyReportsTDS()
        {
            return View();
        }
        public ActionResult YearlyReportsSSC()
        {
            return View();
        }
        public ActionResult YearlyReportsOne()
        {
            return View();
        }
        public ActionResult YearlyReportsTwo()
        {
            return View();
        }
        #region PrintSSFReport
        public FileResult PrintSSFReport(int year, int month)
        {
            PageNo = 1;
            try
            {
                var outputPdfStream = new MemoryStream();
                using (Document document = new Document())
                {
                    using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))
                    {
                        document.Open();
                        AddDataSheets(copy, BRANCHID, year, month);
                    }
                }

                byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                outputPdfStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + BRANCHID + ".pdf");
                Response.Buffer = true;
                Response.BinaryWrite(bytesInStream);
                Response.End();

                return File(bytesInStream, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void AddDataSheets(PdfCopy copy, int branchID, int year, int month)
        {
            using (var dbCntx = new HrDataContext())
            {
                usp_SSFSummaryHeaderByMonthTH_Result sSFHeader = dbCntx.usp_SSFSummaryHeaderByMonthTH(BRANCHID, month, year).FirstOrDefault();
                List<usp_SSFSummaryDetailByMonthTH_Result> sSFDetail = dbCntx.usp_SSFSummaryDetailByMonthTH(BRANCHID, month, year).ToList();

                pageCount = (sSFDetail.Count() / 10) + 2;

                for (int i = 0; i < sSFDetail.Count(); i++)
                {
                    TotalSalary += sSFDetail[i].TotalSalary;
                    TotalEmployeeContribution += sSFDetail[i].Amount;
                    TotalEmployerContribution += sSFDetail[i].Amount;
                }

                var path = "";
                int sSFDetailCount = sSFDetail.Count();
                int value = 0;
                for (int i = 0; i < sSFDetail.Count();)
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplates/แบบประกันสังคม.pdf");
                    PdfReader reader = new PdfReader(path);
                    value = (sSFDetailCount - i) % 10;

                    if (value >= 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (PdfStamper stamper = new PdfStamper(reader, ms))
                            {
                                Fill(stamper.AcroFields, sSFDetail, i, 10, sSFHeader);
                                stamper.FormFlattening = true;
                            }
                            reader = new PdfReader(ms.ToArray());
                            if (PageNo == 1)
                                copy.AddPage(copy.GetImportedPage(reader, 1));
                            copy.AddPage(copy.GetImportedPage(reader, 2));
                        }
                        i = i + 10;
                        PageNo += PageNo == 1 ? 2 : 1;
                    }
                }
            }
        }

        public static void Fill(AcroFields pdfFormFields, List<usp_SSFSummaryDetailByMonthTH_Result> sSFDetail, int dcount, int validcount, usp_SSFSummaryHeaderByMonthTH_Result sSFHeader)
        {
            try
            {
                pdfFormFields.SetField("CompanyName", sSFHeader.CompanyName);
                pdfFormFields.SetField("BranchName", sSFHeader.BranchName);

                pdfFormFields.SetField("CompanyAddress", sSFHeader.Address1);
                pdfFormFields.SetField("CompanyAddress1", sSFHeader.Address2 + (sSFHeader.Address3 != null ? ", " + sSFHeader.Address3 : "") + (sSFHeader.Address4 != null ? ", " + sSFHeader.Address4 : "") + (sSFHeader.CityName != null ? ", " + sSFHeader.CityName : "") + (sSFHeader.StateName != null ? ", " + sSFHeader.StateName : "") + (sSFHeader.CountryCode != null ? ", " + sSFHeader.CountryCode : ""));
                pdfFormFields.SetField("PostalCode", sSFHeader.ZipCode);
                pdfFormFields.SetField("PhoneNo", sSFHeader.TelNo);
                pdfFormFields.SetField("FaxNo", sSFHeader.FaxNo);

                pdfFormFields.SetField("TotalSalary", TotalSalary.ToString());
                pdfFormFields.SetField("TotalEmployeeContribution", TotalEmployeeContribution.ToString());
                pdfFormFields.SetField("TotalEmployerContribution", TotalEmployerContribution.ToString());
                pdfFormFields.SetField("Total", (TotalSalary + TotalEmployeeContribution + TotalEmployerContribution).ToString());
                pdfFormFields.SetField("TotalInWards", "Need to work on this....");

                pdfFormFields.SetField("CountOfEmployee", sSFDetail.Count.ToString());
                pdfFormFields.SetField("TotalPages", pageCount.ToString());
                pdfFormFields.SetField("TotalPages1", pageCount.ToString());
                pdfFormFields.SetField("PageNo", PageNo.ToString());

                pdfFormFields.SetField("Month", MonthName(sSFDetail[0].Month));
                pdfFormFields.SetField("Month1", MonthName(sSFDetail[0].Month));
                pdfFormFields.SetField("Year", sSFDetail[0].Year.ToString());
                pdfFormFields.SetField("Year1", sSFDetail[0].Year.ToString());

                if (sSFHeader.SSFNumber != null)
                {
                    char[] SSFNumberArray = sSFHeader.SSFNumber.ToCharArray();
                    for (int i = 0; i <= SSFNumberArray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("SSF" + i, SSFNumberArray[i].ToString());
                        pdfFormFields.SetField("SSF1" + i, SSFNumberArray[i].ToString());
                    }
                }
                if (sSFHeader.BranchCode != null)
                {
                    char[] BranchCodeArray = sSFHeader.BranchCode.ToCharArray();
                    for (int i = 0; i <= BranchCodeArray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("BC" + i, BranchCodeArray[i].ToString());
                        pdfFormFields.SetField("BC1" + i, BranchCodeArray[i].ToString());
                    }
                }
                pdfFormFields.SetField("SocialWelfarePercent", "100%");


                int count = dcount;
                for (int i = 0; i < validcount; i++)
                {
                    if (count < sSFDetail.Count)
                    {
                        string title = validateTitle(sSFDetail[i].SalutationType);
                        pdfFormFields.SetField("SN" + i, (count + 1).ToString());
                        pdfFormFields.SetField("EName" + i, title + sSFDetail[i].FirstName + (sSFDetail[i].MiddleName != null ? " " + sSFDetail[i].MiddleName : "") + (sSFDetail[i].LastName != null ? " " + sSFDetail[i].LastName : ""));

                        if (sSFDetail[i].IDNumber != null)
                        {
                            char[] IDNumberArray = sSFDetail[i].IDNumber.ToCharArray();
                            for (int id = 0; id <= IDNumberArray.Length - 1; id++)
                            {
                                pdfFormFields.SetField("EID" + i + id, IDNumberArray[id].ToString());
                            }
                        }

                        pdfFormFields.SetField("ESalary" + i, sSFDetail[i].TotalSalary.ToString());
                        pdfFormFields.SetField("ECont" + i, sSFDetail[i].Amount.ToString());
                    }
                    else if (count == sSFDetail.Count())
                    {
                        pdfFormFields.SetField("ESalaryTotal", TotalSalary.ToString());
                        pdfFormFields.SetField("EContTotal", TotalEmployeeContribution.ToString());
                        break;
                    }
                    count++;
                }
                //PageNo = PageNo + 1;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        private static string MonthName(byte? month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";

                default:
                    return "";
                    break;
            }
        }

        private static string validateTitle(int? salutationType)
        {
            if (salutationType != null)
            {
                if (salutationType == 2605)
                    return "Mr. ";
                else if (salutationType == 2606)
                    return "Ms. ";
                else if (salutationType == 2607)
                    return "Mrs. ";
            }
            return "";
        }

        #region PrintPND1Report
        public FileResult PrintPND1Report(int year, int month)
        {
            PageNo = 2;
            try
            {
                var outputPdfStream = new MemoryStream();
                using (Document document = new Document())
                {
                    using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))
                    {
                        document.Open();
                        AddPND1DataSheets(copy, BRANCHID, year, month);
                    }
                }

                byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                outputPdfStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "PND1_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
                Response.Buffer = true;
                Response.BinaryWrite(bytesInStream);
                Response.End();

                return File(bytesInStream, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public void AddPND1DataSheets(PdfCopy copy, int branchID, int year, int month)
        {
            using (var dbCntx = new HrDataContext())
            {
                usp_PND1SummaryHeaderByMonthTH_Result PND1Header = dbCntx.usp_PND1SummaryHeaderByMonthTH(BRANCHID).FirstOrDefault();
                List<usp_PND1SummaryDetailByMonthTH_Result> PND1Detail = dbCntx.usp_PND1SummaryDetailByMonthTH(BRANCHID, month, year).ToList();

                pageCount = (PND1Detail.Count() / 8) + 3;

                for (int i = 0; i < PND1Detail.Count(); i++)
                {
                    TotalSalary += PND1Detail[i].TotalSalary;
                    TotalWHT += PND1Detail[i].Amount;
                }

                var path = "";
                int PND1detailcount = PND1Detail.Count();
                int value = 0;
                for (int i = 0; i < PND1Detail.Count();)
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplates/PND1.pdf");
                    PdfReader reader = new PdfReader(path);
                    value = (PND1detailcount - i) % 8;

                    if (value >= 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (PdfStamper stamper = new PdfStamper(reader, ms))
                            {
                                FillPND1(stamper.AcroFields, PND1Detail, i, 8, PND1Header);
                                stamper.FormFlattening = true;
                            }
                            reader = new PdfReader(ms.ToArray());
                            if (PageNo == 1)
                                copy.AddPage(copy.GetImportedPage(reader, 1));
                            copy.AddPage(copy.GetImportedPage(reader, 2));
                            copy.AddPage(copy.GetImportedPage(reader, 3));
                        }
                        i = i + 8;
                        PageNo += 3;
                    }
                }
            }
        }
        public static void FillPND1(AcroFields pdfFormFields, List<usp_PND1SummaryDetailByMonthTH_Result> PND1Detail, int dcount, int validcount, usp_PND1SummaryHeaderByMonthTH_Result PND1Header)
        {
            try
            {
                if (PND1Header.TaxIdNumber != null)
                {
                    char[] taxidnumberarray = PND1Header.TaxIdNumber.ToCharArray();
                    for (int i = 0; i <= taxidnumberarray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("TID0" + i, taxidnumberarray[i].ToString());
                        pdfFormFields.SetField("TID" + i, taxidnumberarray[i].ToString());
                    }
                }
                if (PND1Header.BranchCode != null)
                {
                    char[] BranchCodeArray = PND1Header.BranchCode.ToCharArray();
                    for (int i = 0; i <= BranchCodeArray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("BC0" + i, BranchCodeArray[i].ToString());
                        pdfFormFields.SetField("BC" + i, BranchCodeArray[i].ToString());

                    }
                }
                pdfFormFields.SetField("CompanyName", PND1Header.CompanyName);

                pdfFormFields.SetField("Year", PND1Detail[0].Year.ToString());
                pdfFormFields.SetField("m" + PND1Detail[0].Month, "Yes", true);
                pdfFormFields.SetField("RadioButton0", "Yes", true);
                pdfFormFields.SetField("NoOfAttachmentPages", pageCount.ToString());
                pdfFormFields.SetField("NoOfEmp", PND1Detail.Count().ToString());
                pdfFormFields.SetField("NoOfEmp1", PND1Detail.Count().ToString());
                pdfFormFields.SetField("TotalIncome", TotalSalary.ToString());
                pdfFormFields.SetField("TotalIncome1", TotalSalary.ToString());
                pdfFormFields.SetField("TotalWHT", TotalWHT.ToString());
                pdfFormFields.SetField("TotalWHT1", TotalWHT.ToString());
                pdfFormFields.SetField("PageNo", PageNo.ToString());
                pdfFormFields.SetField("TotalNoOfPages", pageCount.ToString());

                int count = dcount;
                for (int i = 0; i < validcount; i++)
                {
                    if (count < PND1Detail.Count)
                    {
                        string title = validateTitle(PND1Detail[i].SalutationType);
                        pdfFormFields.SetField("SN" + i, (count + 1).ToString());

                        char[] Employeeidarray = PND1Detail[i].EmployeeId.ToString().ToCharArray();
                        for (int j = 0; j <= Employeeidarray.Length - 1; j++)
                        {
                            pdfFormFields.SetField("EID" + i + j, Employeeidarray[j].ToString());
                        }

                        pdfFormFields.SetField("EName" + i, title + PND1Detail[i].FirstName.ToString() + (PND1Detail[i].MiddleName != null ? " " + PND1Detail[i].MiddleName : ""));
                        pdfFormFields.SetField("ESurname" + i, PND1Detail[i].LastName.ToString());
                        pdfFormFields.SetField("PaymentDate" + i, PND1Detail[i].ProcessDate.Value.ToShortDateString().ToString());
                        pdfFormFields.SetField("MonthlyIncome" + i, PND1Detail[i].TotalSalary.ToString());
                        pdfFormFields.SetField("WHTPerMonth" + i, PND1Detail[i].Amount.ToString());

                        pdfFormFields.SetField("t" + i, "1");
                    }
                    else if (count == PND1Detail.Count())
                    {
                        pdfFormFields.SetField("TotalMonthlyIncome", TotalSalary.ToString());
                        pdfFormFields.SetField("TotalMonthlyWHT", TotalWHT.ToString());
                        break;
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region PrintPND1KReport
        public FileResult PrintPND1KReport(int year)
        {
            PageNo = 2;
            try
            {
                var outputPdfStream = new MemoryStream();
                using (Document document = new Document())
                {
                    using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))
                    {
                        document.Open();
                        AddPND1KDataSheets(copy, BRANCHID, year);
                    }
                }

                byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                outputPdfStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "PND1-K_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
                Response.Buffer = true;
                Response.BinaryWrite(bytesInStream);
                Response.End();

                return File(bytesInStream, "application/pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public void AddPND1KDataSheets(PdfCopy copy, int branchID, int year)
        {
            using (var dbCntx = new HrDataContext())
            {
                usp_PND1KSummaryHeaderByYearTH_Result PND1KHeader = dbCntx.usp_PND1KSummaryHeaderByYearTH(BRANCHID, year).FirstOrDefault();
                List<usp_PND1KSummaryDetailByYearTH_Result> PND1KDetail = dbCntx.usp_PND1KSummaryDetailByYearTH(BRANCHID, year).ToList();

                pageCount = (PND1KDetail.Count() / 7) + 2;

                for (int i = 0; i < PND1KDetail.Count(); i++)
                {
                    TotalWHT += PND1KDetail[i].YearlyWithHoldingTax;
                    TotalTaxableIncome += PND1KDetail[i].YearlyTaxableIncome;
                }

                var path = "";
                int PND1detailcount = PND1KDetail.Count();
                int value = 0;
                for (int i = 0; i < PND1KDetail.Count();)
                {
                    path = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplates/PND1-K.pdf");
                    PdfReader reader = new PdfReader(path);
                    value = (PND1detailcount - i) % 7;

                    if (value >= 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            using (PdfStamper stamper = new PdfStamper(reader, ms))
                            {
                                FillPND1K(stamper.AcroFields, PND1KDetail, i, 7, PND1KHeader);
                                stamper.FormFlattening = true;
                            }
                            reader = new PdfReader(ms.ToArray());
                            if (PageNo == 1)
                                copy.AddPage(copy.GetImportedPage(reader, 1));
                            copy.AddPage(copy.GetImportedPage(reader, 2));
                        }
                        i = i + 7;
                        PageNo += 2;
                    }
                }
            }
        }
        public static void FillPND1K(AcroFields pdfFormFields, List<usp_PND1KSummaryDetailByYearTH_Result> PND1KDetail, int dcount, int validcount, usp_PND1KSummaryHeaderByYearTH_Result PND1KHeader)
        {
            try
            {
                if (PND1KHeader.TaxIdNumber != null)
                {
                    char[] taxidnumberarray = PND1KHeader.TaxIdNumber.ToCharArray();
                    for (int i = 0; i <= taxidnumberarray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("tid0" + i, taxidnumberarray[i].ToString());
                        pdfFormFields.SetField("tid1" + i, taxidnumberarray[i].ToString());
                    }
                }
                if (PND1KHeader.BranchCode != null)
                {
                    char[] BranchCodeArray = PND1KHeader.BranchCode.ToCharArray();
                    for (int i = 0; i <= BranchCodeArray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("bc" + i, BranchCodeArray[i].ToString());
                        pdfFormFields.SetField("bc1" + i, BranchCodeArray[i].ToString());

                    }
                }
                pdfFormFields.SetField("CompanyName", PND1KHeader.CompanyName);

                pdfFormFields.SetField("YearThai", "Missing");
                pdfFormFields.SetField("RadioButton0", "Yes", true);
                pdfFormFields.SetField("RadioButton1", "Yes", true);
                pdfFormFields.SetField("NoOfAttachmentPages", pageCount.ToString());
                pdfFormFields.SetField("NoOfEmp", PND1KDetail.Count().ToString());
                pdfFormFields.SetField("NoOfEmp1", PND1KDetail.Count().ToString());

                pdfFormFields.SetField("TotalIncome", TotalTaxableIncome.ToString());
                pdfFormFields.SetField("TotalIncome1", TotalTaxableIncome.ToString());
                pdfFormFields.SetField("TotalWHT", TotalWHT.ToString());
                pdfFormFields.SetField("TotalWHT1", TotalWHT.ToString());

                pdfFormFields.SetField("PageNo", PageNo.ToString());
                pdfFormFields.SetField("NoOfPages", pageCount.ToString());


                int count = dcount;
                for (int i = 0; i < validcount; i++)
                {
                    if (count < PND1KDetail.Count)
                    {
                        string title = validateTitle(PND1KDetail[i].SalutationType);
                        pdfFormFields.SetField("sn" + i, (count + 1).ToString());

                        char[] Employeeidarray = PND1KDetail[i].IDNumber.ToString().ToCharArray();
                        for (int j = 0; j <= Employeeidarray.Length - 1; j++)
                        {
                            pdfFormFields.SetField("EID" + i + j, Employeeidarray[j].ToString());
                        }

                        pdfFormFields.SetField("EName" + i, title + PND1KDetail[i].FirstName.ToString() + (PND1KDetail[i].MiddleName != null ? " " + PND1KDetail[i].MiddleName : ""));
                        pdfFormFields.SetField("ESurname" + i, PND1KDetail[i].LastName.ToString());
                        pdfFormFields.SetField("EAddress" + i, "Missing");
                        pdfFormFields.SetField("MonthlyIncome" + i, PND1KDetail[i].YearlyTaxableIncome.ToString());
                        pdfFormFields.SetField("MonthWHT" + i, PND1KDetail[i].YearlyWithHoldingTax.ToString());

                        pdfFormFields.SetField("t" + i, "1");
                    }

                    else if (count == PND1KDetail.Count())
                    {
                        pdfFormFields.SetField("TotalIncome", TotalTaxableIncome.ToString());
                        pdfFormFields.SetField("TotalWHT", TotalTaxableIncome.ToString());
                        break;
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

    }
}