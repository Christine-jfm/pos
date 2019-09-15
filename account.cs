using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSFM
{
    class account
    {
        static string username;
        public static string Username
        {
            get { return username; }
            set { username = value; }
        }

        static int useraccountid;
        public static int UserAccountId
        {
            get { return useraccountid; }
            set { useraccountid = value; }
        }

        static string usertype;
        public static string Usertype
        {
            get { return usertype; }
            set { usertype = value; }
        }

        static string userFullName;
        public static string UserFullName
        {
            get { return userFullName; }
            set { userFullName = value; }
        }

        //----User

        static bool isUserAdd = false;
        public static bool IsUserAdd
        {
            get { return isUserAdd; }
            set { isUserAdd = value; }
        }

        static bool isUserEdit = false;
        public static bool IsUserEdit
        {
            get { return isUserEdit; }
            set { isUserEdit = value; }
        }

        static int selectUserId;
        public static int SelectUserId
        {
            get { return selectUserId; }
            set { selectUserId = value; }
        }

        //----Employee

        static bool isEmpAdd = false;
        public static bool IsEmpAdd
        {
            get { return isEmpAdd; }
            set { isEmpAdd = value; }
        }

        static bool isEmpEdit = false;
        public static bool IsEmpEdit
        {
            get { return isEmpEdit; }
            set { isEmpEdit = value; }
        }

        static int selectEmpId;
        public static int SelectEmpId
        {
            get { return selectEmpId; }
            set { selectEmpId = value; }
        }

        //----Employee

        static bool isProdAdd = false;
        public static bool isprodAdd
        {
            get { return isProdAdd; }
            set { isProdAdd = value; }
        }

        static bool isProdEdit = false;
        public static bool IsProdEdit
        {
            get { return isProdEdit; }
            set { isProdEdit = value; }
        }

        static int selectProdId;
        public static int SelectProdId
        {
            get { return selectProdId; }
            set { selectProdId = value; }
        }

        //trans
        static int transNo;
        public static int TransNo
        {
            get { return transNo; }
            set { transNo = value; }
        }

        static bool isTrans = false;
        public static bool IsTrans
        {
            get { return isTrans; }
            set { isTrans = value; }
        }

        static string orderType = "Dine-In";
        public static string OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }

        static string get_strUsername;
        public static string Get_strUsername
        {
            get { return get_strUsername; }
            set { get_strUsername = value; }
        }

        static string strTableNo;
        public static string StrTableNo
        {
            get { return strTableNo; }
            set { strTableNo = value; }
        }

        static bool isCategory = false;
        public static bool IsCategory
        {
            get { return isCategory; }
            set { isCategory = value; }
        }

        static string discNo;
        public static string DiscNo
        {
            get { return discNo; }
            set { discNo = value; }
        }

        static string discName;
        public static string DiscName
        {
            get { return discName; }
            set { discName = value; }
        }

        //supplier

        static bool isSupplierAdd = false;
        public static bool IsSupplierAdd
        {
            get { return isSupplierAdd; }
            set { isSupplierAdd = value; }
        }

        static bool isSupplierEdit = false;
        public static bool IsSupplierEdit
        {
            get { return isSupplierEdit; }
            set { isSupplierEdit = value; }
        }

        static int selectSupplierId;
        public static int SelectSupplierId
        {
            get { return selectSupplierId; }
            set { selectSupplierId = value; }
        }
    }
}
