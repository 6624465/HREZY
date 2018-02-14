using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class TravelClaimVm
    {
        public TravelClaimHeader claimHeader { get; set; }
        public List<TravelClaimDetail> claimDetail { get; set; }

        public List<TravelClaimDetailVm> travelClaimDetailHeader { get; set; }
        public List<TravelClaimDetailVm> claimDetailVm { get; set; }
    }

    public class TravelClaimDetailVm
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
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public int BranchId { get; set; }

        public TimeSpan DepartureTime { get; set; }
    }

    public class LookupObj
    {

      public  List<LooKUpList> lookUplist = new List<LooKUpList>() {
            new LooKUpList(){
                code="AIRFARE",
                description="AIRFARE"
            },
            new LooKUpList(){
                code="VISA",
                description="VISA"
            },
            new LooKUpList(){
                code="ACCOMDATION",
                description="ACCOMDATION"
            },
            new LooKUpList(){
                code="TAXIFARE-LOCAL",
                description="TAXIFARE-LOCAL"
            },new LooKUpList(){
                code="FOOD EXPENCES-LOCAL",
                description="FOOD EXPENCES-LOCAL"
            },
            new LooKUpList(){
                code="FOOD EXPENCES-OVERSEAS",
                description="FOOD EXPENCES-OVERSEAS"
            },
            new LooKUpList(){
                code="OTHER EXPENSES",
                description="OTHER EXPENSES"
            }
        };
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

    public class LooKUpList
    {
        public string description { get; set; }
        public string code { get; set; }
    }

}