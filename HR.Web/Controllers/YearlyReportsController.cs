using HR.Web.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.ViewModels;

using HR.Web.BusinessObjects.Payroll;



namespace HR.Web.Controllers
{
    public class YearlyReportsController : BaseController
    {

        PayslipBatchHeaderBo PayslipbatchheaderBo = null;
        PayslipBatchDetailBO payslipbatchdetailBo = null;

        public static int PageNo = 1;
        public static int pageCount = 0;

        public static decimal? TotalSalary = 0.0M;
        public static decimal? TotalEmployeeContribution = 0.0M;
        public static decimal? TotalEmployerContribution = 0.0M;
        public static decimal? TotalWHT = 0.0M;
        public static decimal? TotalTaxableIncome = 0.0M;

        public static decimal? TotalIncome = 0.0M;
        public static decimal? TotalDeductions = 0.0M;

        public static decimal? taxamount = 0.0M;


        public YearlyReportsController()
        {

            PayslipbatchheaderBo = new PayslipBatchHeaderBo(SESSIONOBJ);
            payslipbatchdetailBo = new PayslipBatchDetailBO(SESSIONOBJ);


        }

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
                Response.AddHeader("content-disposition", "attachment;filename=" + "SSF_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
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
            TotalSalary = 0.0M;
            TotalEmployeeContribution = 0.0M;
            TotalEmployerContribution = 0.0M;

            using (var dbCntx = new HrDataContext())
            {
                usp_SSFSummaryHeaderByMonthTH_Result sSFHeader = dbCntx.usp_SSFSummaryHeaderByMonthTH(BRANCHID, month, year).FirstOrDefault();
                List<usp_SSFSummaryDetailByMonthTH_Result> sSFDetail = dbCntx.usp_SSFSummaryDetailByMonthTH(BRANCHID, month, year).ToList();

                if (sSFDetail.Count == 0)
                {
                    pageCount = 2;
                }
                else
                {
                    pageCount = (sSFDetail.Count() / 10) + 2;
                }

                for (int i = 0; i < sSFDetail.Count(); i++)
                {
                    TotalSalary += sSFDetail[i].TotalSalary;
                    TotalEmployeeContribution += sSFDetail[i].Amount;
                    TotalEmployerContribution += sSFDetail[i].Amount;
                }

                var path = "";
                int sSFDetailCount = pageCount; // sSFDetail.Count();
                int value = 0;
                for (int i = 0; i < pageCount;)
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
                                Fill(stamper.AcroFields, sSFDetail, i, 10, sSFHeader, month, year);
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

        public static void Fill(AcroFields pdfFormFields, List<usp_SSFSummaryDetailByMonthTH_Result> sSFDetail, int dcount, int validcount, usp_SSFSummaryHeaderByMonthTH_Result sSFHeader, int month, int year)
        {
            try
            {
                if (sSFHeader != null)
                {

                    pdfFormFields.SetField("CompanyName", sSFHeader.BranchName);
                    pdfFormFields.SetField("CompanyName1", sSFHeader.BranchName);
                    pdfFormFields.SetField("BranchName", "");

                    pdfFormFields.SetField("CompanyAddress", (sSFHeader.Address1 != null ? sSFHeader.Address1 : "") + (sSFHeader.Address2 != null ? sSFHeader.Address2 : ""));
                    pdfFormFields.SetField("CompanyAddress1", (sSFHeader.Address3 != null ? "," + sSFHeader.Address3 : "") + (sSFHeader.Address4 != null ? "," + sSFHeader.Address4 : "") +
                        (sSFHeader.Address5 != null ? "," + sSFHeader.Address5 : "") + (sSFHeader.Address6 != null ? "," + sSFHeader.Address6 : "") + (sSFHeader.Address7 != null ? "," + sSFHeader.Address7 : "") +
                        (sSFHeader.Address8 != null ? "," + sSFHeader.Address8 : "") + (sSFHeader.Address9 != null ? "," + sSFHeader.Address9 : "") + (sSFHeader.Address10 != null ? "," + sSFHeader.Address10 : "") +
                        (sSFHeader.Address11 != null ? "," + sSFHeader.Address11 : "") + (sSFHeader.Address12 != null ? "," + sSFHeader.Address12 : "") + (sSFHeader.Address13 != null ? "," + sSFHeader.Address13 : "") +
                        (sSFHeader.CityName != null ? "," + sSFHeader.CityName : "") + (sSFHeader.StateName != null ? "," + sSFHeader.StateName : ""));
                    pdfFormFields.SetField("PostalCode", sSFHeader.ZipCode);
                    pdfFormFields.SetField("PhoneNo", sSFHeader.TelNo);
                    pdfFormFields.SetField("FaxNo", sSFHeader.FaxNo);

                    pdfFormFields.SetField("TotalSalary", TotalSalary.ToString());
                    pdfFormFields.SetField("TotalEmployeeContribution", TotalEmployeeContribution.ToString());
                    pdfFormFields.SetField("TotalEmployerContribution", TotalEmployerContribution.ToString());
                    pdfFormFields.SetField("Total", (TotalEmployeeContribution + TotalEmployerContribution).ToString());

                    string strToBaht = "";

                    strToBaht = HR.Web.Helpers.NumberToWordExtensionMethods.ToBaht(Convert.ToDouble((TotalEmployeeContribution + TotalEmployerContribution)));

                    pdfFormFields.SetField("TotalInWards", strToBaht);

                    pdfFormFields.SetField("CountOfEmployee", sSFDetail.Count.ToString());
                    pdfFormFields.SetField("TotalPages", pageCount.ToString());
                    pdfFormFields.SetField("TotalPages1", pageCount.ToString());
                    pdfFormFields.SetField("PageNo", PageNo.ToString());

                    //pdfFormFields.SetField("Month", MonthName(sSFDetail[0].Month));
                    //pdfFormFields.SetField("Month1", MonthName(sSFDetail[0].Month));
                    //pdfFormFields.SetField("Year", sSFDetail[0].Year.ToString());
                    //pdfFormFields.SetField("Year1", sSFDetail[0].Year.ToString());


                    pdfFormFields.SetField("Month", MonthName(Convert.ToByte(month)));
                    pdfFormFields.SetField("Month1", MonthName(Convert.ToByte(month)));
                    pdfFormFields.SetField("Year", year.ToString());
                    pdfFormFields.SetField("Year1", year.ToString());



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
                    pdfFormFields.SetField("SocialWelfarePercent", sSFHeader.SSFContributionRate.ToString());


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
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

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
            TotalSalary = 0.0M;
            TotalWHT = 0.0M;

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
                            if (PageNo == 2)
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
                if (PND1Header != null)
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
                    pdfFormFields.SetField("CompanyName", PND1Header.BranchName == null ? "" : PND1Header.BranchName);
                    pdfFormFields.SetField("Address1", PND1Header.Address1 == null ? "" : PND1Header.Address1);
                    pdfFormFields.SetField("Address2", PND1Header.Address2 == null ? "" : PND1Header.Address2);
                    pdfFormFields.SetField("Address3", PND1Header.Address3 == null ? "" : PND1Header.Address3);
                    pdfFormFields.SetField("Address4", PND1Header.Address4 == null ? "" : PND1Header.Address4);
                    pdfFormFields.SetField("Address5", PND1Header.Address5 == null ? "" : PND1Header.Address5);
                    pdfFormFields.SetField("Address6", PND1Header.Address6 == null ? "" : PND1Header.Address6);
                    pdfFormFields.SetField("Address7", PND1Header.Address7 == null ? "" : PND1Header.Address7);
                    pdfFormFields.SetField("Address8", PND1Header.Address8 == null ? "" : PND1Header.Address8);
                    pdfFormFields.SetField("Address9", PND1Header.Address9 == null ? "" : PND1Header.Address9);
                    pdfFormFields.SetField("Address10", PND1Header.Address10 == null ? "" : PND1Header.Address10);
                    pdfFormFields.SetField("Address11", PND1Header.Address11 == null ? "" : PND1Header.Address11);
                    pdfFormFields.SetField("Address12", PND1Header.Address12 == null ? "" : PND1Header.Address12);
                    pdfFormFields.SetField("Address13", PND1Header.Address13 == null ? "" : PND1Header.Address13);

                    if (PND1Header.ZipCode != null)
                    {
                        char[] ZipCodeArray = PND1Header.ZipCode.ToString().ToCharArray();
                        for (int i = 0; i <= ZipCodeArray.Length - 1; i++)
                        {
                            pdfFormFields.SetField("PC" + i, ZipCodeArray[i].ToString());
                        }
                    }


                    pdfFormFields.SetField("Year", PND1Detail[0].Year.ToString());
                    pdfFormFields.SetField("m" + PND1Detail[0].Month, "Yes", true);
                    pdfFormFields.SetField("RB2", "Yes", true);
                    pdfFormFields.SetField("RadioButton0", "Yes", true);
                    pdfFormFields.SetField("NoOfAttachmentPages", pageCount.ToString());
                    pdfFormFields.SetField("NoOfEmp", PND1Detail.Count().ToString());
                    pdfFormFields.SetField("NoOfEmp1", PND1Detail.Count().ToString());
                    pdfFormFields.SetField("TotalIncome", TotalSalary.ToString());
                    pdfFormFields.SetField("TotalIncome1", TotalSalary.ToString());
                    pdfFormFields.SetField("TotalWHT", TotalWHT.ToString());
                    pdfFormFields.SetField("TotalWHT1", TotalWHT.ToString());
                    pdfFormFields.SetField("GrandTotalWHT", TotalWHT.ToString());
                    pdfFormFields.SetField("PageNo", PageNo.ToString());
                    pdfFormFields.SetField("TotalNoOfPages", pageCount.ToString());

                    int count = dcount;
                    for (int i = 0; i < validcount; i++)
                    {
                        if (count < PND1Detail.Count)
                        {
                            string title = validateTitle(PND1Detail[i].SalutationType);
                            pdfFormFields.SetField("SN" + i, (count + 1).ToString());

                            char[] Employeeidarray = PND1Detail[i].IDNumber.ToString().ToCharArray();
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
            TotalWHT = 0.0M;
            TotalTaxableIncome = 0.0M;

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
                            if (PageNo == 2)
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
                if (PND1KHeader != null)
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
                    pdfFormFields.SetField("CompanyName", PND1KHeader.BranchName);

                    pdfFormFields.SetField("YearThai", DateTime.Now.Year.ToString());
                    pdfFormFields.SetField("RadioButton0", "Yes", true);
                    pdfFormFields.SetField("RadioButton1", "Yes", true);
                    pdfFormFields.SetField("RadioButton2", "Yes", true);

                    pdfFormFields.SetField("NoOfAttachmentPages", pageCount.ToString());
                    pdfFormFields.SetField("NoOfEmp", PND1KDetail.Count().ToString());
                    pdfFormFields.SetField("NoOfEmp1", PND1KDetail.Count().ToString());

                    pdfFormFields.SetField("TotalIncome", TotalTaxableIncome.ToString());
                    pdfFormFields.SetField("TotalIncome1", TotalTaxableIncome.ToString());
                    pdfFormFields.SetField("TotalWHT", TotalWHT.ToString());
                    pdfFormFields.SetField("TotalWHT1", TotalWHT.ToString());
                    pdfFormFields.SetField("GrandTotalIncome", TotalTaxableIncome.ToString());
                    pdfFormFields.SetField("GrandTotalWHT", TotalWHT.ToString());

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
                                pdfFormFields.SetField("eid" + i + j, Employeeidarray[j].ToString());
                            }

                            pdfFormFields.SetField("EName" + i, title + PND1KDetail[i].FirstName.ToString() + (PND1KDetail[i].MiddleName != null ? " " + PND1KDetail[i].MiddleName : ""));
                            pdfFormFields.SetField("ESurname" + i, PND1KDetail[i].LastName.ToString());
                            pdfFormFields.SetField("EAddress" + i, PND1KDetail[i].Address1 + "," +
                            PND1KDetail[i].CityName + "," + PND1KDetail[i].StateName + "," + PND1KDetail[i].ZipCode);
                            pdfFormFields.SetField("MonthlyIncome" + i, PND1KDetail[i].YearlyTaxableIncome.ToString());
                            pdfFormFields.SetField("MonthWHT" + i, PND1KDetail[i].YearlyWithHoldingTax.ToString());

                            pdfFormFields.SetField("t" + i, "1");
                        }

                        else if (count == PND1KDetail.Count())
                        {
                            pdfFormFields.SetField("TotalIncome", TotalTaxableIncome.ToString());
                            pdfFormFields.SetField("TotalWHT", TotalWHT.ToString());
                            break;
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region PrintTAVSummaryByEmployeeReport

        public FileResult PrintTAVSummaryByEmployeeReportForEmployee(int empId)
        {
            PageNo = 1;
            try
            {
                using (var dbcnx = new HrDataContext())
                {

                    var obj = dbcnx.PayslipBatchHeaders.
                        Join(dbcnx.PayslipBatchDetails,
                       hd => hd.BatchHeaderId, dt => dt.BatchHeaderId,
                       (hd, dt) => new { Hd = hd, Dt = dt }).
                       Where(x => x.Dt.EmployeeId == empId && x.Hd.BranchId == BRANCHID).
                       OrderByDescending(x => x.Hd.BatchHeaderId).
                       Select(x => new EmployeeMonthlyPayslipVM
                       {
                           Month = x.Hd.Month,
                           Year = x.Hd.Year
                       }).FirstOrDefault();



                    var outputPdfStream = new MemoryStream();
                    using (Document document = new Document())
                    {
                        using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))
                        {
                            document.Open();
                            AddTAVSummaryByEmployeeDataSheets(copy, BRANCHID, obj.Year.Value, empId);
                        }
                    }

                    byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                    outputPdfStream.Close();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + "แบบฟอร์ม-ทวิ-50_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
                    Response.Buffer = true;
                    Response.BinaryWrite(bytesInStream);
                    Response.End();

                    return File(bytesInStream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        public FileResult PrintTAVSummaryByEmployeeReport(int year, int empId)
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
                        AddTAVSummaryByEmployeeDataSheets(copy, BRANCHID, year, empId);
                    }
                }

                byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                outputPdfStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "แบบฟอร์ม-ทวิ-50_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
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


        public void AddTAVSummaryByEmployeeDataSheets(PdfCopy copy, int branchID, int year, int empid)
        {
            using (var dbCntx = new HrDataContext())
            {
                usp_TAVSummaryByEmployeeTH_Result TAVSummaryByEmployee = dbCntx.usp_TAVSummaryByEmployeeTH(BRANCHID, empid, year).FirstOrDefault();

                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplates/แบบฟอร์ม-ทวิ-50.pdf");
                PdfReader reader = new PdfReader(path);
                using (var ms = new MemoryStream())
                {
                    using (PdfStamper stamper = new PdfStamper(reader, ms))
                    {
                        FillTAVSummaryByEmployee(stamper.AcroFields, TAVSummaryByEmployee);
                        stamper.FormFlattening = true;
                    }
                    reader = new PdfReader(ms.ToArray());
                    copy.AddPage(copy.GetImportedPage(reader, 1));
                }
            }
        }
        public static void FillTAVSummaryByEmployee(AcroFields pdfFormFields, usp_TAVSummaryByEmployeeTH_Result TAVSummaryByEmployee)
        {
            try
            {
                if (TAVSummaryByEmployee != null)
                {
                    if (TAVSummaryByEmployee.BranchTaxID != null)
                    {
                        char[] taxidnumberarray = TAVSummaryByEmployee.BranchTaxID.ToCharArray();
                        for (int i = 0; i <= taxidnumberarray.Length - 1; i++)
                        {
                            pdfFormFields.SetField("ctid" + i, taxidnumberarray[i].ToString());
                        }
                    }

                    if (TAVSummaryByEmployee.BranchCode != null)
                    {
                        char[] BranchSSFNumberArray = TAVSummaryByEmployee.BranchSSFNumber.ToCharArray();
                        for (int i = 0; i <= BranchSSFNumberArray.Length - 1; i++)
                        {
                            pdfFormFields.SetField("ssfNo" + i, BranchSSFNumberArray[i].ToString());

                        }
                    }
                    //string title = validateTitle(TAVSummaryByEmployee.SalutationType);



                    pdfFormFields.SetField("RunNo", "");
                    pdfFormFields.SetField("CompanyName", TAVSummaryByEmployee.BranchName);
                    pdfFormFields.SetField("CompanyAddress",
                        TAVSummaryByEmployee.BranchAddress1 +
                        (TAVSummaryByEmployee.BranchAddress2 != null ? "," + TAVSummaryByEmployee.BranchAddress2 : "") +
                        TAVSummaryByEmployee.BranchCityName + "," + TAVSummaryByEmployee.BranchZipCode
                        );

                    pdfFormFields.SetField("EmployeeName", TAVSummaryByEmployee.SalutationType + "." + TAVSummaryByEmployee.FirstName + (TAVSummaryByEmployee.MiddleName != null ? " " + TAVSummaryByEmployee.MiddleName : "") + (TAVSummaryByEmployee.LastName != null ? " " + TAVSummaryByEmployee.LastName : ""));
                    char[] Employeeidarray = TAVSummaryByEmployee.IDNumber.ToString().ToCharArray();
                    for (int i = 0; i <= Employeeidarray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("eid" + i, Employeeidarray[i].ToString());
                    }
                    char[] EmployeeSSFNoarray = TAVSummaryByEmployee.SocialWelfareNo.ToString().ToCharArray();
                    for (int i = 0; i <= EmployeeSSFNoarray.Length - 1; i++)
                    {
                        pdfFormFields.SetField("essfNo" + i, EmployeeSSFNoarray[i].ToString());
                    }
                    pdfFormFields.SetField("EmployeeAddress", TAVSummaryByEmployee.EmpAdd1 +
                        (TAVSummaryByEmployee.EmpAddCityName != null ? "," + TAVSummaryByEmployee.EmpAddCityName + "," : ",") +
                        TAVSummaryByEmployee.EmpAddStateName + "," + TAVSummaryByEmployee.EmpAddZipCode);
                    pdfFormFields.SetField("PageNo", PageNo.ToString());


                    DateTime lastdayofYear = new DateTime(Convert.ToInt32(DateTime.Now.Year), 12, 31);
                    pdfFormFields.SetField("YearEndDate", lastdayofYear.ToString("dd-MM-yyyy"));
                    pdfFormFields.SetField("CTC", TAVSummaryByEmployee.TotalSalary.ToString());
                    pdfFormFields.SetField("TotalTDS", TAVSummaryByEmployee.TotalWHTax.ToString());
                    pdfFormFields.SetField("CTC1", TAVSummaryByEmployee.TotalSalary.ToString());
                    pdfFormFields.SetField("TotalTDS1", TAVSummaryByEmployee.TotalWHTax.ToString());

                    pdfFormFields.SetField("InWordsTotal", "");
                    pdfFormFields.SetField("TotalSSF", TAVSummaryByEmployee.TotalSSF.ToString());
                    pdfFormFields.SetField("TotalPF", TAVSummaryByEmployee.TotalPF.ToString());

                    pdfFormFields.SetField("chk1", "Yes", true);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        #region DownloadEmployeePaySlip

        public FileResult DownloadEmployeeLatestPaySlip(int empid)
        {
            PageNo = 1;
            try
            {

                //var payslipbatchheaderItem = PayslipbatchheaderBo.GetByProperty(x=>x.BranchId==BRANCHID).
                using (var dbcnx = new HrDataContext())
                {

                    var payslipObj = dbcnx.PayslipBatchHeaders.
                        Join(dbcnx.PayslipBatchDetails,
                       hd => hd.BatchHeaderId, dt => dt.BatchHeaderId,
                       (hd, dt) => new { Hd = hd, Dt = dt }).
                       Where(x => x.Dt.EmployeeId == empid && x.Hd.BranchId == BRANCHID).
                       OrderByDescending(x => x.Hd.BatchHeaderId).
                       Select(x => new EmployeeMonthlyPayslipVM
                       {
                           Month = x.Hd.Month,
                           Year = x.Hd.Year
                       }).FirstOrDefault();


                    var outputPdfStream = new MemoryStream();
                    using (Document document = new Document())
                    {
                        using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))

                        {
                            document.Open();
                            AddEmployeePaySlipDataSheets(copy, BRANCHID, empid, Convert.ToInt32(payslipObj.Month), payslipObj.Year.Value);
                        }
                    }

                    byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                    outputPdfStream.Close();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + "PaySlip_" + empid.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
                    Response.Buffer = true;
                    Response.BinaryWrite(bytesInStream);
                    Response.End();

                    return File(bytesInStream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }





        public FileResult DownloadEmployeePaySlip(int year, int month, int empid)
        {
            PageNo = 1;
            try
            {
                //          BaseFont bfR;
                //          bfR = BaseFont.CreateFont("C:\\Windows\\Fonts\\Verdana.ttf",
                //            BaseFont.IDENTITY_H,
                //            BaseFont.EMBEDDED);
                //          Font fntHead =
                //new Font(bfR, 12, NORMAL, clrBlack);



                var outputPdfStream = new MemoryStream();
                using (Document document = new Document())
                {
                    using (PdfSmartCopy copy = new PdfSmartCopy(document, outputPdfStream))
                    {
                        document.Open();
                        AddEmployeePaySlipDataSheets(copy, BRANCHID, empid, month, year);
                    }
                }

                byte[] bytesInStream = outputPdfStream.ToArray(); // simpler way of converting to array
                outputPdfStream.Close();

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + "PaySlip_" + empid.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".pdf");
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


        public void AddEmployeePaySlipDataSheets(PdfCopy copy, int branchID, int empid, int year, int month)
        {
            using (var dbCntx = new HrDataContext())
            {
                usp_EmployeePaySlipHeaderTH_Result PayslipHeader = dbCntx.usp_EmployeePaySlipHeaderTH(BRANCHID, empid, year, month).FirstOrDefault();
                List<usp_EmployeePaySlipDetailTH_Result> payslipDetail = dbCntx.usp_EmployeePaySlipDetailTH(BRANCHID, empid, year, month).ToList();

                TotalSalary = 0.0M;
                TotalDeductions = 0.0M;

                for (int i = 0; i < payslipDetail.Count(); i++)
                {
                    TotalSalary += payslipDetail[i].RegisterCode == "BASIC SALARY" ? payslipDetail[i].Amount : 0;
                    TotalDeductions += payslipDetail[i].RegisterCode == "EMPLOYEE CONTRIBUTION" ? payslipDetail[i].Amount : 0;
                    if(payslipDetail[i].RegisterCode == "EMPLOYEE CONTRIBUTION" && payslipDetail[i].ContributionCode== "INCOME TAX")
                    {
                        taxamount = payslipDetail[i].Amount;
                    }
                }

                var path = "";
                int PND1detailcount = payslipDetail.Count();
                path = System.Web.Hosting.HostingEnvironment.MapPath("~/PdfTemplates/PaySlipEZY.pdf");
                PdfReader reader = new PdfReader(path);



                using (var ms = new MemoryStream())
                {
                    using (PdfStamper stamper = new PdfStamper(reader, ms))
                    {
                        FillEmployeePaySlip(stamper.AcroFields, payslipDetail, PayslipHeader, month, year);
                        stamper.FormFlattening = true;
                    }
                    reader = new PdfReader(ms.ToArray());

                    copy.AddPage(copy.GetImportedPage(reader, 1));

                }
            }
        }
        public static void FillEmployeePaySlip(AcroFields pdfFormFields, List<usp_EmployeePaySlipDetailTH_Result> payslipDetail, usp_EmployeePaySlipHeaderTH_Result payslipHeader, int month, int year)
        {
            try
            {
                if (payslipHeader != null)
                {

                    pdfFormFields.SetField("CompanyName", payslipHeader.BranchName.Trim() == null ? "" : payslipHeader.BranchName.Trim());
                    pdfFormFields.SetField("Address1", (payslipHeader.Address1 == null ? "" : payslipHeader.Address1.Trim()) + "," + (payslipHeader.Address2 == null ? "" : payslipHeader.Address2.Trim()));
                    pdfFormFields.SetField("Address2", payslipHeader.CityName == null ? "" : payslipHeader.CityName.Trim());
                    pdfFormFields.SetField("Address3", payslipHeader.StateName == null ? "" : payslipHeader.StateName.Trim());
                    pdfFormFields.SetField("Address4", (payslipHeader.CountryName == null ? "" : payslipHeader.CountryName.Trim()) + "-" + (payslipHeader.ZipCode == null ? "" : payslipHeader.ZipCode.Trim()));

                    pdfFormFields.SetField("Month", MonthName(year));
                    pdfFormFields.SetField("Year", month.ToString());

                    pdfFormFields.SetField("EmpName", payslipHeader.SalutationType + "." + payslipHeader.FirstName.ToString() + (payslipHeader.MiddleName != null ? " " + payslipHeader.MiddleName : ""));
                    pdfFormFields.SetField("Title", payslipHeader.EmployeeDescription);
                    pdfFormFields.SetField("NICNo", payslipHeader.PasspostNo);

                    pdfFormFields.SetField("Department", payslipHeader.EmployeeDepartment);
                    pdfFormFields.SetField("EmpNo", payslipHeader.EmployeeId.ToString());
                    pdfFormFields.SetField("EpfNo", payslipHeader.EPFNO == null ? "" : payslipHeader.EPFNO.ToString());
                    pdfFormFields.SetField("PayeNo", "");
                    pdfFormFields.SetField("Location", payslipHeader.CountryName);
                    pdfFormFields.SetField("Total", TotalSalary.ToString());
                    pdfFormFields.SetField("Total1", TotalDeductions.ToString());
                    for (int i = 0; i < payslipDetail.Count; i++)
                    {
                        if (payslipHeader.BranchID == 10008)
                        {
                            if (payslipDetail[i].RegisterCode == "EMPLOYER CONTRIBUTION")
                            {
                           
                                pdfFormFields.SetField("EpfEmployer0" , "E.P.F Employer 12%");
                                pdfFormFields.SetField("EtfEmployer0" , "E.T.F Employer 3%");
                                string epf = "E.P.F - 12%";

                                string etf = "ETF 3 %";
                                if (payslipDetail[i].ContributionCode.ToUpper() == epf)
                                {
                                    pdfFormFields.SetField("EpfEmployer", payslipDetail[i].Amount.ToString());
                                }
                                else if (payslipDetail[i].ContributionCode.ToUpper() == etf)
                                {
                                    pdfFormFields.SetField("EtfEmployer", payslipDetail[i].Amount.ToString());
                                }
                            }
                          
                        }
                        else
                        {
                            pdfFormFields.SetField("EpfEmployer0" , "");
                            pdfFormFields.SetField("EtfEmployer0" , "");
                            pdfFormFields.SetField("EpfEmployer", "");
                            pdfFormFields.SetField("EtfEmployer", "");
                        }
                    }

                    pdfFormFields.SetField("NoPayDays0", "No Pay Days");
                    pdfFormFields.SetField("NoPayDays", payslipHeader.LossOfPayDays.ToString());
                    if (payslipHeader.BranchID == 10003)
                    {
                        if (taxamount > 0)
                        {
                            pdfFormFields.SetField("NetPay", (TotalSalary - taxamount).ToString());
                        }
                        else
                        {
                            pdfFormFields.SetField("NetPay", (TotalSalary - TotalDeductions).ToString());
                        }
                    }
                    else
                    {
                        pdfFormFields.SetField("NetPay", (TotalSalary - TotalDeductions).ToString());
                    }
                    


                    for (int i = 0; i < payslipDetail.Count; i++)
                    {
                        if (payslipDetail[i].RegisterCode == "BASIC SALARY")
                        {
                            pdfFormFields.SetField("Desc" + i, payslipDetail[i].ContributionCode.ToString());
                            pdfFormFields.SetField("Item" + i, payslipDetail[i].Amount.ToString());
                        }
                    }
                    int j = 0;

                    for (int p = 0; p < payslipDetail.Count; p++)
                    {
                        if (payslipDetail[p].RegisterCode == "EMPLOYEE CONTRIBUTION")
                        {
                            pdfFormFields.SetField("Deduct" + j, payslipDetail[p].ContributionCode.ToString());
                            pdfFormFields.SetField("Amount" + j, payslipDetail[p].Amount.ToString());
                            j++;
                        }
                      
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion
        private static string MonthName(int? month)
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




    }
}