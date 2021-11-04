using Model.UIControls.Tree;
using Model.ViewModels;
using Model.ViewModels.Category;
using Model.ViewModels.FeatureType;
using Model.ViewModels.Product;
using Model.ViewModels.SundryImage;

using System;
using System.Collections.Generic;

namespace Model.ApplicationDomainModels
{

    public class _ImageType
    {
        public string this[byte index]
        {
            get
            {
                if (index == HomePageImage)
                    return HomePageImageDesc;
                else
                if (index == LeftSponsorImage)
                    return LeftSponsorImageDesc;
                else
                if (index == RightSponsorImage)
                    return RightSponsorImageDesc;

                return string.Empty;

            }
        }
        public byte this[string stringIndex]
        {
            get
            {

                if (stringIndex == HomePageImageDesc)
                    return HomePageImage;
                else
                if (stringIndex == LeftSponsorImageDesc)
                    return LeftSponsorImage;
                else
                if (stringIndex == RightSponsorImageDesc)
                    return RightSponsorImage;

                throw new Exception();

            }
        }
        public byte HomePageImage { get { return 0; } }
        public string HomePageImageDesc { get { return "Home Page Image"; } }
        public byte LeftSponsorImage { get { return 1; } }
        public string LeftSponsorImageDesc { get { return "Left Sponsor Image"; } }
        public byte RightSponsorImage { get { return 2; } }
        public string RightSponsorImageDesc { get { return "Right Sponsor Image"; } }

    }
    public static class ConstantObjects
    {
        public static List<VmActiveLanguage> ActiveLanguageList;
        public static IEnumerable<VmLanguageDictionaryList> LanguageDictionaryList { get; set; }

        public static bool CD9C83EF5A204B71BC2A1B105A1EC0F1 { get; set; }

        private static _ImageType _imageType;
        public static _ImageType ImageType
        {
            get
            {
                if (_imageType == null)
                {
                    _imageType = new _ImageType();
                }
                return _imageType;
            }
        }

        public static Dictionary<string, IEnumerable<SmUserRoles>> LoginUsers = new Dictionary<string, IEnumerable<SmUserRoles>>();
        public enum SundryImageType
        {
            HomePage = 0,
            PackageItem = 1
        };
        public enum PackageItemType
        {
            None = -1,
            New = 0,
            Discount = 1,
            Popular = 2,
            Category = 3
        };

        public enum InvoiceStatus
        {
            Cancel = -1,
            Failed = 0,
            Successful = 1,
        };
        public enum ProductSortType
        {
            HighestToLowestPrice = 0,
            LowestToHighestPrice = 1
        };
        public enum ImageColumnType
        {
            OneColumn = 1,
            TwoColumn = 2,
            ThreeColumn = 3
        }

        public enum JournalType
        {
            Journal = 0,
            Conference = 1,
            Book = 2,
            Course = 3
        };

        public enum VolumeTypes
        {
            Root = 0,
            Volume = 1,
            Issue = 2,
            AssignArticle = 3
        };

        public enum MessageType
        {
            ClearMessage = 0,
            UserActivated = 1,
            AssignedToEditor = 2,
            AcceptedByEditor = 3,
            RejectedByEditor = 4,
            NeedToReviseByEditor = 5,
            AssignedToReviewer = 6,
            AcceptedByReviewer = 7,
            RejectedByReviewer = 8,
            NeedToReviseByReviewer = 9

        };

        public enum SystemRoles
        {
            Admin = 0,
            Member = 1,
        }

        public static string GetSystemRolesId(SystemRoles systemRoles)
        {
            switch (systemRoles)
            {
                case SystemRoles.Admin:
                    return "652a69dc-d46c-4cbf-ba28-8e7759b37752";
                case SystemRoles.Member:
                    return "f522e425-0407-4fe5-894e-93dbdcfd1a2c";

                default:
                    return "";
            }
        }
        public static string GetSystemRolesString(SystemRoles systemRoles)
        {
            switch (systemRoles)
            {
                case SystemRoles.Admin:
                    return "Admin";

                case SystemRoles.Member:
                    return "Member";

                default:
                    return "";
            }
        }

        public static TreeNode StaticMenuItemTreeNode { get; set; }
        public static List<VmCategory> StaticCategoryList { get; set; }
        public static List<VmCategoryFeatureType> StaticCategoryFeatureTypeList { get; set; }
        public static List<VmCountry> StaticCountryList { get; set; }
        public static List<VmShopProductFullInfo> StaticShopProductFullInfoList { get; set; }
        public static List<VmSundryImage> StaticSundryImageList { get; set; }

    }
}
