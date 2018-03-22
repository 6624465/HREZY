using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class TravelClaimVm
    {
        public TravelClaimHeaderVm claimHeader { get; set; }
        public List<TravelDetailAirfareVm> claimDetailAirfareVm { get; set; }
        public List<TravelDetailVisaVm> claimDetailVisaVm { get; set; }
        public List<TravelDetailAccomdationVm> claimDetailAccomdationVm { get; set; }
        public List<TravelDetailTaxiLocalVm> claimDetailTaxiLocalVm { get; set; }
        public List<TravelDetailTaxiOverseasVm> claimDetailTaxiOverseasVm { get; set; }
        public List<TravelDetailFoodLocalVm> claimDetailFoodLocalVm { get; set; }
        public List<TravelDetailFoodOverseasVm> claimDetailFoodOverseasVm { get; set; }
        public List<TravelDetailOtherExpensesVm> claimDetailOtherExpensesVm { get; set; }
        public List<TravelClaimDocumentVm> claimDocumentVm { get; set; }

    }

    public class SelectedTCVm
    {
        public bool IsChecked { get; set; }
        public int BranchId { get; set; }
        public int TravelClaimId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TotalAmtPaid { get; set; }
    }

    public class TravelClaimHeaderVm : TravelClaimHeader {
        public decimal? claimDetailAirfareTotal { get; set; }
        public decimal? claimDetailVisaTotal { get; set; }
        public decimal? claimDetailAccomdationTotal { get; set; }
        public decimal? claimDetailTaxiLocalTotal { get; set; }
        public decimal? claimDetailTaxiOverseasTotal { get; set; }
        public decimal? claimDetailFoodLocalTotal { get; set; }

        public decimal? claimDetailFoodOverseasTotal { get; set; }
        public decimal? claimDetailOtherExpensesTotal { get; set; }
    }

    public class TravelDetailAirfareVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }

        public int? DepartureTime { get; set; }

        public string ClaimNo { get; set; }
    }

    public class TravelDetailVisaVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }

        public int? DepartureTime { get; set; }
        public string ClaimNo { get; set; }
    }

    public class TravelDetailAccomdationVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }

    public class TravelDetailTaxiLocalVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }

    public class TravelDetailTaxiOverseasVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }

    public class TravelDetailFoodLocalVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }

    public class TravelDetailFoodOverseasVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }

    public class TravelDetailOtherExpensesVm
    {
        public string LookUpCode { get; set; }
        public int TravelClaimDetailId { get; set; }
        public int? TravelClaimId { get; set; }
        public string Category { get; set; }
        public DateTime? TravelDate { get; set; }
        public string Perticulars { get; set; }
        public bool? Receipts { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? TotalInSGD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public int BranchId { get; set; }
        public string ClaimNo { get; set; }
        public int? DepartureTime { get; set; }
    }
    public class LookupObj
    {
        public List<string> Code = new List<string>() {
            "AIRFARE",
            "VISA",
            "ACCOMDATION",
            "TAXIFARE-LOCAL",
            "FOOD EXPENCES-LOCAL",
              "FOOD EXPENCES-OVERSEAS",
              "OTHER EXPENSES"
        };
    }
    public class TravelClaimDocumentVm
    {
        public int DocumentDetailId { get; set; }
        public int DocumentType { get; set; }
        public string DocumentDescription { get; set; }
        public HttpPostedFileBase Document { get; set; }
        public string fileName { get; set; }
        public int EmployeeId { get; set; }
    }

}